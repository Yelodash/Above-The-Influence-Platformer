using Interfaces;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents a shopkeeper NPC that the player can interact with.
/// </summary>
[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList _shopItemsHeld;
    [SerializeField] private ShopSystem _shopSystem;

    private ShopSaveData _shopSaveData;

    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    private string _id;

    private void Awake()
    {
        // Initialize the shop system with the provided shop items and settings
        _shopSystem =
            new ShopSystem(_shopItemsHeld.Items.Count, _shopItemsHeld.MaxAllowedGold, _shopItemsHeld.BuyMarkUp,
                _shopItemsHeld.SellMarkUp);

        foreach (var item in _shopItemsHeld.Items)
        {
            _shopSystem.AddToShop(item.ItemData, item.Amount);
        }

        _id = GetComponent<UniqueID>().ID;
        _shopSaveData = new ShopSaveData(_shopSystem);
    }

    private void Start()
    {
        // Add shop data to the save game manager
        if (!SaveGameManager.data._shopKeeperDictionary.ContainsKey(_id)) SaveGameManager.data._shopKeeperDictionary.Add(_id, _shopSaveData);
    }

    private void OnEnable()
    {
        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void LoadInventory(SaveData data)
    {
        // Load shop data from save game manager
        if (!data._shopKeeperDictionary.TryGetValue(_id, out ShopSaveData shopSaveData)) return;

        _shopSaveData = shopSaveData;
        _shopSystem = _shopSaveData.ShopSystem;
    }

    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadInventory;
    }

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    /// <summary>
    /// Interact with the shopkeeper.
    /// </summary>
    /// <param name="interactor">The interactor object interacting with the shopkeeper.</param>
    /// <param name="interactSuccessful">Flag indicating whether the interaction was successful.</param>
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            
            Vector3 shopkeeperPosition = transform.position;
            Vector3 interactorPosition = interactor.transform.position;
            
            float maxInteractionDistance = 3.0f; // Par exemple, 3 unités

           
            float distance = Vector3.Distance(shopkeeperPosition, interactorPosition);

           
            if (distance <= maxInteractionDistance)
            {
                Debug.Log("Interacting with the shopkeeper.");
                OnShopWindowRequested?.Invoke(_shopSystem, playerInv);
                interactSuccessful = true;
            }
            else
            {
                interactSuccessful = false;
                Debug.Log("Interactor is out of range for interaction with the shopkeeper.");
            }
        }
        else
        {
            interactSuccessful = false;
            Debug.LogError("Player inventory not found");
        }
    }

    public void EndInteraction()
    {
        // Additional actions to perform when the interaction ends
    }
}

/// <summary>
/// Represents data used to save the state of a shop.
/// </summary>
[System.Serializable]
public class ShopSaveData
{
    /// <summary>
    /// The shop system data to be saved.
    /// </summary>
    public ShopSystem ShopSystem;

    /// <summary>
    /// Constructs a new instance of the shop save data.
    /// </summary>
    /// <param name="shopSystem">The shop system data to be saved.</param>
    public ShopSaveData(ShopSystem shopSystem)
    {
        ShopSystem = shopSystem;
    }
}
