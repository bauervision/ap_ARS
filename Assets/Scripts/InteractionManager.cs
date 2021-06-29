using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AMPS_DataModel;
public class InteractionManager : MonoBehaviour
{
    [Header("Base Elements")]
    public OnlineMaps MapsInstance;
    public GameObject InteractiveMap;
    public GameObject InitialMenu;
    public GameObject LoadingMenu;
    public GameObject NewMenu;
    public GameObject CloseUI;

    [Header("Prefabs to spawn on the map")]
    public GameObject PointPrefab;

    [Header("Path Drawing Parameters")]
    public float pathLineSize = 10f;

    public Vector2 pathUvScale = new Vector2(2, 1);
    public Material pathMaterial;
    private Vector2[] _pathCoords;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Mesh _mesh;

    #region Public Methods

    #region Private Variables
    List<OnlineMapsMarker3D> missionMarkers = new List<OnlineMapsMarker3D>();

    private Mission _currentLoadedMission;
    private float _size;

    #endregion
    #region UI Methods



    public void NewMission()
    {
        Debug.Log("New Mission!");
    }

    public void LoadMission()
    {
        Debug.Log("Load Mission!");
        //ToggleLoadingMenu();
        //InitialMenu.SetActive(!LoadingMenu.activeInHierarchy);
    }

    #endregion

    #region Voice Commands

    public void Voice_MainMenu()
    {
        Debug.Log("You said: Main Menu!");
        // hide all UI menus
        HideAllUI();
        //unhide main menu
        InitialMenu.SetActive(true);
        // hide the map
        InteractiveMap.SetActive(false);
    }
    public void Voice_NewMission()
    {
        Debug.Log("You said: New Mission!");
        ToggleNewMenu();
        InitialMenu.SetActive(!NewMenu.activeInHierarchy);
    }

    public void Voice_LoadMission()
    {
        Debug.Log("You said: Load Mission!");
        ToggleLoadingMenu();
        InitialMenu.SetActive(!LoadingMenu.activeInHierarchy);
    }

    public void Voice_LoadSelected()
    {
        Debug.Log("You said: Load Selected!");
        // hide loading menu
        LoadingMenu.SetActive(false);
        //launch the mission
        LaunchSelectedMission();
    }


    #endregion
    #endregion

    #region Private Methods

    void LaunchSelectedMission()
    {
        //show the close UI
        CloseUI.SetActive(true);
        //show the map
        InteractiveMap.SetActive(true);

        // set the local refernce of this variable
        _currentLoadedMission = DataManager.instance._currentMission;

        if (_currentLoadedMission.Points.Length < 1)
            return;

        // get the coords from the first Point in the Routes object stored in the mission
        double mLat = _currentLoadedMission.Points[0].Coordinates.Latitude.value;
        double mLng = _currentLoadedMission.Points[0].Coordinates.Longitude.value;
        // and now set them as the coords for the mission
        _currentLoadedMission.latitude = mLat;
        _currentLoadedMission.longitude = mLng;

        // set the map to the location of the mission
        //TODO: figure out a way to set the zoom to show the entire mission route
        int zoom = 11;
        OnlineMaps.instance.SetPositionAndZoom(mLng, mLat, zoom);

        // Subscribe to events of map.
        OnlineMaps.instance.OnChangePosition += UpdateLine;
        OnlineMaps.instance.OnChangeZoom += UpdateLine;

        // create markers for all of the points in the missions
        HandleMissionPoints();
    }

    void HandleMissionPoints()
    {
        // create the mesh line that will be drawn between points
        GameObject _lineContainer = new GameObject("Path: dotted line");
        _meshFilter = _lineContainer.AddComponent<MeshFilter>();
        _meshRenderer = _lineContainer.AddComponent<MeshRenderer>();
        _mesh = _meshFilter.sharedMesh = new Mesh();
        _mesh.name = "Dotted Line";
        _mesh.MarkDynamic();
        _meshRenderer.sharedMaterial = pathMaterial;


        // run through all of the points stored in the mission
        foreach (Point point in _currentLoadedMission.Points)
        {
            //create the vector2 for the coords
            Vector2 pointCoords = new Vector2((float)point.Coordinates.Longitude.value, (float)point.Coordinates.Latitude.value);
            //create the 3d marker
            OnlineMapsMarker3D cur3dPointMarker = OnlineMapsMarker3DManager.CreateItem(pointCoords, PointPrefab);
            // store the data in the marker
            cur3dPointMarker["data"] = point;
            // store the marker
            missionMarkers.Add(cur3dPointMarker);

        }

        UpdateLine();
        // now push the line up a little to get it off the map
        float heightOffset = 0.06f;
        _lineContainer.transform.localPosition = new Vector3(0, heightOffset, 0);
    }

