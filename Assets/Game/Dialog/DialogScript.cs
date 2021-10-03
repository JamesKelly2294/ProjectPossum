using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogSpeaker
{
    public int speaker;
    [TextArea(5, 20)]
    public string dialog;
}

[Serializable]
public class DialogEvent
{
    public int scriptIndex;
    public string eventName;
}


[CreateAssetMenu(fileName = "Dialog", menuName = "Character Interactions/DialogScript", order = 1)]
public class DialogScript : ScriptableObject
{
    public string title;
    public List<DialogEvent> events;
    public List<DialogSpeaker> script;
}
