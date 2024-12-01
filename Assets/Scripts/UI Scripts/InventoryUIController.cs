using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// Manages the display of the inventory UI.
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    [FormerlySerializedAs("chestPanel")] public DynamicInventoryDisplay inventoryPanel;
    public DynamicInventoryDisplay playerBackpackPanel;

    
    /// <summary>
    /// Disables the inventory panel on awake.
    /// </summary>
    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
        playerBackpackPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Subscribes to the OnDynamicInventoryDisplayRequested event.
    /// </summary>
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
    }

    /// <summary>
    /// Unsubscribes from the OnDynamicInventoryDisplayRequested event.
    /// </summary>
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
    }

    /// <summary>
    /// Checks for the escape key to close the inventory panel.
    /// </summary>
    void Update()
    {
        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame) 
            inventoryPanel.gameObject.SetActive(false);

        if (playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            playerBackpackPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Displays the inventory panel.
    /// </summary>
    /// <param name="invToDisplay"></param>
    /// <param name="offset"></param>
    void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
}