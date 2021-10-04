using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem: MonoBehaviour
{
    public BaseItem Item { get; set; }
    public int Quantity { get; set; }
}

/**
 * Inventory features
 * - Item selection/deselection (on click in UI)
 * - Item highlighting (Prompted when inventory is open and the user is near a table/bench etc, or if some other UI element needs to hint to the player that they can make stuff)
 * - Toggling Inventory visibility
 * - Use/Drop
 */
public class PlayerInventory : MonoBehaviour
{
    public IEnumerable<InventoryItem> InventoryItems;
    
    public int UsableInventorySlots = 8;

    public PubSubSender publisher;

    public static string PlayerInventoryItemChanged = "inventory-item.changed";
    public static string PlayerInventoryItemAdded = "inventory-item.added";
    public static string PlayerInventoryItemRemoved = "inventory-item.removed";
    

    public void Start()
    {
        if (InventoryItems == null) { InventoryItems = new List<InventoryItem>(); }
        if (!publisher)
        {
            publisher = gameObject.AddComponent<PubSubSender>();
        }
    }

    public void Update()
    {
        
    }

    public InventoryItem GetInventoryItemForBaseItem(BaseItem item)
    {
        var target = InventoryItems.Where(inventoryItem => inventoryItem.Item == item).FirstOrDefault();
        return target;
    }

    public void AddItemToPlayerInventory(BaseItem newItem, int quantity = 1)
    {
        var go = new GameObject();
        go.transform.parent = gameObject.transform;

        var inventoryItem = go.AddComponent<InventoryItem>();
        inventoryItem.Item = newItem;
        inventoryItem.Quantity = quantity;

        var smolList = new List<InventoryItem>();
        smolList.Add(inventoryItem);
        InventoryItems = InventoryItems.Concat(smolList);

        publisher.Publish(PlayerInventoryItemAdded, inventoryItem);
        return;
    }

    public void UpdateItemQuantity(BaseItem existingItem, int quantity)
    {
        var target = GetInventoryItemForBaseItem(existingItem);
        if (!target) {
            Debug.Log($"Can't update {existingItem.ItemName} because it isn't in the player's inventory");
            return;
        }

        target.Quantity += quantity;
        Debug.Log($"Updated ${existingItem.ItemName} quantity to ${target.Quantity}");

        if (target.Quantity <= 0)
        {
            Debug.Log("Removing due to the item quantity falling below to or below zero");
            RemoveItemFromPlayerInventory(existingItem);
        }
        else
        {
            publisher.Publish(PlayerInventoryItemChanged, target);
        }
    }

    public void RemoveItemFromPlayerInventory(BaseItem item)
    {
        var target = GetInventoryItemForBaseItem(item);
        if (!target)
        {
            Debug.Log($"Couldn't find item {item.ItemName} in inventory, skipping removal");
            return;
        }

        publisher.Publish(PlayerInventoryItemRemoved, target);
        InventoryItems = InventoryItems.Where(existing => existing.Item != item);
        Destroy(target);
        return;
    }

}
