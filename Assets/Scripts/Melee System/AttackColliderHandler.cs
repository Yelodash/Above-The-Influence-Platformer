using Audio;
using HealthSystem;
using Powerups;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Melee_System
{
    /// <summary>
    /// Handles the attack collider for the player.
    /// </summary>
    public class AttackColliderHandler : MonoBehaviour
    {
   
        private float damage;
        private int damageConversion;

        /// <summary>
        /// If the tag is "SpinAttackGO", the damage is set to the spin attack power.
        /// If the tag is "HandAttackGO", the damage is set to the melee attack power.
        /// </summary>
        private void Update()
        {
            if (CompareTag("SpinAttackGO"))
            {
                damage = AttributeManager.Instance.attackPowerSpin;
            }
            else if (CompareTag("HandAttackGO"))
            {
                damage = AttributeManager.Instance.attackPowerMelee;
            }
        }
    
        /// <summary>
        /// Called when the Collider enters another Collider.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);
        
            // 8 is the layer for "Damageable" objects
            if (other.gameObject.layer == 8 )
            {
                AudioManager.Instance.PlaySound(Random.Range(23, 25));
            
                UnitHealth unitHealth = other.GetComponent<UnitHealth>();
                if (unitHealth != null)
                {
                    damageConversion = Mathf.RoundToInt(damage);
                    unitHealth.TakeDamage(damageConversion);
                }
            }
        }
    }
}
