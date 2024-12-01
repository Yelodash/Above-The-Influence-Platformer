using Audio;
using Managers;
using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Script that manages the bouncy bin in the game.
    /// </summary>
    public class BouncyBin : MonoBehaviour
    {
        public static event OnBinHit BinHitEvent;
    
        [SerializeField] private float bounceForce = 10f;
    
        /// <summary>
        /// Invokes an event when the player collides with the bouncy bin.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BouncyBin"))
            {
                BinHitEvent?.Invoke((int)bounceForce);
                AudioManager.Instance.PlaySoundAtPoint(4, gameObject.transform.position);
            }
        }
    }
}