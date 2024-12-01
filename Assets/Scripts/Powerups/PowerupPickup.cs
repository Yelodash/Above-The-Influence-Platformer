using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// Base class for all powerups
    /// </summary>
    public class PowerupPickup : MonoBehaviour
    {
        /// <summary>
        /// Called when the player collides with the powerup
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        public virtual void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                Debug.Log("Powerup picked up");
                Destroy(gameObject);
            }
        }
    }
}
