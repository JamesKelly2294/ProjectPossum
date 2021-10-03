using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClassExtension
{
    public static List<GameObject> GetAllChildrenGOs(this GameObject Go)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            list.Add(Go.transform.GetChild(i).gameObject);
        }
        return list;
    }

    public static List<Transform> GetAllChildren(this GameObject Go)
    {
        List<Transform> list = new List<Transform>();
        for (int i = 0; i < Go.transform.childCount; i++)
        {
            list.Add(Go.transform.GetChild(i));
        }
        return list;
    }
}

public class PatronManager : MonoBehaviour
{
    public static PatronManager Instance
    {
        get; private set;
    }

    [Header("Prefabs")]
    public GameObject PatronPrefab;

    [Header("Pathing")]
    public GameObject WestPathStart;
    public GameObject WestPathEnd;
    public GameObject EastPathStart;
    public GameObject EastPathEnd;
    public GameObject ApothecaryTurnaround;
    public GameObject ApothecaryQueue;

    public List<PatronCharacter> _patrons;
    public PatronCharacter[] _apothecaryQueuePatrons;

    private GameObject _patronsParent;
    
    void Start()
    {
        Instance = this;

        _patronsParent = new GameObject();
        _patronsParent.transform.parent = transform;
        _patronsParent.transform.name = "Patrons";
        _patronsParent.transform.localPosition = Vector3.zero;

        _patrons = new List<PatronCharacter>();
        _apothecaryQueuePatrons = new PatronCharacter[ApothecaryQueue.transform.childCount];
    }
    
    void Update()
    {
        
    }

    public bool ApothecarySpotOpenForPatron()
    {
        for (int i = 0; i < _apothecaryQueuePatrons.Length; i++)
        {
            if (_apothecaryQueuePatrons[i] == null)
            {
                return true;
            }
        }

        return false;
    }

    public int NextAvailableApothecarySpotForPatron()
    {
        for (int i = 0; i < _apothecaryQueuePatrons.Length; i++)
        {
            if (_apothecaryQueuePatrons[i] == null)
            {
                return i;
            }
        }

        return -1;
    }

    public void ReserveApothecarySpotForPatron(int spot, PatronCharacter patron)
    {
        if (_apothecaryQueuePatrons[spot] != null)
        {
            Debug.LogError("Error! Trying to reserve an apothecary spot that is already taken.");
        } else
        {
            _apothecaryQueuePatrons[spot] = patron;
        }
    }

    public void SpawnPatron()
    {
        GameObject patronGO = Instantiate(PatronPrefab, _patronsParent.transform);
        patronGO.transform.name = "Patron";
        PatronCharacter patron = patronGO.GetComponent<PatronCharacter>();
        patron.BeginPathingToApothecary();

        _patrons.Add(patron);
    }
}
