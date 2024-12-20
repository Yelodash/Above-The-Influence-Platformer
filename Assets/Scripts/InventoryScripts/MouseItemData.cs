using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Represents the data and behavior of the mouse item UI element.
/// </summary>
public class MouseItemData : MonoBehaviour
{
    /// <summary>
    /// The image representing the item sprite.
    /// </summary>
    public Image ItemSprite;

    /// <summary>
    /// The text representing the item count.
    /// </summary>
    public TextMeshProUGUI ItemCount;

    /// <summary>
    /// The inventory slot assigned to the mouse item.
    /// </summary>
    public InventorySlot AssignedInventorySlot;

    private Transform _playerTransform;
    public float _dropOffset = 0.5f;

    /// <summary>
    /// Initializes the mouse item data.
    /// </summary>
    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;
        ItemCount.text = "";

        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (_playerTransform == null) Debug.Log("Player not found!");
    }

    /// <summary>
    /// Updates the mouse item slot with the given inventory slot.
    /// </summary>
    /// <param name="invSlot">The inventory slot to assign to the mouse item.</param>
    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        AssignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }
    
    /// <summary>
    /// Updates the visual representation of the mouse item slot.
    /// </summary>
    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.ItemData.Icon;
        ItemCount.text = AssignedInventorySlot.StackSize.ToString();
        ItemSprite.color = Color.white;
    }

    /// <summary>
    /// Updates the mouse item slot.
    /// </summary>
    private void Update()
    {
        // TODO: Add controller support.

        if (AssignedInventorySlot.ItemData != null) // If has an item, follow the mouse position.
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                if(AssignedInventorySlot.ItemData.ItemPrefab != null) Instantiate(AssignedInventorySlot.ItemData.ItemPrefab, _playerTransform .position + _playerTransform.forward * _dropOffset,
                    Quaternion.identity);

                if (AssignedInventorySlot.StackSize > 1)
                {
                    AssignedInventorySlot.AddToStack(-1);
                    UpdateMouseSlot();
                }
                else
                {
                    ClearSlot();    
                }
                
            }
        }
    }

    /// <summary>
    /// Clears the mouse item slot.
    /// </summary>
    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
    }

    /// <summary>
    /// Checks if the mouse pointer is over any UI object.
    /// </summary>
    /// <returns>True if the mouse pointer is over a UI object, otherwise false.</returns>
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}