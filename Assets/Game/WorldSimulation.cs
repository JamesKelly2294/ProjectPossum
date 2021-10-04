using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSimulation : MonoBehaviour
{
    public static WorldSimulation Instance
    {
        get; private set;
    }

    public GameObject PausedText;
    public FinalizedDialogScript StartingScript;
    public bool SimulationPaused { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CharacterDialog.Instance.BeginScript(StartingScript);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SimulationPaused = !SimulationPaused;

            Time.timeScale = SimulationPaused ? 0.0f : 1.0f;
            PausedText.SetActive(SimulationPaused);
        }
    }
}
