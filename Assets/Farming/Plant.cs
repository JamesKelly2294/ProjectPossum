using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public Sprite seedlingSprite;
    public Sprite growingSprite;
    public Sprite fullyGrownSprite;
    public Sprite ruinedSprite;
    public Sprite inventoryIcon;

    public double seedPrice = 10.0;

    private double growthProgress = 0.0;
    public double growthPerSecond = 1.0;
    public double totalGrowth = 30.0;

    enum State {
        Seedling,
        Growing,
        FullyGrown,
        Dying,
        Ruined
    }
}
