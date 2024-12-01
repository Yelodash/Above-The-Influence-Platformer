using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a slot in the shop's inventory.
/// </summary>
[System.Serializable]
public class ShopSlot : ItemSlot
{
    /// <summary>
    /// Initializes a new instance of the ShopSlot class.
    /// </summary>
    public ShopSlot()
    {
        ClearSlot();
    } 
}