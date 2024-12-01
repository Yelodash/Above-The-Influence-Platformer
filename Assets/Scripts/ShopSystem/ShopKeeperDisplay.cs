using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the display of the shop keeper.
/// </summary>
public class ShopKeeperDisplay : MonoBehaviour
{
    /// <summary>
    /// The prefab for the shop slot UI.
    /// </summary>
    [SerializeField] private ShopSlotUI _shopSlotPrefab;
    
    /// <summary>
    /// The prefab for the shopping cart item UI.
    /// </summary>
    [SerializeField] private ShoppingCartItemUI _shoppingCartItemPrefab;

    /// <summary>
    /// The buy tab button.
    /// </summary>
    [SerializeField] private Button _buyTab;
    
    /// <summary>
    /// The sell tab button.
    /// </summary>
    [SerializeField] private Button _sellTab;

    /// <summary>
    ///The text displaying the total cost of the shopping cart.
    /// </summary>
    [Header("Shopping Cart")] 
    [SerializeField] private TextMeshProUGUI _basketTotalText;
    
    /// <summary>
    ///The text displaying the player's gold.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _playerGoldText;
    
    /// <summary>
    /// The text displaying the shop's gold.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _shopGoldText;
    
    /// <summary>
    ///The button to buy items.
    /// </summary>
    [SerializeField] private Button _buyButton;
    
    /// <summary>
    /// The text of the buy button.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _buyButtonText;

    [Header("Item Preview Section")] 
    [SerializeField] private Image _itemPreviewSprite;
    
    /// <summary>
    /// The text displaying the name of the item preview.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _itemPreviewName;
    
    /// <summary>
    /// The text displaying the description of the item preview.
    /// </summary>
    [SerializeField] private TextMeshProUGUI _itemPreviewDescription;

    /// <summary>
    /// The content panel for the item list.
    /// </summary>
    [SerializeField] private GameObject _itemListContentPanel;
    
    
    /// <summary>
    /// The content panel for the shopping cart.
    /// </summary>
    [SerializeField] private GameObject _shoppingCartContentPanel;

    /// <summary>
    /// The total cost of the shopping cart.
    /// </summary>
    private int _basketTotal;
    
    /// <summary>
    /// Whether the shop keeper is selling items.
    /// </summary>
    private bool _isSelling;
    
    /// <summary>
    /// The shop system.
    /// </summary>
    private ShopSystem _shopSystem;
    
    /// <summary>
    /// The player's inventory holder.
    /// </summary>
    private PlayerInventoryHolder _playerInventoryHolder;

    /// <summary>
    /// The shopping cart.
    /// </summary>
    private Dictionary<InventoryItemData, int> _shoppingCart = new Dictionary<InventoryItemData, int>();

