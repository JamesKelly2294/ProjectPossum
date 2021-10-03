using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterDialog))]
public class CharacterDialogEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CharacterDialog cd = (CharacterDialog)target;
        if (GUILayout.Button("Start Script") && Application.isPlaying)
        {
            cd.BeginScript(cd.ActiveScript);
        }

        if (GUILayout.Button("Next Line") && Application.isPlaying)
        {
            cd.DisplayNextScriptLine();
        }
    }
}
