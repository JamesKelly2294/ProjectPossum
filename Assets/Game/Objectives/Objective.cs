using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveType
{
    Unknown = 0,
    Tutorial_1,
}

[CreateAssetMenu(fileName = "Objective", menuName = "Gameplay/Objective", order = 1)]
public class Objective : ScriptableObject
{
    [TextArea(5, 20)]
    public string Text;

    public ObjectiveType Type = ObjectiveType.Unknown;
}