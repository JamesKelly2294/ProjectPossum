using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CropType
{
    YellowFlower,
    OrangeFlower,
    PurpleFlower,
    BlueFlower,
}

public enum CropState
{
    Seedling,
    Growing,
    FullyGrown,
    Dying,
    Ruined
}

// not really a delegate, more of an observer. deal with itl.
public interface ICropDelegate
{
    void CropDidBeginGrowing(Crop crop);
    void CropDidFinishGrowing(Crop crop);
    void CropWasHarvested(Crop crop);
}

public class Crop : MonoBehaviour
{
    [Header("Customizable Stats")]
    public string Name = "Crop";
    public float GrowthPerSecond = 1.0f;
    public float GrowthRequiredTotal = 30.0f;
    public float GrowthRequiredForSprouting = 5.0f;
    public CropType CropType;
    public List<CropType> BoostedGrowthFromAdjacentCropTypes;
    public List<CropType> HinderedGrowthFromAdjacentCropTypes;

    [Header("Managed Stats")]
    public float CurrentGrowth = 0.0f;
    public int BoostingNeighbors = 0;
    public int HinderingNeighbors = 0;
    public CropState State = CropState.Seedling;
    public Vector2Int Coordinate;

    [Header("Visuals")]
    public Sprite SeedlingSprite;
    public List<Sprite> GrowthSprites;
    public Sprite GrownSprite;
    public Sprite InventoryIcon;
    public List<Sprite> BlipSprites;
    public SpriteRenderer BlipRenderer;
    public SpriteRenderer SpriteRenderer;
    
    public ICropDelegate CropDelegate;

    public void Start()
    {
        CalculateAdjacenyBonus();

        if (CropDelegate != null)
        {
            CropDelegate.CropDidBeginGrowing(this);
        }
    }

    public void Update()
    {
        CurrentGrowth += GrowthPerSecond * Time.deltaTime;

        if (State == CropState.Seedling)
        {
            if(CurrentGrowth > GrowthRequiredForSprouting)
            {
                State = CropState.Growing;
                SpriteRenderer.sprite = GrowthSprites[0];
            }
            else
            {
                SpriteRenderer.sprite = SeedlingSprite;
            }
        } 
        else if (State == CropState.Growing)
        {
            if (CurrentGrowth > GrowthRequiredTotal)
            {
                State = CropState.FullyGrown;
                SpriteRenderer.sprite = GrownSprite;

                if(CropDelegate != null)
                {
                    CropDelegate.CropDidFinishGrowing(this);
                }
            }
            else
            {
                float growthPerStage = (GrowthRequiredTotal - GrowthRequiredForSprouting) / GrowthSprites.Count;

                float GrowingStageProgress = CurrentGrowth - GrowthRequiredForSprouting;
                int spriteIndex = Mathf.FloorToInt(GrowingStageProgress / growthPerStage);
                SpriteRenderer.sprite = GrowthSprites[spriteIndex];
            }
        }
    }

    public void Harvest()
    {
        if (CropDelegate != null)
        {
            CropDelegate.CropWasHarvested(this);
        }
        Destroy(gameObject);
    }

    public List<Vector2Int> AdjacentTilesForBoostedGrowth()
    {
        Vector2Int center = Coordinate;
        List<Vector2Int> candidateNeighbors = new List<Vector2Int>();
        candidateNeighbors.Add(new Vector2Int(center.x - 1, center.y - 1));
        candidateNeighbors.Add(new Vector2Int(center.x + 1, center.y - 1));
        candidateNeighbors.Add(new Vector2Int(center.x + 1, center.y + 1));
        candidateNeighbors.Add(new Vector2Int(center.x - 1, center.y + 1));

        List<Vector2Int> adjacentTiles = new List<Vector2Int>();
        foreach (Vector2Int v in candidateNeighbors)
        {
            if (SoilManager.Instance.CropCoordinateIsValid(v))
            {
                adjacentTiles.Add(v);
            }
        }

        return adjacentTiles;
    }

