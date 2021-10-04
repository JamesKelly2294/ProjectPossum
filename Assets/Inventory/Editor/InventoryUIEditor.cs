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

#if UNITY_EDITOR
            invUI.DebugSpawnItemFromLibrary();
#endif
        }

        if (GUILayout.Button("Remove Selected Item") && Application.isPlaying)
        {
#if UNITY_EDITOR
            invUI.DebugRemoveSelectedItem();
#endif
        }
    }
}
