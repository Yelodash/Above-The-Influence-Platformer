using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract class which is representing an inventory holder. 
/// </summary>
[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize; // The size of the inventory
    [SerializeField] protected InventorySystem primaryInventorySystem; // The primary inventory system
    [SerializeField] protected int offset = 10; // Offset for dynamic inventory display
    [SerializeField] protected int _gold; // Amount of gold in the inventory

    /// <summary>
    /// Gets the offset for dynamic inventory display.
    /// </summary>
    public int Offset => offset;

    /// <summary>
    /// Gets the primary inventory system.
    /// </summary>
    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    /// <summary>
    /// Event invoked when a dynamic inventory display is requested.
    /// </summary>
    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested; // Inv System to Display, amount to offset display by

    /// <summary>
    /// Initializes the InventoryHolder.
    /// </summary>
    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory;
        
        primaryInventorySystem = new InventorySystem(inventorySize, _gold);
    }

    /// <summary>
    /// Loads inventory data from saved game data.
    /// </summary>
    /// <param name="saveData">The saved game data.</param>
    protected abstract void LoadInventory(SaveData saveData);
}

/// <summary>
/// Represents the data structure for saving inventory.
/// </summary>
[System.Serializable]
public struct InventorySaveData
{
    /// <summary>
    /// The inventory system to save.
    /// </summary>
    public InventorySystem InvSystem;

    /// <summary>
    /// The position where the inventory is saved.
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// The rotation where the inventory is saved.
    /// </summary>
    public Quaternion Rotation;

    /// <summary>
    /// Initializes a new instance of the InventorySaveData structure.
    /// </summary>
    /// <param name="_invSystem">The inventory system to save.</param>
    /// <param name="_position">The position where the inventory is saved.</param>
    /// <param name="_rotation">The rotation where the inventory is saved.</param>
    public InventorySaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation)
    {
        InvSystem = _invSystem;
        Position = _position;
        Rotation = _rotation;
    }

    /// <summary>
    /// Initializes a new instance of the InventorySaveData structure with default position and rotation.
    /// </summary>
    /// <param name="_invSystem">The inventory system to save.</param>
    public InventorySaveData(InventorySystem _invSystem)
    {
        InvSystem = _invSystem;
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
    }
}