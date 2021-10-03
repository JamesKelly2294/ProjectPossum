using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogSpeaker
{
    public int speaker;
    public string dialog;
}

[CreateAssetMenu(fileName = "Dialog", menuName = "Character Interactions/DialogScript", order = 1)]
public class DialogScript : ScriptableObject
{
    public string title;
    public List<DialogSpeaker> script;
}
