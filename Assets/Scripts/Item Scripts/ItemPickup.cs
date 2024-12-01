using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an item pickup in the game world.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickup : MonoBehaviour
{
    /// <summary>
    /// The radius within which the item can be picked up.
    /// </summary>
    public float PickUpRadius = 1f;

    /// <summary>
    /// The data of the inventory item associated with this pickup.
    /// </summary>
    public InventoryItemData ItemData;

    [SerializeField] private float _rotationSpeed = 20f;

    /// <summary>
    /// The collider component of the item pickup.
    /// </summary>
    private SphereCollider myCollider;

    /// <summary>
    /// The save data of the item pickup.
    /// </summary>
    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;
    
    /// <summary>
    /// Initializes the item pickup.
    /// </summary>
    private void Awake()
    {
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);
        
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
    }

    
    
    private void Start()
    {
        id = GetComponent<UniqueID>().ID;
        SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    /// <summary>
    /// update the rotation of the item pickup.
    /// </summary>
    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// loads the game data for the item pickup.
    /// </summary>
    /// <param name="data"></param>
    private void LoadGame(SaveData data)
    {
        if (data.collectedItems.Contains(id)) Destroy(this.gameObject);
    }

    /// <summary>
    /// removes the item pickup from the save data when destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (SaveGameManager.data.activeItems.ContainsKey(id)) SaveGameManager.data.activeItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadGame;
    }

    /// <summary>
    /// Handles the player picking up the item.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        
        if (!inventory) return;

        if (inventory.AddToInventory(ItemData, 1))
        {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(this.gameObject);
        }
    }
}

/// <summary>
/// Represents the save data for an item pickup.
/// </summary>
[System.Serializable]
public struct ItemPickUpSaveData
{
    /// <summary>
    /// The data of the inventory item.
    /// </summary>
    public InventoryItemData ItemData;

    /// <summary>
    /// The position of the item pickup.
    /// </summary>
    public Vector3 Position;

    /// <summary>
    /// The rotation of the item pickup.
    /// </summary>
    public Quaternion Rotation;

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemPickUpSaveData"/> struct.
    /// </summary>
    /// <param name="_data">The inventory item data.</param>
    /// <param name="_position">The position of the item pickup.</param>
    /// <param name="_rotation">The rotation of the item pickup.</param>
    public ItemPickUpSaveData(InventoryItemData _data, Vector3 _position, Quaternion _rotation)
    {
        ItemData = _data;
        Position = _position;
        Rotation = _rotation;
    }
}
