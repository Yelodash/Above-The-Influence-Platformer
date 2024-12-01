using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///Represents a static inventory display in the UI.
/// </summary>
public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlot_UI[] slots;


    /// <summary>
    ///Subscribes to the OnPlayerInventoryChanged event.
    /// </summary>
    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    /// <summary>
    /// Unsubscribes from the OnPlayerInventoryChanged event.
    /// </summary>
    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    /// <summary>
    /// Refreshes the static display.
    /// </summary>
    private void RefreshStaticDisplay()
    {
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory assigned to {this.gameObject}");

        AssignSlot(inventorySystem, 0);
    }

    /// <summary>
    ///Initializes the static display.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        RefreshStaticDisplay();
    }

    /// <summary>
    /// Assigns the inventory to the display.
    /// </summary>
    /// <param name="invToDisplay"></param>
    /// <param name="offset"></param>
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        int minSlots = Mathf.Min(slots.Length, inventorySystem.InventorySlots.Count);

        for (int i = 0; i < minSlots; i++)
        {
            if (!slotDictionary.ContainsKey(slots[i]))
            {
                slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
                slots[i].Init(inventorySystem.InventorySlots[i]);
            }
            else
            {
                Debug.LogWarning("Duplicate key detected: " + slots[i].name);
            }
        }
    }
}