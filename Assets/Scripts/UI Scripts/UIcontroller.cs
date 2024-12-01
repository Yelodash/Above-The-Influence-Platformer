using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the display of the UI.
/// </summary>
public class UIcontroller : MonoBehaviour
{
    /// <summary>
    ///The shop keeper display.
    /// </summary>
    [SerializeField] private ShopKeeperDisplay _shopKeeperDisplay;

    /// <summary>
    /// Disables the shop keeper display on awake.
    /// </summary>
    private void Awake()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
    }

    /// <summary>
    /// Subscribes to the OnShopWindowRequested event.
    /// </summary>
    private void OnEnable()
    {
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
    }

    /// <summary>
    /// Unsubscribes from the OnShopWindowRequested event.
    /// </summary>
    private void OnDisable()
    {
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
    }

    /// <summary>
    /// Checks for the escape key to close the shop keeper display.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && GameManager.Instance.isShopOpen)
        {
            _shopKeeperDisplay.gameObject.SetActive(false);
            GameManager.Instance.isShopOpen = false;
        }

        if (_shopKeeperDisplay.isActiveAndEnabled)
        {
            GameManager.Instance.isShopOpen = true;
        }
    }

    /// <summary>
    /// Displays the shop window.
    /// </summary>
    /// <param name="shopSystem"></param>
    /// <param name="playerInventory"></param>
    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);
        _shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);
        //GameManager.Instance.testBool = true;
    }
}
