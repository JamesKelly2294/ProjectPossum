using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public Sprite InventoryIcon;

    public virtual bool IsUsable() { return true; }
    public virtual void UseItem() { return; }

    public virtual bool IsSelectable() { return true; }
    public virtual void OnSelect() { return; }
    public virtual void OnDeselect() { return; }

    public virtual bool IsDroppable() { return true; }
    public virtual void OnPickUp() { return; }
    public virtual void OnDrop() { return; }
}