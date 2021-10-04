using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoilManager))]
public class SoilManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SoilManager sm = (SoilManager)target;
        if (GUILayout.Button("Plant Yellow") && Application.isPlaying)
        {
            sm.PlantCropAtCoordinate(CropType.YellowFlower, FindObjectOfType<PlayerCharacter>().HighlightedTileCoordinate);
        }
        if (GUILayout.Button("Plant Orange") && Application.isPlaying)
        {
            sm.PlantCropAtCoordinate(CropType.OrangeFlower, FindObjectOfType<PlayerCharacter>().HighlightedTileCoordinate);
        }
        if (GUILayout.Button("Plant Purple") && Application.isPlaying)
        {
            sm.PlantCropAtCoordinate(CropType.PurpleFlower, FindObjectOfType<PlayerCharacter>().HighlightedTileCoordinate);
        }
        if (GUILayout.Button("Plant Blue") && Application.isPlaying)
        {
            sm.PlantCropAtCoordinate(CropType.BlueFlower, FindObjectOfType<PlayerCharacter>().HighlightedTileCoordinate);
        }
    }
}
