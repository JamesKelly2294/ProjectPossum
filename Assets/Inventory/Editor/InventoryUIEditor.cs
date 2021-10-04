using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(InventoryUI))]
public class InventoryUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        InventoryUI invUI = (InventoryUI)target;
        if (GUILayout.Button("Create InventoryItem") && Application.isPlaying)
        {
            invUI.SpawnInventoryItem();
        }
    }
}
