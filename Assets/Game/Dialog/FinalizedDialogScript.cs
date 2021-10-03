using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinalizedDialogScript : MonoBehaviour
{
    public List<Character> speakers;
    public DialogScript dialogScript;

    public bool IsBeingPlayed;
    public bool CompletedOnce;

    public UnityEvent OnScriptFinished;
    public UnityEvent OnScriptBegan;

    public string Title
    {
        get
        {
            return dialogScript.title ?? "Dialog";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}