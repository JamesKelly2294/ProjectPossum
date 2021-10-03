using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    public GameObject ObjectiveFrame;
    public TextMeshProUGUI ObjectiveTrackerText;

    // TODO: Refactor this class to be data based, rather than code based

    private void ShowObjective(string objectiveText)
    {
        ObjectiveFrame.SetActive(true);
        ObjectiveTrackerText.text = objectiveText;
        AudioManager.Instance.Play("GUI/Objective/Appear");
    }

    public void ObjectiveCompleteSuccess()
    {
        ObjectiveFrame.SetActive(false);
        ObjectiveTrackerText.text = "";
        AudioManager.Instance.Play("GUI/Objective/Appear");
    }

    public void ObjectiveCompleteFailure()
    {
        ObjectiveFrame.SetActive(false);
        ObjectiveTrackerText.text = "";
        AudioManager.Instance.Play("GUI/Objective/Appear");
    }

    public void HideObjective()
    {
        ObjectiveFrame.SetActive(false);
        ObjectiveTrackerText.text = "";
    }

    public void ShowFindFarmObjective()
    {
        ShowObjective("Go to the farm, just out the back.");
    }

    public void ShowInvestigateDevilObjective()
    {
        ShowObjective("Investigate the individual behind the farm.");
    }
}
