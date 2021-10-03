using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem: MonoBehaviour
{
    public PubSubSender sender;

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
    
    public int UsableInventorySlots = 18;

    public InventoryUI View;

    public PubSubManager eventManager;
    public PubSubListener listener;

    public void Start()
    {
        if (Items == null) { Items = new List<InventoryItem>(); }
        //if (selectedItemIndices == null) { selectedItemIndices = new List<int>(); } 
    }

    public void Update()
    {
        
    }

    public void SelectItems(IEnumerable<int> atInventoryIndex)
    {
        throw new NotImplementedException();
    }

    public void DeselectItems(IEnumerable<int> atInventoryIndex)
    {
        throw new NotImplementedException();
    }

    public void HighlightItem(int atInventoryIndex, Color suggestedHighlightColor)
    {
        throw new NotImplementedException();
    }

    public void UnhighlightItem(int atInventoryIndex)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<int> FindItemIndices(BaseItem item)
    {
        throw new NotImplementedException();
    }
}
