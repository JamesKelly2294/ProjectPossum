using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDialog : MonoBehaviour
{
    public static CharacterDialog Instance
    {
        get; private set;
    }

    public bool DebugHideUI = false;

    public FinalizedDialogScript ActiveScript;
    public int ActiveScriptIndex = 0;

    public bool BeginScript(FinalizedDialogScript script)
    {
        if (ActiveScript)
        {
            return false;
        }

        ActiveScript = script;
        ActiveScriptIndex = 0;
        CameraDialogFrame.SetActive(true);

        ActiveScript.IsBeingPlayed = true;

        if (ActiveScript.OnScriptBegan != null)
        {
            ActiveScript.OnScriptBegan.Invoke();
        }

        DisplayNextScriptLine();
        return true;
    }

    public void DisplayNextScriptLine ()
    {
        if (ActiveScriptIndex >= ActiveScript.dialogScript.script.Count)
        {
            FinishScript();
            return;
        }

        int speakerIndex = ActiveScript.dialogScript.script[ActiveScriptIndex].speaker;
        SpeakerName.text = ActiveScript.speakers[speakerIndex].FirstName;
        SpeakerDialog.text = ActiveScript.dialogScript.script[ActiveScriptIndex].dialog;
        CameraTarget = ActiveScript.speakers[speakerIndex].gameObject;

        DialogEvent dialogEvent = null;
        foreach (var e in ActiveScript.dialogScript.events)
        {
            if (e.scriptIndex == ActiveScriptIndex)
            {
                dialogEvent = e;
                break;
            }
        }
        
        if (dialogEvent != null)
        {
            DialogUnityEvent dialogUnityEvent = null;
            foreach (var e in ActiveScript.DialogEvents)
            {
                if (e.dialogEventName == dialogEvent.eventName)
                {
                    dialogUnityEvent = e;
                    break;
                }
            }

            if (dialogUnityEvent != null && dialogUnityEvent.unityEvent != null)
            {
                dialogUnityEvent.unityEvent.Invoke();
            }
        }

        ActiveScriptIndex += 1;

        if (DebugHideUI)
        {
            DisplayNextScriptLine();
        } else
        {
            AudioManager.Instance.Play("Dialog/Enter", false, 0.8f, 1.4f, 1.2f, 1.1f);
        }
    }

    public void FinishScript()
    {
        ActiveScript.IsBeingPlayed = false;
        ActiveScript.CompletedOnce = true;
        if (ActiveScript.OnScriptFinished != null)
        {
            ActiveScript.OnScriptFinished.Invoke();
        }

        ActiveScript = null;
        ActiveScriptIndex = 0;
        CameraDialogFrame.SetActive(false);
    }

    public Camera SpeakerCam;
    public TextMeshProUGUI SpeakerName;
    public TextMeshProUGUI SpeakerDialog;

    public GameObject CameraDialogFrame;
    public GameObject CameraTarget;

    public RectTransform CharacterDialogArrow;

    public float CharacterDialogArrowSpeed = 1.0f;
    public float CharacterDialogArrowDistance = -7.5f;
    private float _characterDialogArrowStartingY;

    private void Awake()
    {
        Instance = this;
        _characterDialogArrowStartingY = CharacterDialogArrow.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (ActiveScript != null)
        {
            SpeakerCam.transform.position = new Vector3(CameraTarget.transform.position.x,
                CameraTarget.transform.position.y + 0.5f, SpeakerCam.transform.position.z);

            CharacterDialogArrow.position = new Vector2(CharacterDialogArrow.position.x,
                _characterDialogArrowStartingY + (Mathf.Sin(Time.time * CharacterDialogArrowSpeed) * CharacterDialogArrowDistance) + CharacterDialogArrowDistance / 2.0f);

            if (InputUtil.GetPrimaryActionDown())
            {
                DisplayNextScriptLine();
            }
        }
    }
}

class InputUtil
{
    public static bool GetPrimaryActionDown()
    {
        return Input.GetKeyDown(KeyCode.Return)
                || Input.GetKeyDown(KeyCode.E)
                || Input.GetKeyDown(KeyCode.F);
    }
}