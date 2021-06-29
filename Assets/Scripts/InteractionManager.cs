using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public GameObject InteractiveMap;
    public GameObject InitialMenu;
    public GameObject LoadingMenu;
    public GameObject NewMenu;
    public GameObject CloseUI;

    #region Public Methods

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
        Debug.Log("Loading Selected Mission: " + DataManager.instance._currentMission.name);
        //show the close UI
        CloseUI.SetActive(true);
        //show the map
        InteractiveMap.SetActive(true);

        // get the coords from the first Point in the Routes object stored in the mission
        double mLat = DataManager.instance._currentMission.Points[0].Coordinates.Latitude.value;
        double mLng = DataManager.instance._currentMission.Points[0].Coordinates.Longitude.value;

        // set the map to the location of the mission
        OnlineMaps.instance.SetPositionAndZoom(mLng, mLat, 12);
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

    // Update is called once per frame
    void Update()
    {

    }


}
