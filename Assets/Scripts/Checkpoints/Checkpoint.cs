using Audio;
using HealthSystem;
using UnityEngine;

/// <summary>
/// Represents a checkpoint in the game world.
/// </summary>
public class Checkpoint : MonoBehaviour
{
    /// <summary>
    /// Called when a collider enters the trigger area of the checkpoint.
    /// </summary>
    /// <param name="other">The collider that entered the trigger area.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Update the last checkpoint reached by passing the position and maximum health of the player
            int maxHealth = other.GetComponent<UnitHealth>().MaxHealth;
            CheckpointManager.Instance.SetCheckpoint(transform.position, maxHealth);
            AudioManager.Instance.PlaySound(14);
        }
    }
}