    public List<Vector2Int> AdjacentTilesForHinderedGrowth()
    {
        Vector2Int center = Coordinate;
        List<Vector2Int> candidateNeighbors = new List<Vector2Int>();
        candidateNeighbors.Add(new Vector2Int(center.x - 1, center.y + 0));
        candidateNeighbors.Add(new Vector2Int(center.x + 1, center.y + 0));
        candidateNeighbors.Add(new Vector2Int(center.x + 0, center.y - 1));
        candidateNeighbors.Add(new Vector2Int(center.x + 0, center.y + 1));

        List<Vector2Int> adjacentTiles = new List<Vector2Int>();
        foreach (Vector2Int v in candidateNeighbors)
        {
            if (SoilManager.Instance.CropCoordinateIsValid(v))
            {
                adjacentTiles.Add(v);
            }
        }

        return adjacentTiles;
    }

    public List<Vector2Int> AdjacentTiles()
    {
        Vector2Int center = Coordinate;
        List<Vector2Int> candidateNeighbors = new List<Vector2Int>();
        candidateNeighbors.Add(new Vector2Int(center.x - 1, center.y - 1));
        candidateNeighbors.Add(new Vector2Int(center.x + 1, center.y - 1));
        candidateNeighbors.Add(new Vector2Int(center.x + 1, center.y + 1));
        candidateNeighbors.Add(new Vector2Int(center.x - 1, center.y + 1));
        candidateNeighbors.Add(new Vector2Int(center.x - 1, center.y + 0));
        candidateNeighbors.Add(new Vector2Int(center.x + 1, center.y + 0));
        candidateNeighbors.Add(new Vector2Int(center.x + 0, center.y - 1));
        candidateNeighbors.Add(new Vector2Int(center.x + 0, center.y + 1));

        List<Vector2Int> adjacentTiles = new List<Vector2Int>();
        foreach (Vector2Int v in candidateNeighbors)
        {
            if (SoilManager.Instance.CropCoordinateIsValid(v))
            {
                adjacentTiles.Add(v);
            }
        }

        return adjacentTiles;
    }

    public void CalculateAdjacenyBonus()
    {
        List<Vector2Int> boostingNeighborsList = AdjacentTilesForBoostedGrowth();
        List<Vector2Int> hinderingNeighborsList = AdjacentTilesForHinderedGrowth();

        BoostingNeighbors = 0;
        HinderingNeighbors = 0;

        Debug.Log("boostingNeighborsList = " + boostingNeighborsList);
        Debug.Log("hinderingNeighborsList = " + hinderingNeighborsList);

        foreach (var coord in boostingNeighborsList)
        {
            Crop neighboringCrop = SoilManager.Instance.GetCrop(coord);
            if(neighboringCrop != null)
            {
                Debug.Log("Checking = " + neighboringCrop + ", " + neighboringCrop.CropType.ToString());
                foreach(var v in BoostedGrowthFromAdjacentCropTypes)
                {
                    Debug.Log("Boosted: " + v);
                }
                if (BoostedGrowthFromAdjacentCropTypes.Contains(neighboringCrop.CropType))
                {
                    Debug.Log("Boosting!");
                    BoostingNeighbors += 1;
                }
            }
        }

        foreach (var coord in hinderingNeighborsList)
        {
            Crop neighboringCrop = SoilManager.Instance.GetCrop(coord);
            if (neighboringCrop != null)
            {
                Debug.Log("Checking = " + neighboringCrop);
                if (HinderedGrowthFromAdjacentCropTypes.Contains(neighboringCrop.CropType))
                {
                    Debug.Log("Hindering!");
                    HinderingNeighbors += 1;
                }
            }
        }

        int growthModifier = BoostingNeighbors - HinderingNeighbors;
        int growthModifierAbs = Mathf.Min(Mathf.Abs(growthModifier), BlipSprites.Count);

        if (growthModifierAbs == 0)
        {
            BlipRenderer.enabled = false;
        }
        else
        {
            BlipRenderer.enabled = true;
            BlipRenderer.sprite = BlipSprites[growthModifierAbs - 1];
            BlipRenderer.color = growthModifier < 0 ? Color.red : Color.green;
        }
    }
}
