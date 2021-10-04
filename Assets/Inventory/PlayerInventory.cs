using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem: MonoBehaviour
{
    public PubSubSender Sender;

    public BaseItem Item { get; set; }
    public int Quantity { get; set; }

    public void HandleClick(PubSubListenerEvent e)
    {
        if (Item.IsSelectable())
        {
            var view = e.sender.GetComponent<InventoryItemUI>();
            view.ToggleSelection();
        }
    }
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
    public IEnumerable<InventoryItem> Items;
    
    public int UsableInventorySlots = 8;

    public PubSubListener listener;

    public void Start()
    {
        if (Items == null) { Items = new List<InventoryItem>(); }
    }

    public void Update()
    {
        
    }

    public void HandleItemUsed(string key, GameObject sender, object value)
    {
        var senderView = sender.GetComponent<InventoryUI>();
        var itemUsed = (InventoryItem)value;

        if(itemUsed.Item.IsUsable())
        {
            itemUsed.Quantity -= 1;
            itemUsed.Item.UseItem();
        }
    }
}
