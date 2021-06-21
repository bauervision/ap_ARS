using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#region Serialized Classes
[System.Serializable]
public class MissionAsset
{
    public string _country;
    public string _name;
    public string _type;// will become a Type Object


}

[System.Serializable]
public class MissionPhase
{
    public string _name;

}

[System.Serializable]
public class MissionCheckPoint
{
    public int _checkpointNumber;
    public string _timeStamp;

}

[System.Serializable]
public class SingleMission
{
    public MissionAsset[] _Assets;
    public MissionCheckPoint[] _Checkpoints;
    public string _Location;
    public string _Name;
    public MissionPhase[] _Phases;



}

[System.Serializable]
public class MissionData
{
    public SingleMission[] missions;
}
#endregion


public class DataManager : MonoBehaviour
{

    #region Public Members
    public TextAsset MissionsJSON;// replaced eventually with incoming data
    public TextMeshProUGUI CurrentMissionCheckpointCount;
    public TextMeshProUGUI CurrentMissionName;
    public TextMeshProUGUI CurrentMissionStep;
    public TextMeshProUGUI MissionTotal;


    public List<SingleMission> MissionsFromList;
    #endregion

    #region Private Members
    SingleMission _currentMission;
    private int _currentMissionIndex = 0;
    private int _currentStepIndex = 1;
    #endregion

    void Awake()
    {
        InitUI();
    }


    // Update is called once per frame
    void Update()
    {

    }


    #region Init Methods

    private void InitUI()
    {
        // now we need to read all the data from the json IF we have one loaded
        if (MissionsJSON != null)
        {
            MissionData missionsInJson = JsonUtility.FromJson<MissionData>(MissionsJSON.text);

            // now process each procedure and dump them into a List<SingleProcedure> for better access.
            foreach (SingleMission mission in missionsInJson.missions)
                MissionsFromList.Add(mission);

            // now that we have our data, load up the first procedure
            ProcessCurrentMission();


        }
        else
            Debug.LogError("No Procedure JSON loaded. Please add to continue");
    }

    #endregion

    #region Private Methods
    ///<summary>Sets the procedure name and all of its steps based on whatever _currentProcedureIndex has been set to, as well
    ///showing the current step within total steps of the procedure  </summary>
    private void ProcessCurrentMission()
    {
        // handle displaying the current mission
        // setup for displaying the CurrentProcedureStepCount string
        string curMission = _currentStepIndex.ToString();
        string totalMissions = (MissionsFromList.Count).ToString();

        _currentMission = MissionsFromList[_currentMissionIndex];
        //CurrentMissionName.text = _currentMission._Name;
        MissionTotal.text = totalMissions;
        //CurrentMissionCheckpointCount.text = (_currentMission._Checkpoints.Length - 1).ToString();

    }

    #endregion
}
