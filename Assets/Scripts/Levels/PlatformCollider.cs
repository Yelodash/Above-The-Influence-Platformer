using Managers;
using UnityEngine;

namespace Levels
{
    /// <summary>
    /// This class is used to inform the PlayerCharacter that the player is on a platform.
    /// </summary>
    public class PlatformCollider : MonoBehaviour
    {
        public static event PlayerOnPlatform PlayerOnPlatform;
        public static event PlayerOffPlatform PlayerOffPlatform;
        
        private Transform playerTransform;
        private Transform platformTransform;
    
        /// <summary>
        /// Invokes an event when the player collides with the platform.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                PlayerOnPlatform?.Invoke();
            }
            
            if (other.gameObject.layer == 7 || other.gameObject.layer == 8)
            {
                playerTransform = other.transform;
                playerTransform.parent = transform;
            }
        }

        /// <summary>
        /// Invokes an event when the player exits the platform.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                PlayerOffPlatform?.Invoke();
                other.gameObject.transform.parent = null;
            }
        }
    }
}