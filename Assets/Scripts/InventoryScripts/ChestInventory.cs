using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Represents the inventory  holder that acts as a chest and can be interacted with.
/// </summary>
[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    /// <summary>
    /// Event invoked when the interaction with the chest is complete.
    /// </summary>
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    /// <summary>
    /// Initializes the chest inventory.
    /// </summary>
    private void Start()
    {
        SaveInventoryData();
    }

    /// <summary>
    /// Loads inventory data from saved game data.
    /// </summary>
    /// <param name="data">The saved game data.</param>
    protected override void LoadInventory(SaveData data)
    {
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            this.primaryInventorySystem = chestData.InvSystem;
            this.transform.position = chestData.Position;
            this.transform.rotation = chestData.Rotation;
        }
    }

    /// <summary>
    /// Handles interaction with the chest.
    /// </summary>
    /// <param name="interactor">The interactor initiating the interaction.</param>
    /// <param name="interactSuccessful">Outputs whether the interaction was successful.</param>
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        DisplayInventory();
        interactSuccessful = true;
    }

    /// <summary>
    /// Ends the interaction with the chest.
    /// </summary>
    public void EndInteraction()
    {
        // Add any logic needed to end interaction
    }

    /// <summary>
    /// Saves inventory data to the save game manager.
    /// </summary>
    private void SaveInventoryData()
    {
        var chestSaveData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);
        SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    /// <summary>
    /// Displays the chest inventory.
    /// </summary>
    private void DisplayInventory()
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem, 0);
    }
}