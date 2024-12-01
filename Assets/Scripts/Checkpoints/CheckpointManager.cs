using UnityEngine;
/// <summary>
/// Manages game checkpoints.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the CheckpointManager.
    /// </summary>
    private static CheckpointManager _instance;

    /// <summary>
    /// Gets the singleton instance of the CheckpointManager.
    /// </summary>
    public static CheckpointManager Instance { get { return _instance; } }

    /// <summary>
    /// The position of the last registered checkpoint.
    /// </summary>
    private Vector3 lastCheckpointPosition;
    
    /// <summary>
    /// The initial maximum health set at the last registered checkpoint.
    /// </summary>
    private int initialMaxHealth;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Sets the checkpoint position and initial maximum health.
    /// </summary>
    /// <param name="position">The position of the checkpoint.</param>
    /// <param name="maxHealth">The initial maximum health.</param>
    public void SetCheckpoint(Vector3 position, int maxHealth)
    {
        lastCheckpointPosition = position;
        initialMaxHealth = maxHealth > 0 ? maxHealth : 100; // Ensure positive value, default to 100

        Debug.Log($"Checkpoint registered at {position}. Initial Max Health set to {initialMaxHealth}.");
    }

    /// <summary>
    /// Gets the position of the last registered checkpoint.
    /// </summary>
    /// <returns>The position of the last registered checkpoint.</returns>
    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }

    /// <summary>
    /// Gets the initial maximum health set at the last registered checkpoint.
    /// </summary>
    /// <returns>The initial maximum health.</returns>
    public int GetInitialMaxHealth()
    {
        return initialMaxHealth;
    }
}
