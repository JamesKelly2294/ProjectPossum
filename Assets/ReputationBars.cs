using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationBars : MonoBehaviour
{
    public GameObject PatronReputationBar;
    public GameObject DevilReputationBar;
    public GameObject ApothecaryReputationBar;

    //GUI/Appear
    
    private void ShowReputationBar(GameObject reputationBar)
    {
        reputationBar.SetActive(true);
        AudioManager.Instance.Play("GUI/Appear");
    }

    private void HideReputationBar(GameObject reputationBar)
    {
        reputationBar.SetActive(false);
        AudioManager.Instance.Play("GUI/Appear");
    }

    public void ShowApothecaryReputationBar()
    {
        ShowReputationBar(ApothecaryReputationBar);
    }

    public void HideApothecaryReputationBar()
    {
        HideReputationBar(ApothecaryReputationBar);
    }
    
    public void ShowDevilReputationBar()
    {
        ShowReputationBar(DevilReputationBar);
    }

    public void HideDevilReputationBar()
    {
        HideReputationBar(DevilReputationBar);
    }

    public void ShowPatronReputationBar()
    {
        ShowReputationBar(PatronReputationBar);
    }

    public void HidePatronReputationBar()
    {
        HideReputationBar(PatronReputationBar);
    }
}
