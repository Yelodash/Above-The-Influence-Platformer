using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a slot in an inventory that can hold an item.
/// </summary>
[System.Serializable]
public class InventorySlot : ItemSlot
{
    /// <summary>
    /// Constructor to make an inventory slot with the specified item data and amount.
    /// </summary>
    /// <param name="source">The item data of the source.</param>
    /// <param name="amount">The amount of the item in the slot.</param>
    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        _itemID = itemData.ID;
        stackSize = amount;
    }

    /// <summary>
    ///  make an empty inventory slot.
    /// </summary>
    public InventorySlot()
    {
        ClearSlot();
    }

    /// <summary>
    /// Updates slot directly with new data and amount.
    /// </summary>
    /// <param name="data">The new item data.</param>
    /// <param name="amount">The new amount of the item.</param>
    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        itemData = data;
        _itemID = itemData.ID;
        stackSize = amount;
    }

    /// <summary>
    /// Checks if there is enough room left in the stack for the specified amount to add.
    /// </summary>
    /// <param name="amountToAdd">The amount to add to the stack.</param>
    /// <param name="amountRemaining">Outputs the amount remaining space in the stack.</param>
    /// <returns>True if there is enough room left, otherwise false.</returns>
    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - stackSize;
        return EnoughRoomLeftInStack(amountToAdd);
    }

    /// <summary>
    /// Checks if there is enough room left in the stack for the specified amount to add.
    /// </summary>
    /// <param name="amountToAdd">The amount to add to the stack.</param>
    /// <returns>True if there is enough room left, otherwise false.</returns>
    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        return itemData == null || stackSize + amountToAdd <= itemData.MaxStackSize;
    }

    /// <summary>
    /// Splits the stack into two equal parts.
    /// </summary>
    /// <param name="splitStack">Outputs the new inventory slot with half of the stack.</param>
    /// <returns>True if the split was successful, otherwise false.</returns>
    public bool SplitStack(out InventorySlot splitStack)
    {
        if (stackSize <= 1)
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(stackSize / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(itemData, halfStack);
        return true;
    }
}