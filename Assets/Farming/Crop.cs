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

    [Header("Managed Stats")]
    public float CurrentGrowth = 0.0f;
    public CropState State = CropState.Seedling;
    public Vector2Int Coordinate;

    [Header("Visuals")]
    public Sprite SeedlingSprite;
    public List<Sprite> GrowthSprites;
    public Sprite GrownSprite;
    public Sprite InventoryIcon;
    public SpriteRenderer SpriteRenderer;


    public ICropDelegate CropDelegate;

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
}
