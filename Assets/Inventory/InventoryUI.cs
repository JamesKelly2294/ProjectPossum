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

    public PubSubSender EventSender;

    public Color UnselectedItemColor;
    public Color SelectedItemColor;

    public PlayerInventory Inventory;

    private bool isHidden;

    public static string ItemUsedKey = "inventory.used";
    public static string ItemSelectionChangedKey = "inventory.selection-changed";

    private IEnumerable<InventoryItemUI> ItemViews;

    // Start is called before the first frame update
    public void Start()
    {
        ItemViews = new List<InventoryItemUI>();
        HideInventory();
        UseButton.interactable = false;

        UseButton.onClick.AddListener(() => {
            EventSender.Publish(ItemUsedKey, Selected);
        });
    }

    // Update is called once per frame
    public void Update()
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
        // no-op
    }

    public void HandleItemSelected(PubSubListenerEvent e)
    {
        var candidate = (InventoryItem)e.value;
        if (!candidate || !candidate.Item.IsSelectable()) { return; }
        foreach (InventoryItemUI subview in ItemViews)
        {
            if(subview.InventoryItem == candidate)
            {
                Selected = subview;
                subview.ShowSelected();
            }
            else
            {
                subview.ShowUnselected();
            }
        }

        if(candidate.Item.IsUsable())
        {
            UseButton.interactable = true;
        }
    }

    public void HandleItemDeselected(PubSubListenerEvent e)
    {
        var candidate = (InventoryItem)e.value;
        if(!candidate || !candidate.Item.IsSelectable()) { return; }
        if(candidate == Selected.InventoryItem)
        {
            Selected.ShowUnselected();
            Selected = null;
        }
        UseButton.interactable = false;
    }

    public void HandleItemSelectionToggled(PubSubListenerEvent e)
    {
        var candidate = (InventoryItem)e.value;
        if(!candidate || !candidate.Item.IsSelectable()) { return; }
        if(Selected && candidate == Selected.InventoryItem)
        {
            HandleItemDeselected(e);
        }
        else
        {
            HandleItemSelected(e);
        }
    }

    public void HandleInventoryItemClicked(PubSubListenerEvent e)
    {
        var candidate = (InventoryItem)e.value;
        if (candidate && candidate.Item.IsSelectable())
        {
            EventSender.Publish(ItemSelectionChangedKey, candidate);
        }
    }

    public void HandleItemUsed(PubSubListenerEvent e)
    {
        var itemUsed = (InventoryItem)e.value;
        if(!itemUsed) { return; }

        if (itemUsed.Item.IsUsable())
        {
            itemUsed.Item.UseItem();
        }

        if (itemUsed.Item.IsItemConsumable())
        {
            Inventory.UpdateItemQuantity(itemUsed.Item, -1);
        }

        RefreshUI();
    }

    public void HandleItemAdded(PubSubListenerEvent e)
    {
        var inventoryItem = (InventoryItem)e.value;

        var inventoryItemUIPrefab = Instantiate(InventoryItemUIPrefab, Grid.transform);
        inventoryItemUIPrefab.SetActive(false);
        inventoryItemUIPrefab.name = $"Inventory UI Prefab {inventoryItem.Item.ItemName}";

        var inventoryItemUI = inventoryItemUIPrefab.GetComponentInChildren<InventoryItemUI>();
        inventoryItemUI.InventoryItem = inventoryItem;
        inventoryItemUI.SelectedItemColor = SelectedItemColor;
        inventoryItemUI.NormalItemColor = UnselectedItemColor;

        var concatList = new List<InventoryItemUI>();
        concatList.Add(inventoryItemUI);
        ItemViews = ItemViews.Concat(concatList);

        inventoryItemUIPrefab.SetActive(true);
        //RefreshUI();
    }

    public void HandleItemRemoved(PubSubListenerEvent e)
    {
        var inventoryItem = (InventoryItem)e.value;
        var target = ItemViews.Where(subview => subview.InventoryItem == inventoryItem).FirstOrDefault();
        if(target)
        {
            if (Selected == target) { Selected = null; }
            ItemViews = ItemViews.Where(subview => subview != target);

            Debug.Log($"Deleting {target.name}");
            Destroy(target.gameObject);
            RefreshUI();
        }
    }

    public void HandleItemChanged(PubSubListenerEvent e)
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach(var subview in ItemViews) {
            if(!subview.InventoryItem)
            {
                Destroy(subview);
            }
            else
            {
                subview.RefreshUI();
            }
        }
    }

    /**
     * This is here to support debug/editor based addition of stuff, fire an event or call AddItemToPlayerInventory() instead.
     */
    public void DebugSpawnItemFromLibrary()
    {
#if UNITY_EDITOR
        var library = GameObject.Find("ItemLibrary");
        if (!library)
        {
            Debug.Log("Unable to locate ItemLibrary, double check that the Prefab is present?");
            return;
        }

        int itemIndex = this.ItemViews.Count() % Inventory.UsableInventorySlots;
        var item = library.GetComponents<BaseItem>()[itemIndex];
        if (!item)
        {
            Debug.Log("Something went wrong while looking for items from the library");
            return;
        }

        Inventory.AddItemToPlayerInventory(item);
#endif
    }

    public void DebugRemoveSelectedItem()
    {
#if UNITY_EDITOR
        if(!Selected) {
            Debug.Log("Nothing selected to remove!");
            return;
        }
        Inventory.RemoveItemFromPlayerInventory(Selected.InventoryItem.Item);
#endif
    }
}