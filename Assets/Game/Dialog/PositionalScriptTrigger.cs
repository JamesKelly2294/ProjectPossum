using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionalScriptTrigger : MonoBehaviour
{
    public FinalizedDialogScript TargetScript;
    public bool TriggerOnce;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterDialog.Instance.BeginScript(TargetScript);
        if (TriggerOnce)
        {
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
