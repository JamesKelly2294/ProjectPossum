using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatronManager))]
public class PatronManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PatronManager pm = (PatronManager)target;
        if (GUILayout.Button("Spawn Patron") && Application.isPlaying)
        {
            pm.SpawnPatron();
        }
    }
}
