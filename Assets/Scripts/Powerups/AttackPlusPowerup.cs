using Audio;
using Managers;
using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The AttackPlusPowerup class is a script that is used to detect when the player picks up an attack powerup.
    /// </summary>
    public class AttackPlusPowerup : PowerupPickup
    {
        public static event AttackPowerupEventHandler AttackPowerupObtained;
        [SerializeField] private float attackPowerIncrease;
    
        /// <summary>
        /// Invokes an event when colliding with the player.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        public override void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                AudioManager.Instance.PlaySound(Random.Range(12, 16));
                AttackPowerupObtained?.Invoke();
                AttributeManager.Instance.attackPower *= attackPowerIncrease;

                Debug.Log("Attack Powerup picked up!");
                Destroy(gameObject);
            }
        }
    }
}