    private void UpdateLine()
    {
        if (_currentLoadedMission?.Points.Length < 1)
            return;

        _size = pathLineSize;

        Point[] missionPoints = _currentLoadedMission.Points;

        float totalDistance = 0;
        Vector3 lastPosition = Vector3.zero;

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> triangles = new List<int>();

        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < missionPoints.Length; i++)
        {
            //get the vector2 for this point
            Vector2 pointCoords = new Vector2((float)missionPoints[i].Coordinates.Longitude.value, (float)missionPoints[i].Coordinates.Latitude.value);

            // Get world position by coordinates
            Vector3 position = OnlineMapsTileSetControl.instance.GetWorldPosition(pointCoords);
            positions.Add(position);

            if (i != 0)
            {
                // Calculate angle between coordinates.
                float a = OnlineMapsUtils.Angle2DRad(lastPosition, position, 90);

                // Calculate offset
                Vector3 off = new Vector3(Mathf.Cos(a) * pathLineSize, 0, Mathf.Sin(a) * pathLineSize);

                // Init vertices, normals and triangles.
                int vCount = vertices.Count;

                vertices.Add(lastPosition + off);
                vertices.Add(lastPosition - off);
                vertices.Add(position + off);
                vertices.Add(position - off);

                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                triangles.Add(vCount);
                triangles.Add(vCount + 3);
                triangles.Add(vCount + 1);
                triangles.Add(vCount);
                triangles.Add(vCount + 2);
                triangles.Add(vCount + 3);

                totalDistance += (lastPosition - position).magnitude;
            }

            lastPosition = position;
        }

        float tDistance = 0;

        for (int i = 1; i < positions.Count; i++)
        {
            float distance = (positions[i - 1] - positions[i]).magnitude;

            // Updates UV
            uvs.Add(new Vector2(tDistance / totalDistance, 0));
            uvs.Add(new Vector2(tDistance / totalDistance, 1));

            tDistance += distance;

            uvs.Add(new Vector2(tDistance / totalDistance, 0));
            uvs.Add(new Vector2(tDistance / totalDistance, 1));
        }

        // Update mesh
        _mesh.vertices = vertices.ToArray();
        _mesh.normals = normals.ToArray();
        _mesh.uv = uvs.ToArray();
        _mesh.triangles = triangles.ToArray();
        _mesh.RecalculateBounds();

        // Scale texture
        Vector2 scale = new Vector2(totalDistance / pathLineSize, 1);
        scale.Scale(pathUvScale);
        _meshRenderer.material.mainTextureScale = scale;
    }

    void HideAllUI()
    {
        InitialMenu.SetActive(false);
        NewMenu.SetActive(false);
        LoadingMenu.SetActive(false);
    }

    void ToggleLoadingMenu()
    {
        LoadingMenu.SetActive(!LoadingMenu.activeInHierarchy);
        // if the new menu was showing, hide it
        if (NewMenu.activeInHierarchy)
            NewMenu.SetActive(false);

    }

    void ToggleNewMenu()
    {
        NewMenu.SetActive(!NewMenu.activeInHierarchy);
        // if the loading menu was showing, hide it
        if (LoadingMenu.activeInHierarchy)
            LoadingMenu.SetActive(false);

    }


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        LoadingMenu.SetActive(false);
        NewMenu.SetActive(false);
        CloseUI.SetActive(false);
        // hide the map
        InteractiveMap.SetActive(false);



    }

    private void Update()
    {
        // If size changed, then update line.
        if (System.Math.Abs(_size - pathLineSize) > float.Epsilon) UpdateLine();
    }


}
