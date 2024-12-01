using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Represents a dynamic inventory display in the UI.
/// </summary>
public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlot_UI slotPrefab;

    /// <summary>
    /// Refreshes the dynamic inventory display.
    /// </summary>
    /// <param name="invToDisplay"></param>
    /// <param name="offset"></param>
    public void RefreshDynamicInventory(InventorySystem invToDisplay, int offset)
    {
        ClearSlots();
        inventorySystem = invToDisplay;
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged += UpdateSlot;
        AssignSlot(invToDisplay, offset);
    }

    /// <summary>
    /// Assigns the inventory to the display.
    /// </summary>
    /// <param name="invToDisplay"></param>
    /// <param name="offset"></param>
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {

        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null) return;

        for (int i = offset; i < invToDisplay.InventorySize; i++)
        {
            var uiSlot = Instantiate(slotPrefab, transform);
            slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);
            uiSlot.Init(invToDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }
    }

    /// <summary>
    ///Clears the slots in the display.
    /// </summary>
    private void ClearSlots()
    {
        foreach (var item in transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        if (slotDictionary != null) slotDictionary.Clear();
    }

    
    /// <summary>
    /// Unsubscribes from the OnInventorySlotChanged event.
    /// </summary>
    private void OnDisable()
    {
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }
}