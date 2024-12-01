using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents the shop system that manages the shop's inventory and transactions.
/// </summary>
[System.Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> _shopInventory;
    [SerializeField] private int _availableGold;
    [SerializeField] private float _buyMarkUp;
    [SerializeField] private float _sellMarkUp;

    /// <summary>
    /// Gets the list of shop slots representing the shop's inventory.
    /// </summary>
    public List<ShopSlot> ShopInventory => _shopInventory;

    /// <summary>
    /// Gets the available gold in the shop.
    /// </summary>
    public int AvailableGold => _availableGold;

    /// <summary>
    /// Gets the buy mark-up percentage applied to item prices when buying from the shop.
    /// </summary>
    public float BuyMarkUp => _buyMarkUp;

    /// <summary>
    /// Gets the sell mark-up percentage applied to item prices when selling to the shop.
    /// </summary>
    public float SellMarkUp => _sellMarkUp;

    /// <summary>
    /// Initializes a new instance of the ShopSystem class.
    /// </summary>
    /// <param name="size">The size of the shop's inventory.</param>
    /// <param name="gold">The initial amount of gold available in the shop.</param>
    /// <param name="buyMarkUp">The buy mark-up percentage.</param>
    /// <param name="sellMarkUp">The sell mark-up percentage.</param>
    public ShopSystem(int size, int gold, float buyMarkUp, float sellMarkUp)
    {
        _availableGold = gold;
        _buyMarkUp = buyMarkUp;
        _sellMarkUp = sellMarkUp;

        SetShopSize(size);
    }

    private void SetShopSize(int size)
    {
        _shopInventory = new List<ShopSlot>(size);

        for (int i = 0; i < size; i++)
        {
            _shopInventory.Add(new ShopSlot());
        }
    }

    /// <summary>
    /// Adds an item to the shop's inventory.
    /// </summary>
    /// <param name="data">The item data to add.</param>
    /// <param name="amount">The amount of the item to add.</param>
    public void AddToShop(InventoryItemData data, int amount)
    {
        if (ContainsItem(data, out ShopSlot shopSlot))
        {
            shopSlot.AddToStack(amount);
            return;
        }

        var freeSlot = GetFreeSlot();
        freeSlot.AssignItem(data, amount);
    }

    private ShopSlot GetFreeSlot()
    {
        var freeSlot = _shopInventory.FirstOrDefault(i => i.ItemData == null);

        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            _shopInventory.Add(freeSlot);
        }

        return freeSlot;
    }

    /// <summary>
    /// Checks if the shop's inventory contains a specific item.
    /// </summary>
    /// <param name="itemToAdd">The item to check.</param>
    /// <param name="shopSlot">The slot containing the item, if found.</param>
    /// <returns>True if the item is found in the shop's inventory; otherwise, false.</returns>
    public bool ContainsItem(InventoryItemData itemToAdd, out ShopSlot shopSlot)
    {
        shopSlot = _shopInventory.Find(i => i.ItemData == itemToAdd);
        return shopSlot != null;
    }

    /// <summary>
    /// Handles the purchase of an item from the shop.
    /// </summary>
    /// <param name="data">The item data to purchase.</param>
    /// <param name="amount">The amount of the item to purchase.</param>
    public void PurchaseItem(InventoryItemData data, int amount)
    {
        if (!ContainsItem(data, out ShopSlot slot)) return;

        slot.RemoveFromStack(amount);
    }

    /// <summary>
    /// Increases the available gold in the shop.
    /// </summary>
    /// <param name="basketTotal">The total gold amount gained.</param>
    public void GainGold(int basketTotal)
    {
        _availableGold += basketTotal;
    }

    /// <summary>
    /// Sells an item to the shop.
    /// </summary>
    /// <param name="kvpKey">The item data to sell.</param>
    /// <param name="kvpValue">The amount of the item to sell.</param>
    /// <param name="price">The price of the item.</param>
    public void SellItem(InventoryItemData kvpKey, int kvpValue, int price)
    {
        AddToShop(kvpKey, kvpValue);
        ReduceGold(price);
    }

    private void ReduceGold(int price)
    {
        _availableGold -= price;
    }
}
