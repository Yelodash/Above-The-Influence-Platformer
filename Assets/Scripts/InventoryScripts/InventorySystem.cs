using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

/// <summary>
/// Class that represents the inventory system of the player.
/// </summary>
[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;
    [SerializeField] private int _gold;

    /// <summary>
    ///  The amount of gold the player has.
    /// </summary>
    public int Gold => _gold;
    
    /// <summary>
    ///  The list of inventory slots.
    /// </summary>
    public List<InventorySlot> InventorySlots => inventorySlots;
    
    /// <summary>
    ///  The size of the inventory.
    /// </summary>
    public int InventorySize => InventorySlots.Count;
    
    /// <summary>
    ///  Event invoked when an inventory slot has changed.
    /// </summary>

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    /// <summary>
    ///  Constructor that sets the amount of slots.
    /// </summary>
    /// <param name="size"></param>
    public InventorySystem(int size) // Constructor that sets the amount of slots.
    {
        _gold = 1000;
        CreateInventory(size);
    }

    /// <summary>
    ///  Constructor that sets the amount of slots and the amount of gold.
    /// </summary>
    /// <param name="size"></param>
    /// <param name="gold"></param>
    public InventorySystem(int size, int gold)
    {
        _gold = gold;
        CreateInventory(size);
    }

    /// <summary>
    ///  Creates the inventory.
    /// </summary>
    /// <param name="size"></param>
    private void CreateInventory(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    /// <summary>
    ///  Adds an item to the inventory.
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Check whether item exists in inventory.
        {
            foreach (var slot in invSlot)
            {
                if(slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
            
        }
        
        if (HasFreeSlot(out InventorySlot freeSlot)) // Gets the first available slot
        {
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
            // Add implementation to only take what can fill the stack, and check for another free slot to put the remainder in.
        }

        return false;
    }

    /// <summary>
    ///  Checks whether the inventory contains the item.
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="invSlot"></param>
    /// <returns></returns>
    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot) // Do any of our slots have the item to add in them?
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList(); // If they do, the get a list of all of them.
        return invSlot == null ? false : true; // If they do return true, if not return false.
    }

    /// <summary>
    ///  Checks whether the inventory has a free slot.
    /// </summary>
    /// <param name="freeSlot"></param>
    /// <returns></returns>
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null); // Get the first free slot
        return freeSlot == null ? false : true;
    }

    /// <summary>
    ///  Adds an item to the inventory.
    /// </summary>
    /// <param name="shoppingCart"></param>
    /// <returns></returns>
    public bool CheckInventoryRemaining(Dictionary<InventoryItemData, int> shoppingCart)
    {
        var clonedSystem = new InventorySystem(this.InventorySize);

        for (int i = 0; i < InventorySize; i++)
        {
            clonedSystem.InventorySlots[i].AssignItem(this.InventorySlots[i].ItemData,
                this.InventorySlots[i].StackSize);
        }

        foreach (var kvp in shoppingCart)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                if (!clonedSystem.AddToInventory(kvp.Key, 1)) return false;
            }
        }

        return true;
    }

    /// <summary>
    ///  Removes an item from the inventory.
    /// </summary>
    /// <param name="basketTotal"></param>
    public void SpendGold(int basketTotal)
    {
        _gold -= basketTotal;
    }

    /// <summary>
    ///  Gets all the items held in the inventory.
    /// </summary>
    /// <returns></returns>
    public Dictionary<InventoryItemData, int> GetAllItemsHeld()
    {
        var distinctItems = new Dictionary<InventoryItemData, int>();

        foreach (var item in inventorySlots)
        {
            if (item.ItemData == null) continue;

            if (!distinctItems.ContainsKey(item.ItemData)) distinctItems.Add(item.ItemData, item.StackSize);
            else distinctItems[item.ItemData] += item.StackSize;
        }

        return distinctItems;
    }
    /// <summary>
    ///  Adds gold to the inventory.
    /// </summary>
    /// <param name="price"></param>

    public void GainGold(int price)
    {
        _gold += price;
    }

    /// <summary>
    ///  Removes items from the inventory.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="amount"></param>
    public void RemoveItemsFromInventory(InventoryItemData data, int amount)
    {
        if (ContainsItem(data, out List <InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                var stackSize = slot.StackSize;
                
                if (stackSize > amount) slot.RemoveFromStack(amount);
                else
                {
                    slot.RemoveFromStack(stackSize);
                    amount -= stackSize;
                }
                
                OnInventorySlotChanged?.Invoke(slot);
            }
        }
    }
}
