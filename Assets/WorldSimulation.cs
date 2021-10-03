using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSimulation : MonoBehaviour
{
    public FinalizedDialogScript StartingScript;

    // Start is called before the first frame update
    void Start()
    {
        CharacterDialog.Instance.BeginScript(StartingScript);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
