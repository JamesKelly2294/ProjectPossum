using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    public PlayerMovement PlayerMovement;

    public bool MouseDrivenHighlighting = true;
    public Vector2Int TileCoordinate;
    public Vector2Int HighlightedTileCoordinate;
    public GameObject TileHighlight;
    public bool TileHighlightInRange;

    // Start is called before the first frame update
    void Start()
    {
        // Very professional
        var animators = GetComponentsInChildren<Animator>();
        Vector2 facing = Vector2.right;
        foreach (var animator in animators)
        {
            animator.SetFloat("Horizontal_Facing", facing.x);
            animator.SetFloat("Vertical_Facing", facing.y);
        }
    }

    void PollPlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SoilManager.Instance.PlantCropAtCoordinate(CropType.BlueFlower, HighlightedTileCoordinate);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SoilManager.Instance.PlantCropAtCoordinate(CropType.OrangeFlower, HighlightedTileCoordinate);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SoilManager.Instance.PlantCropAtCoordinate(CropType.PurpleFlower, HighlightedTileCoordinate);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SoilManager.Instance.PlantCropAtCoordinate(CropType.YellowFlower, HighlightedTileCoordinate);
        }
    }

    Tuple<Vector2Int, bool> PollMouseTilePosition()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position;

        Vector2Int mouseTilePosition = new Vector2Int(Mathf.FloorToInt(mousePosition.x), Mathf.FloorToInt(mousePosition.y));
        const float mouseRange = 3.0f;
        bool inRange = Vector2.Distance(mousePosition, playerPosition) < mouseRange;
        
        return new Tuple<Vector2Int, bool>(mouseTilePosition, inRange);
    }
    
    void Update()
    {
        PollPlayerInput();
        DetermineTileHighlight();
    }

    void TryToPlantCrop()
    {
        if (TileHighlightInRange)
        {
            SoilManager.Instance.PlantCropAtCoordinate(CropType.BlueFlower, HighlightedTileCoordinate);
        }
    }

    void DetermineTileHighlight()
    {
        TileCoordinate = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));

        if (!MouseDrivenHighlighting)
        {
            HighlightedTileCoordinate = TileCoordinate + PlayerMovement.CalculateOffsetVector2IntForFacing(PlayerMovement.FacingDirection);
            TileHighlight.transform.position = new Vector3(HighlightedTileCoordinate.x + 0.5f, HighlightedTileCoordinate.y - 0.5f, TileHighlight.transform.position.z);

            if (SoilManager.Instance.CropCoordinateIsValid(HighlightedTileCoordinate))
            {
                BeginTileHighlighting();
            }
            else
            {
                StopTileHighlighting();
            }
        }
        else
        {
            var results = PollMouseTilePosition();
            bool inRange = results.Item2;
            Vector2Int MouseTileCoordinate = results.Item1;
            HighlightedTileCoordinate = MouseTileCoordinate;
            TileHighlightInRange = inRange;
            
            if (inRange && SoilManager.Instance.CropCoordinateIsValid(HighlightedTileCoordinate))
            {
                TileHighlight.transform.position = new Vector3(HighlightedTileCoordinate.x, HighlightedTileCoordinate.y, TileHighlight.transform.position.z);
                BeginTileHighlighting();
            }
            else
            {
                StopTileHighlighting();
            }
        }

        if (Input.GetMouseButton(0))
        {
            TryToPlantCrop();
        }
    }

    public void BeginTileHighlighting()
    {
        TileHighlight.SetActive(true);
    }

    public void StopTileHighlighting()
    {
        TileHighlight.SetActive(false);
    }
}
