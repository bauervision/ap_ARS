using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#region Serialized Classes
[System.Serializable]
public class ProcedureStep
{
    public bool completed;
    ///<summary> The actual step that must be performed </summary>
    public string step;

}

[System.Serializable]
public class SingleProcedure
{
    public bool procedureCompleted;
    public string procedureName;
    public ProcedureStep[] procedureSteps;
}

[System.Serializable]
public class ProcedureList
{
    public SingleProcedure[] procedures;
}
#endregion

///<summary>Class used to read procedures from a JSON file and update the AR UI element </summary>
public class DynamicProcedure : MonoBehaviour
{

    #region Public Members
    public TextMeshPro CurrentProcedureStepCount;
    public TextMeshPro CurrentProcedureName;
    public TextMeshPro CurrentProcedureStep;
    public TextMeshPro CurrentProcedureComplete;
    public TextAsset ProcedureJSON;
    public List<SingleProcedure> ProceduresFromList;
    #endregion

    #region Private Members
    private int _currentProcedureIndex = 0;
    private int _currentStepIndex = 1;
    #endregion
    void Awake()
    {
        // make sure we have set what we need on our UI
        if (CurrentProcedureStepCount == null)
            CurrentProcedureStepCount = GameObject.Find("ProcedureCount").GetComponent<TextMeshPro>();

        if (CurrentProcedureName == null)
            CurrentProcedureName = GameObject.Find("ProcedureName").GetComponent<TextMeshPro>();

        if (CurrentProcedureStep == null)
            CurrentProcedureStep = GameObject.Find("ProcedureStep").GetComponent<TextMeshPro>();

        if (CurrentProcedureComplete == null)
            CurrentProcedureComplete = GameObject.Find("ProcedureComplete").GetComponent<TextMeshPro>();

        // now we need to read all the data from the json IF we have one loaded
        if (ProcedureJSON != null)
        {
            ProcedureList proceduresInJson = JsonUtility.FromJson<ProcedureList>(ProcedureJSON.text);

            // now process each procedure and dump them into a List<SingleProcedure> for better access.
            foreach (SingleProcedure procedure in proceduresInJson.procedures)
                ProceduresFromList.Add(procedure);

            // now that we have our data, load up the first procedure
            ProcessCurrentProcedure();
        }
        else
            Debug.LogError("No Procedure JSON loaded. Please add to continue");

    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("right"))
            NextProcedure();

        if (Input.GetKeyUp("left"))
            PrevProcedure();

        if (Input.GetKeyUp("up"))
            ProcedureStepComplete();


    }

    #region Public Methods

    ///<summary>Called from the UI buttons, simply increments _currentProcedureIndex and cycles back to zero when it reaches the end of ProceduresFromList  </summary>
    public void NextProcedure()
    {
        _currentProcedureIndex++;

        // always start from the first step of the procedure
        _currentStepIndex = 1;

        if (_currentProcedureIndex > (ProceduresFromList.Count - 1))
            _currentProcedureIndex = 0;

        ProcessCurrentProcedure();
    }

    ///<summary>Called from the UI buttons, simply decrements _currentProcedureIndex and cycles back to ProceduresFromList.Count when it reaches zero   </summary>
    public void PrevProcedure()
    {
        _currentProcedureIndex--;

        // always start from the first step of the procedure
        _currentStepIndex = 1;

        if (_currentProcedureIndex < 0)
            _currentProcedureIndex = (ProceduresFromList.Count - 1);

        ProcessCurrentProcedure();

    }

    ///<summary>Triggered from the Voice Command "Next Step", simply increments _currentStepIndex without completing the step  </summary>
    public void NextStep()
    {
        _currentStepIndex++;

        // if we have tried to skip steps past the end of this procedure, go back to the start
        if (_currentStepIndex > ProceduresFromList[_currentProcedureIndex].procedureSteps.Length - 1)
            _currentStepIndex = 1;

        ProcessCurrentProcedure();
    }

    ///<summary>Triggered from the Voice Command "Last Step", simply decrements _currentStepIndex without completing the step  </summary>
    public void LastStep()
    {
        _currentStepIndex--;

        // if we have tried to skip steps past the end of this procedure, go back to the start
        if (_currentStepIndex < 1)
            _currentStepIndex = ProceduresFromList[_currentProcedureIndex].procedureSteps.Length - 1;

        ProcessCurrentProcedure();
    }

    ///<summary>Triggered from the Voice Command "Step Completed", increments _currentStepIndex and cycles to the next procedure if all
    /// steps in the current procedure has been finished.   </summary>
    public void ProcedureStepComplete()
    {
        // set this step as complete
        ProceduresFromList[_currentProcedureIndex].procedureSteps[_currentStepIndex].completed = true;

        _currentStepIndex++;

        // if we have completed all the steps in the current procedure, advance the _currentProcedureIndex so we can
        // load up the next procedure
        if (_currentStepIndex > ProceduresFromList[_currentProcedureIndex].procedureSteps.Length - 1)
        {
            // jump to the next procedure
            _currentProcedureIndex++;

            //if we are at the end of the procedure list, cycle back to the beginning
            if (_currentProcedureIndex > (ProceduresFromList.Count - 1))
                _currentProcedureIndex = 0;

            // reset the step index back to 1 so we can start from the beginning of this procedure
            _currentStepIndex = 1;

        }


        ProcessCurrentProcedure();

    }
    #endregion


    #region Private Methods
    ///<summary>Sets the procedure name and all of its steps based on whatever _currentProcedureIndex has been set to, as well
    ///showing the current step within total steps of the procedure  </summary>
    private void ProcessCurrentProcedure()
    {
        // setup for displaying the CurrentProcedureStepCount string
        string curStep = _currentStepIndex.ToString();
        string totalSteps = (ProceduresFromList[_currentProcedureIndex].procedureSteps.Length - 1).ToString();
        CurrentProcedureStepCount.text = $"{curStep} / {totalSteps}";
        // set the name
        CurrentProcedureName.text = ProceduresFromList[_currentProcedureIndex].procedureName;
        // set the step directions
        CurrentProcedureStep.text = ProceduresFromList[_currentProcedureIndex].procedureSteps[_currentStepIndex].step;

        CurrentProcedureComplete.text = ProceduresFromList[_currentProcedureIndex].procedureSteps[_currentStepIndex].completed ? "Completed!" : "Not Completed";
    }

    #endregion

}
