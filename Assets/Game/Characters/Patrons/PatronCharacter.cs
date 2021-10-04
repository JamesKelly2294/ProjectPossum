using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PatronMovement))]
public class PatronCharacter : Character
{
    [Header("Patron State")]
    public PatronState CurrentState;
    public PatronEntryDirection EntryDirection;

    [Header("External References")]
    public PatronMovement PatronMovement;
    public GameObject TimesRunningOutIndicator;

    public enum PatronState
    {
        Idle,
        Entering,
        Exiting,
        Queueing,
        Queued
    }

    public enum PatronEntryDirection
    {
        West, East
    }

    // Start is called before the first frame update
    void Start()
    {
        Transform spriteTransform = transform.Find("Sprite");
        ReskinAnimation hair = spriteTransform.Find("Hair").GetComponent<ReskinAnimation>();
        ReskinAnimation eyes = spriteTransform.Find("Eyes").GetComponent<ReskinAnimation>();
        ReskinAnimation shirt = spriteTransform.Find("Shirt").GetComponent<ReskinAnimation>();
        ReskinAnimation pants = spriteTransform.Find("Pants").GetComponent<ReskinAnimation>();
        ReskinAnimation blush = spriteTransform.Find("Blush").GetComponent<ReskinAnimation>();
        ReskinAnimation lipstick = spriteTransform.Find("Lipstick").GetComponent<ReskinAnimation>();
        ReskinAnimation body = spriteTransform.Find("Body").GetComponent<ReskinAnimation>();

        bool isMasculinePresenting = Random.Range(0, 2) == 0;

        string[] hairArray = isMasculinePresenting ? MasculinePresentingHair : FemininePresentingHair;
        hair.spriteSheetName = "Spritesheets/Hair/" + hairArray[Random.Range(0, hairArray.Length)] 
                                                    + "_" + HairColors[Random.Range(0, HairColors.Length)];

        eyes.spriteSheetName = "Spritesheets/Eyes/Eyes/" + Eyes[Random.Range(0, Eyes.Length)];

        string[] shirtArray = isMasculinePresenting ? MasculinePresentingShirts : FemininePresentingShirts;
        shirt.spriteSheetName = "Spritesheets/Clothing/Shirts/" + shirtArray[Random.Range(0, shirtArray.Length)];
        shirt.GetComponent<SpriteRenderer>().color = ShirtColors[Random.Range(0, ShirtColors.Length)];


        string[] pantsArray = isMasculinePresenting ? MasculinePresentingPants : FemininePresentingPants;
        pants.spriteSheetName = "Spritesheets/Clothing/Pants/" + pantsArray[Random.Range(0, pantsArray.Length)];
        pants.GetComponent<SpriteRenderer>().color = PantsColors[Random.Range(0, PantsColors.Length)];

        int bodyType = Random.Range(0, 5);
        body.spriteSheetName = "Spritesheets/Characters/" + Characters[bodyType];
        lipstick.spriteSheetName = "Spritesheets/Eyes/Lipstick/" + Lipstick[bodyType];
        blush.spriteSheetName = "Spritesheets/Eyes/Blush/" + Blush[bodyType];

        if (Random.Range(0.0f, 1.0f) > 0.30f)
        {
            lipstick.gameObject.SetActive(false);
        }
        if (Random.Range(0.0f, 1.0f) > 0.30f)
        {
            blush.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReachedEndOfCurrentPath()
    {
        if (CurrentState == PatronState.Entering)
        {
            TryQueuingInApothecaryQueue();
        }
        else if (CurrentState == PatronState.Queueing)
        {
            PatronMovement.FaceDirection(Direction.North);
            CurrentState = PatronState.Queued;
        }
        else if (CurrentState == PatronState.Exiting)
        {
            Destroy(this.gameObject);
        }
    }

    public void BeginPathingToApothecary()
    {
        if (Random.Range(0, 2) == 0)
        {
            EntryDirection = PatronEntryDirection.West;
        }
        else
        {
            EntryDirection = PatronEntryDirection.East;
        }

        GameObject pathStart = EntryDirection == PatronEntryDirection.West ?
            PatronManager.Instance.WestPathStart : PatronManager.Instance.EastPathStart;

        PatronMovement.path = pathStart.GetAllChildren();
        PatronMovement.followingPath = true;

        CurrentState = PatronState.Entering;

        transform.position = PatronMovement.path[0].position;
    }

    public void TryQueuingInApothecaryQueue()
    {
        int nextAvailableSpot = PatronManager.Instance.NextAvailableApothecarySpotForPatron();

        List<Transform> path = new List<Transform>();

        if (nextAvailableSpot == -1)
        {
            CurrentState = PatronState.Exiting;
            GameObject pathEnd = EntryDirection == PatronEntryDirection.West ?
                PatronManager.Instance.WestPathEnd : PatronManager.Instance.EastPathEnd;
            path.AddRange(pathEnd.GetAllChildren().Skip(1));

            // Todo: 50% chance of looking at shop and seeing the queue is full,
            // 50 % chance of just walking along the road
        }
        else
        {
            CurrentState = PatronState.Queueing;
            path.Add(PatronManager.Instance.ApothecaryTurnaround.GetAllChildren()[0]);
            path.Add(PatronManager.Instance.ApothecaryQueue.GetAllChildren()[nextAvailableSpot]);
            PatronManager.Instance.ReserveApothecarySpotForPatron(nextAvailableSpot, this);
        }


        PatronMovement.pathIndex = 0;
        PatronMovement.path = path;
        PatronMovement.followingPath = true;
    }

    private static readonly string[] Characters = new string[]
    {
        "char1_animation",
        "char2_animation",
        "char3_animation",
        "char4_animation",
        "char5_animation",
    };

    private static readonly string[] Blush = new string[]
    {
        "blush1",
        "blush2",
        "blush3",
        "blush4",
        "blush5",
}   ;

    private static readonly string[] Lipstick = new string[]
    {
        "lipstick1",
        "lipstick2",
        "lipstick3",
        "lipstick4",
        "lipstick5",
    };
    private static readonly string[] Eyes = new string[]
    {
        "eyes_black",
        "eyes_blue_dark",
        "eyes_blue_light",
        "eyes_brown",
        "eyes_brown_dark",
        "eyes_brown_light",
        "eyes_green",
        "eyes_green_dark",
        "eyes_green_light",
        "eyes_grey",
        "eyes_grey_light",
        "eyes_pink",
        "eyes_pink_dark",
        "eyes_pink_light",
        "eyes_red",
        "eyes_red_dark",
    };

    private static readonly Color[] ShirtColors = new Color[]
    {
        Color.red,
        Color.green,
        Color.white,
        Color.blue,
        Color.cyan,
        Color.yellow,
    };

    private static readonly Color[] PantsColors = new Color[]
    {
        new Color(0.754717f, 0.3741884f, 0.05339978f),
        new Color(0.514151f, 0.5860363f, 1.0f),
        new Color(1.0f, 0.8340892f, 0.656f),
    };

    private static readonly string[] HairColors = new string[]
    {
        "black",
        "blonde",
        "brown",
        "brown_light",
        "copper",
        "grey",
    }; 

    private static readonly string[] MasculinePresentingHair = new string[]
    {
         "buzzcut",
         "curly",
         "emo",
         "frenchcurl",
         "gentleman",
         "midiwave",
         "wavy",
    };

    private static readonly string[] FemininePresentingHair = new string[]
    {
         "braids",
         "curly",
         "emo",
         "extralong",
         "frenchcurl",
         "midiwave",
         "spacebuns",
         "wavy",
    };

    private static readonly string[] MasculinePresentingPants = new string[]
    {
        "grey_pants",
    };

    private static readonly string[] FemininePresentingPants = new string[]
    {
        "grey_pants",
        "grey_skirt",
    };

    private static readonly string[] MasculinePresentingShirts = new string[]
    {
        "grey_basic",
        "grey_overalls",
        "grey_sporty",
        "grey_suit",
    };

    private static readonly string[] FemininePresentingShirts = new string[]
    {
        "grey_basic",
        "grey_dress",
        "grey_floral",
    };

}
