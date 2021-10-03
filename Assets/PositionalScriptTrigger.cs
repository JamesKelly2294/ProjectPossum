using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalScriptTrigger : MonoBehaviour
{
    public FinalizedDialogScript TargetScript;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterDialog.Instance.BeginScript(TargetScript);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
