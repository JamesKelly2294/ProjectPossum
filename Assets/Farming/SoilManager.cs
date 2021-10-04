using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class CropPrefab
{
    public CropType type;
    public GameObject prefab;
}

public class SoilManager : MonoBehaviour, ICropDelegate
{
    public static SoilManager Instance
    {
        get; private set;
    }

    public List<Rect> FarmPlots;
    public List<CropPrefab> CropPrefabs;

    private Dictionary<Vector2Int, Crop> _crops;

    public void Awake()
    {
        Instance = this;

        _crops = new Dictionary<Vector2Int, Crop>();
    }

    public GameObject GetCropPrefab(CropType type)
    {
        foreach(var c in CropPrefabs)
        {
            if (c.type == type)
            {
                return c.prefab;
            }
        }
        return null;
    }

    public bool CropCoordinateIsValid(Vector2 coordinate)
    {
        foreach(var rect in FarmPlots)
        {
            if (rect.Contains(coordinate))
            {
                return true;
            }
        }

        return false;
    }

    public bool PlantCropAtCoordinate(CropType cropType, Vector2Int coordinate)
    {
        if (!CropCoordinateIsValid(coordinate))
        {
            return false;
        }

        if (_crops.ContainsKey(coordinate))
        {
            return false;
        }

        GameObject cropGO = Instantiate(GetCropPrefab(cropType));
        cropGO.transform.parent = transform;
        cropGO.transform.position = new Vector3(coordinate.x, coordinate.y, 0.0f) + new Vector3(+0.5f, +0.5f, 0.0f);

        var crop = cropGO.GetComponent<Crop>();
        crop.CropDelegate = this;
        crop.Coordinate = coordinate;

        _crops[coordinate] = crop;

        AudioManager.Instance.Play("SFX/PlantCrop", false, 0.8f, 1.2f, 0.5f, 0.6f);

        return true;
    }
    
    public void CropWasHarvested(Crop crop)
    {
        Debug.Log(crop + " was harvested!");

        if (_crops[crop.Coordinate] == crop)
        {
            _crops.Remove(crop.Coordinate);
        }
        else
        {
            Debug.LogError("SoilManager:ICropDelegate -> received message that crop finished growing that is not in our map!");
        }
    }

    public void CropDidFinishGrowing(Crop crop)
    {
        Debug.Log(crop + " finished growing!");
    }
}
