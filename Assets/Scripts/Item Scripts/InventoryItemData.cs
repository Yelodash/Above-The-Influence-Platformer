using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a scriptable object that defines what an item is in the game.
/// It could be inherited from to have branched versions of items, for example potions and equipment.
/// </summary>
[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    /// <summary>
    /// The unique identifier for the item.
    /// </summary>
    public int ID = -1;

    /// <summary>
    /// The display name of the item.
    /// </summary>
    public string DisplayName;

    /// <summary>
    /// The description of the item.
    /// </summary>
    [TextArea(4, 4)]
    public string Description;

    /// <summary>
    /// The icon representing the item.
    /// </summary>
    public Sprite Icon;

    /// <summary>
    /// The maximum stack size of the item.
    /// </summary>
    public int MaxStackSize;

    /// <summary>
    /// The gold value of the item.
    /// </summary>
    public int GoldValue;

    /// <summary>
    /// The prefab representing the item in the game world.
    /// </summary>
    public GameObject ItemPrefab;
}
