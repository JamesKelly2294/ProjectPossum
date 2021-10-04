using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Crop))]
public class CropEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Crop c = (Crop)target;
        if (GUILayout.Button("Harvest") && Application.isPlaying)
        {
            c.Harvest();
        }
    }
}
