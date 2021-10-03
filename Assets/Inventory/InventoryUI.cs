using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public bool ObjectiveIsVisible;

    private bool isHidden;

    // Start is called before the first frame update
    void Start()
    {
        HideInventory();
        isHidden = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (ObjectiveIsVisible)
            {
                var transform = gameObject.GetComponent<RectTransform>();
                transform.offsetMax = new Vector2(transform.offsetMax.x, -120.0f);
                //transform.ForceUpdateRectTransforms();
            }

            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (isHidden)
        {
            ShowInventory();
        }
        else
        {
            HideInventory();
        }
        isHidden = !isHidden;
    }

    public void ShowInventory()
    {
        var canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if(canvasGroup)
        {
            Debug.Log("Showing the inventory");
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1.0f;
        }
    }

    public void HideInventory()
    {
        var canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup)
        {
            Debug.Log("Hiding the inventory");
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0.0f;
        }
    }
}