using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class InventoryUI : MonoBehaviour
{
    public GameObject Grid;
    public GameObject InventoryItemUIPrefab;
    public InventoryItemUI Selected;

    public Button UseButton;
    public Button DropButton;

    public PubSubSender EventSender;

    public Color UnselectedItemColor;
    public Color SelectedItemColor;

    public PlayerInventory Inventory;

    private bool isHidden;

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
        var inventoryItem = (InventoryItem) e.value;
        var matches = (from view in ItemViews
                       where view.InventoryItem != null && view.InventoryItem.Item.name == inventoryItem.Item.name
                       select view).Count();
        if (matches > 0) {
            Debug.Log($"{inventoryItem.name} UI item already exists, incrementing instead");
            inventoryItem.Quantity += 1;
            RefreshUI();
            return;
        }

        var inventoryItemUIPrefab = Instantiate(InventoryItemUIPrefab, Grid.transform);
        inventoryItemUIPrefab.SetActive(false);
        inventoryItemUIPrefab.name = "Inventory UI Prefab (" + inventoryItem.name + ")";

        var inventoryItemUI = inventoryItemUIPrefab.GetComponentInChildren<InventoryItemUI>();
        inventoryItemUI.InventoryItem = inventoryItem;
        inventoryItemUI.SelectedItemColor = SelectedItemColor;
        inventoryItemUI.NormalItemColor = UnselectedItemColor;

        inventoryItemUIPrefab.SetActive(true);
    }

    public void HandleItemRemoved(PubSubListenerEvent e)
    {
        var item = (InventoryItem)e.value;

        item.Quantity -= 1;
        if(item.Quantity > 0) { return; }

        var matches = (from view in ItemViews
                       where view.InventoryItem.name == item.name
                       select view);
        var target = matches.FirstOrDefault();
        if(target)
        {
            Destroy(target);
        }
    }

    
    public void SpawnInventoryItem()
    {
        var assetId = AssetDatabase.FindAssets("crops_275").FirstOrDefault();
        if (assetId == null || assetId == String.Empty) { return; }

        var path = AssetDatabase.GUIDToAssetPath(assetId);
        if (path == null || path == String.Empty) { return; }

        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null) { return; }

        Debug.Log("Loaded sprite at " + path);

        var itemGO = GameObject.Find("Debug Item");
        if (!itemGO) { itemGO = new GameObject("Debug Item"); }
        var item = itemGO.GetComponent<Seed>();
        if (!item) { item = itemGO.AddComponent<Seed>(); }
        item.InventoryIcon = sprite;
        itemGO.transform.parent = Inventory.transform;

        var inventoryItemGO = GameObject.Find("Debug Inventory Item");
        if (inventoryItemGO == null) { inventoryItemGO = new GameObject("Debug Inventory Item"); }

        var inventoryItem = inventoryItemGO.GetComponent<InventoryItem>();
        if (inventoryItem == null) {
            inventoryItem = inventoryItemGO.AddComponent<InventoryItem>();
            inventoryItem.Quantity = 1;
        }
        inventoryItem.Item = item;
        if (!inventoryItem.Sender) { inventoryItem.Sender = inventoryItemGO.AddComponent<PubSubSender>(); }
        inventoryItemGO.transform.parent = Inventory.transform;

        var e = new PubSubListenerEvent("inventory.debug", gameObject, inventoryItem);
        HandleItemAdded(e);
    }

    public void RefreshUI()
    {
        foreach(var subview in ItemViews) {
            subview.RefreshUI();
        }
    }
}