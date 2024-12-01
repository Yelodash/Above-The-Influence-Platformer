using HealthSystem;
using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Handles the logic for the blackhole.
    /// </summary>
    public class Blackhole : MonoBehaviour
    {
        /// <summary>
        /// When the player collides with the blackhole, the player takes enough damage to die and the level restarts.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                UnitHealth unitHealth = other.GetComponent<UnitHealth>();
                unitHealth.TakeDamage(9999);
                Debug.Log("Restarting level...");
            }
        }
    }
}
