using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Represents the data structure for saving game progress.
/// </summary>
public class SaveData
{
    /// <summary>
    /// List of IDs of items collected by the player.
    /// </summary>
    public List<string> collectedItems;

    /// <summary>
    /// Dictionary containing active items in the game world.
    /// </summary>
    public SerializableDictionary<string, ItemPickUpSaveData> activeItems;

    /// <summary>
    /// Dictionary containing saved inventories for chests.
    /// </summary>
    public SerializableDictionary<string, InventorySaveData> chestDictionary;

    /// <summary>
    /// Dictionary containing saved data for shopkeepers.
    /// </summary>
    public SerializableDictionary<string, ShopSaveData> _shopKeeperDictionary;

    /// <summary>
    /// Saved data for the player's inventory.
    /// </summary>
    public InventorySaveData playerInventory;

    /// <summary>
    /// Constructor to initialize the save data.
    /// </summary>
    public SaveData()
    {
        collectedItems = new List<string>();
        activeItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        chestDictionary = new SerializableDictionary<string, InventorySaveData>();
        playerInventory = new InventorySaveData();
        _shopKeeperDictionary = new SerializableDictionary<string, ShopSaveData>();
    }
}