    /// <summary>
    /// The shopping cart UI.
    /// </summary>
    private Dictionary<InventoryItemData, ShoppingCartItemUI> _shoppingCartUI =
        new Dictionary<InventoryItemData, ShoppingCartItemUI>();

    
    /// <summary>
    /// Displays the shop window.
    /// </summary>
    /// <param name="shopSystem"></param>
    /// <param name="playerInventoryHolder"></param>
    public void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventoryHolder)
    {
        _shopSystem = shopSystem;
        _playerInventoryHolder = playerInventoryHolder;

        RefreshDisplay();
    }

    /// <summary>
    /// Refreshes the display of the shop keeper.
    /// </summary>
    private void RefreshDisplay()
    {
        if (_buyButton != null)
        {
            _buyButtonText.text = _isSelling ? "Sell Items" : "Buy Items";
            _buyButton.onClick.RemoveAllListeners();
            if (_isSelling) _buyButton.onClick.AddListener(SellItems);
            else _buyButton.onClick.AddListener(BuyItems);
        }
        
        ClearSlots();
        ClearItemPreview();

        _basketTotalText.enabled = false;
        _buyButton.gameObject.SetActive(false);
        _basketTotal = 0;
        _playerGoldText.text = $"Player Gold: {_playerInventoryHolder.PrimaryInventorySystem.Gold}";
        _shopGoldText.text = $"Shop Gold: {_shopSystem.AvailableGold}";
        
        if (_isSelling) DisplayPlayerInventory();
        else DisplayShopInventory();
       
    }

    /// <summary>
    ///  Buys the items in the shopping cart.
    /// </summary>
    private void BuyItems()
    {
        if (_playerInventoryHolder.PrimaryInventorySystem.Gold < _basketTotal) return;

        if (!_playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;

        foreach (var kvp in _shoppingCart)
        {
            _shopSystem.PurchaseItem(kvp.Key, kvp.Value);

            for (int i = 0; i < kvp.Value; i++)
            {
                _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(kvp.Key, 1);
            }
        }

        _shopSystem.GainGold(_basketTotal);
        _playerInventoryHolder.PrimaryInventorySystem.SpendGold(_basketTotal);
        
        RefreshDisplay();
    }

    /// <summary>
    ///  Sells the items in the shopping cart.
    /// </summary>
    private void SellItems()
    {
        if (_shopSystem.AvailableGold < _basketTotal) return;

        foreach (var kvp in _shoppingCart)
        {
            var price = GetModifiedPrice(kvp.Key, kvp.Value, _shopSystem.SellMarkUp);
            
            _shopSystem.SellItem(kvp.Key, kvp.Value, price);

            _playerInventoryHolder.PrimaryInventorySystem.GainGold(price);
            _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(kvp.Key, kvp.Value);
        }
        
        RefreshDisplay();
    }
    
    /// <summary>
    ///  Clears the slots in the item list and shopping cart.
    /// </summary>
    private void ClearSlots()
    {
        _shoppingCart = new Dictionary<InventoryItemData, int>();
        _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();
        
        foreach (var item in _itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
        
        foreach (var item in _shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    
    /// <summary>
    ///  Displays the shop's inventory.
    /// </summary>
    private void DisplayShopInventory()
    {
        foreach (var item in _shopSystem.ShopInventory)
        {
            if (item.ItemData == null) continue;

            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Initialize(item, _shopSystem.BuyMarkUp);
        }
    }

    
    /// <summary>
    /// Displays the player's inventory.
    /// </summary>
    private void DisplayPlayerInventory()
    {
        foreach (var item in _playerInventoryHolder.PrimaryInventorySystem.GetAllItemsHeld())
        {
            var tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);

            var shopSlot = Instantiate(_shopSlotPrefab, _itemListContentPanel.transform);
            shopSlot.Initialize(tempSlot, _shopSystem.SellMarkUp);
        }
    }
    
    /// <summary>
    ///  Removes an item from the shopping cart.
    /// </summary>
    /// <param name="shopSlotUI"></param>
    public void RemoveItemFromCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;
        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);
        
        if (_shoppingCart.ContainsKey(data))
        {
            _shoppingCart[data]--;
            var newString = $"{data.DisplayName} ({price}G) x{_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);

            if (_shoppingCart[data] <= 0)
            {
                _shoppingCart.Remove(data);
                var tempObj = _shoppingCartUI[data].gameObject;
                _shoppingCartUI.Remove(data);
                Destroy(tempObj);
            }
        }

        _basketTotal -= price;
        _basketTotalText.text = $"Total: {_basketTotal}G";

        if (_basketTotal <= 0 && _basketTotalText.IsActive())
        {
            _basketTotalText.enabled = false;
            _buyButton.gameObject.SetActive(false);
            ClearItemPreview();
            return;
        }
        
        CheckCartVsAvailableGold();
    }

    /// <summary>
    /// Clears the item preview.
    /// </summary>
    private void ClearItemPreview()
    {
        _itemPreviewSprite.sprite = null;
        _itemPreviewSprite.color = Color.clear;
        _itemPreviewName.text = "";
        _itemPreviewDescription.text = "";
    }

    
    /// <summary>
    ///  Adds an item to the shopping cart.
    /// </summary>
    /// <param name="shopSlotUI"></param>
    public void AddItemToCart(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;

        UpdateItemPreview(shopSlotUI);
        
        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);
        

        if (_shoppingCart.ContainsKey(data))
        {
            _shoppingCart[data]++;
            var newString = $"{data.DisplayName} ({price}G) x{_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);
        }
        else
        {
            _shoppingCart.Add(data, 1);

            var shoppingCartTextObj = Instantiate(_shoppingCartItemPrefab, _shoppingCartContentPanel.transform);
            var newString = $"{data.DisplayName} ({price}G) x1";
            shoppingCartTextObj.SetItemText(newString);
            _shoppingCartUI.Add(data, shoppingCartTextObj);
        }
        
        _basketTotal += price;
        _basketTotalText.text = $"Total: {_basketTotal}G";

        if (_basketTotal > 0 && !_basketTotalText.IsActive())
        {
            _basketTotalText.enabled = true;
            _buyButton.gameObject.SetActive(true);
        }

        CheckCartVsAvailableGold();

    }

    
    
    /// <summary>
    ///  Checks the shopping cart against the available gold.
    /// </summary>
    private void CheckCartVsAvailableGold()
    {
        var goldToCheck = _isSelling ? _shopSystem.AvailableGold : _playerInventoryHolder.PrimaryInventorySystem.Gold;
        _basketTotalText.color = _basketTotal > goldToCheck ? Color.red : Color.white;

        if (_isSelling || _playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;

        _basketTotalText.text = "Not enough room in inventory.";
        _basketTotalText.color = Color.red;
    }

    
    /// <summary>
    ///  Gets the modified price of an item.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="amount"></param>
    /// <param name="markUp"></param>
    /// <returns></returns>
    public static int GetModifiedPrice(InventoryItemData data, int amount, float markUp)
    {
        var baseValue = data.GoldValue * amount;

        return Mathf.FloorToInt(baseValue + baseValue * markUp);
    }

    
    /// <summary>
    ///  Updates the item preview.
    /// </summary>
    /// <param name="shopSlotUI"></param>
    private void UpdateItemPreview(ShopSlotUI shopSlotUI)
    {
        var data = shopSlotUI.AssignedItemSlot.ItemData;

        _itemPreviewSprite.sprite = data.Icon;
        _itemPreviewSprite.color = Color.white;
        _itemPreviewName.text = data.DisplayName;
        _itemPreviewDescription.text = data.Description;
    }

    
    /// <summary>
    ///  Clears the shopping cart.
    /// </summary>
    public void OnBuyTabPressed()
    {
        _isSelling = false;
        RefreshDisplay();
        Debug.Log("Switched to buy tab."); // Ajout du log pour le bouton "Buy"
    }

    
    /// <summary>
    ///  Clears the shopping cart.
    /// </summary>
    public void OnSellTabPressed()
    {
        _isSelling = true;
        RefreshDisplay();
        Debug.Log("Switched to sell tab."); // Ajout du log pour le bouton "Sell"
    }

    
}
