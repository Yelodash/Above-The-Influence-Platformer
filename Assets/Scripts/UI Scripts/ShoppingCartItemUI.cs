using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
///  Represents a shopping cart item in the UI.
/// </summary>
public class ShoppingCartItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemText;

    /// <summary>
    /// Sets the text of the item.
    /// </summary>
    /// <param name="newString"></param>
    public void SetItemText(string newString)
    {
        _itemText.text = newString;
    }
}