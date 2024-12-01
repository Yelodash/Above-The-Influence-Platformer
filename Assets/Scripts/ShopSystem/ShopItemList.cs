using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a list of items available in a shop.
/// </summary>
[CreateAssetMenu(menuName = "Shop System/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    [SerializeField] private List<ShopInventoryItem> _items;
    [SerializeField] private int _maxAllowedGold;
    [SerializeField] private float _sellMarkUp;
    [SerializeField] private float _buyMarkUp;

    /// <summary>
    /// The list of items available in the shop.
    /// </summary>
    public List<ShopInventoryItem> Items => _items;

    /// <summary>
    /// The maximum amount of gold allowed in the shop.
    /// </summary>
    public int MaxAllowedGold => _maxAllowedGold;

    /// <summary>
    /// The markup percentage for selling items.
    /// </summary>
    public float SellMarkUp => _sellMarkUp;

    /// <summary>
    /// The markup percentage for buying items.
    /// </summary>
    public float BuyMarkUp => _buyMarkUp;
}

/// <summary>
/// Represents an item in the shop's inventory.
/// </summary>
[System.Serializable]
public struct ShopInventoryItem
{
    /// <summary>
    /// The data of the inventory item.
    /// </summary>
    public InventoryItemData ItemData;

    /// <summary>
    /// The amount of the inventory item available in the shop.
    /// </summary>
    public int Amount;
}