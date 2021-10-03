using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryItem
{
    public GameObject Item { get; set; }
    public int Quantity { get; set; }
}

/**
 * Inventory features
 * - Item selection/deselection (on click in UI)
 * - Item highlighting (Prompted when inventory is open and the user is near a table/bench etc, or if some other UI element needs to hint to the player that they can make stuff)
 * - Toggling Inventory visibility
 * 
 */
public class PlayerInventory : MonoBehaviour
{
    public IEnumerable<IInventoryItem> Items;
    private IEnumerable<int> selectedItemIndices;
    
    public int UsableInventorySlots = 40;
    public int InventorySlotsVerticalCount = 8;
    public int InventorySlotsHorizontalCount = 5;

    public InventoryUI View;

    public void Start()
    {
        if (Items == null) { Items = new List<IInventoryItem>(); }
        if (selectedItemIndices == null) { selectedItemIndices = new List<int>(); } 
    }

    public void Update()
    {
        
    }

    public void SelectItems(IEnumerable<Vector2> atInventoryIndex)
    {
        throw new NotImplementedException();
    }

    public void DeselectItems(IEnumerable<Vector2> atInventoryIndex)
    {
        throw new NotImplementedException();
    }

    public void HighlightItem(Vector2 atInventoryIndex, Color suggestedHighlightColor)
    {
        throw new NotImplementedException();
    }

    public void UnhighlightItem(Vector2 atInventoryIndex)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<Vector2> FindItemIndices(IInventoryItem item)
    {
        throw new NotImplementedException();
    }
}
