using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SoilManager : MonoBehaviour
{
    public Tilemap soilTileMap;
    public Tilemap plantTileMap;
    //public TileBase soilTile;
    public TileBase activePlantTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        var mousePosition = Input.mousePosition;
        var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var tilePosition = soilTileMap.WorldToCell(worldPosition);
        if(plantTileMap.GetTile(tilePosition) != null)
        {
            //soilTileMap.SetTile(tilePosition, null);
            plantTileMap.SetTile(tilePosition, null);
        }
        else
        {
            //soilTileMap.SetTile(tilePosition, soilTile);
            plantTileMap.SetTile(tilePosition, activePlantTile);
        }
    }
}
