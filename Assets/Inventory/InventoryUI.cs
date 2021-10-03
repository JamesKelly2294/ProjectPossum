using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject Grid;
    public GameObject InventoryItemUIPrefab;

    public Button UseButton;
    public Button DropButton;

    public PubSubSender EventSender;

    public InventoryItemUI Selected;

    private bool isHidden;

    public Color UnselectedItemColor;
    public Color SelectedItemColor;

    public static string ItemUsedKey = "inventory.used";
    public static string ItemDroppedKey = "inventory.dropped";

    public IEnumerable<InventoryItemUI> ItemViews
    {
        get
        {
            var children = Grid.GetComponentsInChildren<InventoryItemUI>();
            return children;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        HideInventory();
        isHidden = true;

        UseButton.onClick.AddListener(() => {
            EventSender.Publish(ItemUsedKey, Selected);
        });

        DropButton.onClick.AddListener(() => {
            EventSender.Publish(ItemDroppedKey, Selected);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
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
        isHidden = false;
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
        isHidden = true;
    }

    public void OnUse()
    {

    }

    public void OnDrop()
    {

    }

    public void HandleItemSelected(PubSubListenerEvent e)
    {
        
    }

    public void HandleItemDeselected(PubSubListenerEvent e)
    {

    }

    public void HandleItemDropped(PubSubListenerEvent e)
    {

    }

    public void HandleItemUsed(PubSubListenerEvent e)
    {

    }

    public void HandleItemAdded(PubSubListenerEvent e)
    {
        var item = (InventoryItem) e.value;
        var matches = (from view in ItemViews
                       where view.InventoryItem.name == item.name
                       select view).Count();
        if (matches > 0) { return; }

        var inventoryItemUIPrefab = Instantiate(InventoryItemUIPrefab, Grid.transform);
        inventoryItemUIPrefab.SetActive(false);
        inventoryItemUIPrefab.name = "Inventory UI Prefab (" + item.name + ")";

        var inventoryItemUI = inventoryItemUIPrefab.GetComponentInChildren<InventoryItemUI>();
        inventoryItemUI.InventoryItem = item;
        inventoryItemUI.SelectedItemColor = SelectedItemColor;
        inventoryItemUI.NormalItemColor = UnselectedItemColor;

        inventoryItemUIPrefab.SetActive(true);
    }

    public void HandleItemRemoved(PubSubListenerEvent e)
    {
        var item = (InventoryItem)e.value;
        var matches = (from view in ItemViews
                       where view.InventoryItem.name == item.name
                       select view);
        var target = matches.FirstOrDefault();
        if(target)
        {
            Destroy(target);
        }
    }
}