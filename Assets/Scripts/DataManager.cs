using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public TextMeshProUGUI CurrentMissionName;

    public TextMeshProUGUI MissionTotal;

    public TMP_Dropdown missionDropDown;

    public List<SingleMission> MissionsFromList;
    #endregion

    #region Private Members
    SingleMission _currentMission;
    private int _currentMissionIndex = 0;
    private int _currentStepIndex = 1;
    private List<TMP_Dropdown.OptionData> _dropDownOptions = new List<TMP_Dropdown.OptionData>();
    #endregion

    void Awake()
    {
        InitUI();
    }


    // Update is called once per frame
    void Update()
    {

    }


    #region Public Methods

    private void InitUI()
    {
        // now we need to read all the data from the json IF we have one loaded
        if (MissionsJSON != null)
        {
            MissionData missionsInJson = JsonUtility.FromJson<MissionData>(MissionsJSON.text);

            // handle the drop down menu
            TMP_Dropdown.OptionData firstOption = new TMP_Dropdown.OptionData();
            firstOption.text = "Select Mission...";
            _dropDownOptions.Add(firstOption);


            // now process each procedure and dump them into a List<SingleProcedure> for better access.
            foreach (SingleMission mission in missionsInJson.missions)
            {
                MissionsFromList.Add(mission);

                // handle the dropdown
                TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
                newOption.text = mission._Name;
                _dropDownOptions.Add(newOption);
            }

            missionDropDown.options = _dropDownOptions;
            // now that we have our data, load up the first procedure
            ProcessCurrentMission();


        }
        else
            Debug.LogError("No Procedure JSON loaded. Please add to continue");
    }


    public void SetMissionIndexToLoad(int index)
    {
        _currentMissionIndex = index - 1;// remove 1 because of the "Select Mission..." option
        ProcessCurrentMission();
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

        _currentMission = MissionsFromList[(_currentMissionIndex)];// take 1 away because of the "Select Mission..." option

        MissionTotal.text = totalMissions;

        CurrentMissionName.text = (missionDropDown.value != 0) ? _currentMission._Name : "Select from the list ";

        //CurrentMissionCheckpointCount.text = (_currentMission._Checkpoints.Length - 1).ToString();

    }

    #endregion
}
