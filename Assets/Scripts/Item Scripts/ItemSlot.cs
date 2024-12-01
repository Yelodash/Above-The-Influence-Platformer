using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a slot that can hold an item in an inventory system.
/// </summary>
public abstract class ItemSlot : ISerializationCallbackReceiver
{
    [NonSerialized] protected InventoryItemData itemData; // Reference to the data
    [SerializeField] protected int _itemID = -1;
    [SerializeField] protected int stackSize; // Current stack size - how many of the data do we have?

    /// <summary>
    /// The item data held by the slot.
    /// </summary>
    public InventoryItemData ItemData => itemData;

    /// <summary>
    /// The stack size of the item held by the slot.
    /// </summary>
    public int StackSize => stackSize;
    
    /// <summary>
    /// Clears the slot.
    /// </summary>
    public void ClearSlot()
    {
        itemData = null;
        _itemID = -1;
        stackSize = -1;
    }
    
    /// <summary>
    /// Assigns an item to the slot.
    /// </summary>
    /// <param name="invSlot">The inventory slot containing the item to assign.</param>
    public void AssignItem(InventorySlot invSlot)
    {
        if (itemData == invSlot.ItemData) AddToStack(invSlot.StackSize); // Does the slot contain the same item? Add to stack if so.
        else // Overwrite slot with the inventory slot that we're passing in.
        {
            itemData = invSlot.ItemData;
            _itemID = itemData.ID;
            stackSize = 0;
            AddToStack(invSlot.StackSize);
        }
    }

    /// <summary>
    /// Assigns an item to the slot.
    /// </summary>
    /// <param name="data">The item data to assign.</param>
    /// <param name="amount">The amount of the item.</param>
    public void AssignItem(InventoryItemData data, int amount)
    {
        if (itemData == data) AddToStack(amount);
        else
        {
            itemData = data;
            _itemID = data.ID;
            stackSize = 0;
            AddToStack(amount);
        }
    }
    
    /// <summary>
    /// Adds an amount to the stack size.
    /// </summary>
    /// <param name="amount">The amount to add.</param>
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    /// <summary>
    /// Removes an amount from the stack size.
    /// </summary>
    /// <param name="amount">The amount to remove.</param>
    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
        if (stackSize <= 0) ClearSlot();
    }
    
    public void OnBeforeSerialize()
    {
        
    }

    /// <summary>
    /// Deserialize the item data after deserialization.
    /// </summary>
    public void OnAfterDeserialize()
    {
        if (_itemID == -1) return;

        var db = Resources.Load<Database>("Database");
        itemData = db.GetItem(_itemID);
    }
}