using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///  Represents a slot in the shop's inventory.
/// </summary>
public class ShopSlotUI : MonoBehaviour
{
    
    [SerializeField] private Image _itemSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private ShopSlot _assignedItemSlot;

    /// <summary>
    ///  The assigned item slot.
    /// </summary>
    public ShopSlot AssignedItemSlot => _assignedItemSlot;

    [SerializeField] private Button _addItemToCartButton;
    [SerializeField] private Button _removeItemFromCartButton;

    private int _tempAmount;
    
    /// <summary>
    ///  The parent display of the slot.
    /// </summary>
    public ShopKeeperDisplay ParentDisplay { get; private set; }
    public float MarkUp { get; private set; }
    
    /// <summary>
    /// Initializes the shop slot UI.
    /// </summary>
    private void Awake()
    {
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;
        _itemName.text = "";
        _itemCount.text = "";
        
        _addItemToCartButton?.onClick.AddListener(AddItemToCart);
        _removeItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }
    /// <summary>
    /// Initializes the shop slot UI.
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="markUp"></param>
    public void Initialize (ShopSlot slot, float markUp)
    {
        _assignedItemSlot = slot;
        MarkUp = markUp;
        _tempAmount = slot.StackSize;
        UpdateUISlot();
    }
    
    /// <summary>
    ///  Updates the UI slot with the given slot data.
    /// </summary>
    private void UpdateUISlot()
    {
        if (_assignedItemSlot.ItemData != null)
        {
            _itemSprite.sprite = _assignedItemSlot.ItemData.Icon;
            _itemSprite.color = Color.white;
            _itemCount.text = _assignedItemSlot.StackSize.ToString();
            var modifiedPrice = ShopKeeperDisplay.GetModifiedPrice(_assignedItemSlot.ItemData, 1, MarkUp);
            
            _itemName.text = $"{_assignedItemSlot.ItemData.DisplayName} - {modifiedPrice}G";
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.color = Color.clear;
            _itemName.text = "";
            _itemCount.text = "";
        }
    }

    /// <summary>
    /// Removes an item from the cart.
    /// </summary>
    private void RemoveItemFromCart()
    {
        if (_tempAmount == _assignedItemSlot.StackSize) return;

        _tempAmount++;
        ParentDisplay.RemoveItemFromCart(this);
        _itemCount.text = _tempAmount.ToString();
    }

    /// <summary>
    /// Adds an item to the cart.
    /// </summary>
    private void AddItemToCart()
    {
        if (_tempAmount <= 0) return;
        
        _tempAmount--;
        ParentDisplay.AddItemToCart(this);
        _itemCount.text = _tempAmount.ToString();
    }

    
}
