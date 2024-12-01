using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// Class that represents the inventory system of the player.
/// </summary>
public class PlayerInventoryHolder : InventoryHolder
{

    /// <summary>
    /// Event invoked when the player inventory has changed.
    /// </summary>
    public static UnityAction OnPlayerInventoryChanged;

    /// <summary>
    /// Initializes the player inventory.
    /// </summary>
    private void Start()
    {
        SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
    }

    /// <summary>
    /// Loads inventory data from saved game data.
    /// </summary>
    /// <param name="data"></param>
    protected override void LoadInventory(SaveData data)
    {
        // Check the save data for this specific chests inventory, and if it exists, load it in.
        if (data.playerInventory.InvSystem != null)
        {
            this.primaryInventorySystem = data.playerInventory.InvSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }
    

    /// <summary>
    /// Updates the player inventory.
    /// </summary>
    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame) OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
    }

    /// <summary>
    /// Adds an item to the player inventory.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
    }
}
