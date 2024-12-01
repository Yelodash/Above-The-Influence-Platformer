using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Script for the coincollection.
/// </summary>
public class CollectCoins : MonoBehaviour
{
    /// <summary>
    /// The number of coins collected.
    /// </summary>
    public int Coin = 0;

    /// <summary>
    ///The text that displays the number of coins collected.
    /// </summary>
    public TextMeshProUGUI coinText;

    /// <summary>
    ///  Set to store the names of the coin prefabs that have been collected.q
    /// </summary>
    private HashSet<string> collectedCoinPrefabs = new HashSet<string>();

    /// <summary>
    /// When the player collides with the coin, the coin is collected and the coin count is updated.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin")) return;

        // Check if the coin is a clone of a prefab
        if (other.gameObject.transform.parent != null && other.gameObject.transform.parent.CompareTag("Coin"))
        {
            // If it's a clone, do not collect it
            return;
        }

        // Check if the coin prefab name has been collected before
        string coinPrefabName = other.gameObject.name;

        // Check if the coin is a prefab or a clone
        bool isPrefab = other.gameObject.transform.parent == null || !other.gameObject.transform.parent.CompareTag("Coin");

        if (!collectedCoinPrefabs.Contains(coinPrefabName) && isPrefab)
        {
            Coin++;
            coinText.text = "Coins: " + Coin.ToString();
            collectedCoinPrefabs.Add(coinPrefabName); // Add the coin prefab name to the collected set
            Debug.Log("Coin Collected");
        }
    }

    /// <summary>
    ///  Method to remove a coin prefab name from the collected set
    /// </summary>
    /// <param name="coinPrefabName"></param>
    public void RemoveCoinPrefab(string coinPrefabName)
    {
        if (collectedCoinPrefabs.Contains(coinPrefabName))
        {
            collectedCoinPrefabs.Remove(coinPrefabName);
        }
    }
}