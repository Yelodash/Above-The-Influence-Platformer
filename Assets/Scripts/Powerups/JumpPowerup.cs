using Audio;
using Managers;
using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The JumpPowerup class is a script that is used to detect when the player picks up a jump powerup
    /// </summary>
    public class JumpPowerup : PowerupPickup
    {
        public static event JumpForcePowerupEventHandler JumpForcePowerupObtained;

        /// <summary>
        /// Invokes an event when colliding with the player.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        public override void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                AudioManager.Instance.PlaySound(Random.Range(12, 16));
                JumpForcePowerupObtained?.Invoke();
                AttributeManager.Instance.jumpForce *= PowerupManager.Instance.jumpPowerupMultiplier;
                AttributeManager.Instance.crouchJumpForce *= PowerupManager.Instance.jumpPowerupMultiplier;
            
                Debug.Log("Jump Powerup picked up!");
                Destroy(gameObject);
            }
        }
    }
}