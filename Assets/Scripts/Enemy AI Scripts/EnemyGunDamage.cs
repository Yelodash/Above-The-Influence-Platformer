using System.Collections;
using Audio;
using HealthSystem;
using Managers;
using UnityEngine;

namespace Enemy_AI_Scripts
{
    /// <summary>
    /// Manages the enemy attack in the game.
    /// </summary>
    public class EnemyGunDamage : MonoBehaviour
    {

        /// <summary>
        /// The amount of damage the enemy's gun does.
        /// </summary>
        [SerializeField] private int damage = 10;
        public HealthBarScript healthBar;

        /// <summary>
        /// Called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);

            // 7 is the layer for "Player" objects
            if (other.gameObject.layer == 7)
            {
                UnitHealth unitHealth = other.GetComponent<UnitHealth>();
                if (unitHealth != null)
                {
                    // GameManager.Instance.PlayerHealth.TakeDamage(damage);
                    unitHealth.TakeDamage(damage);
                    healthBar.SetHealth(GameManager.Instance.playerHealth.Health);
                    AudioManager.Instance.PlaySound(2);
                    Debug.LogWarning("Projectile hit");
                    // Destroy(this.gameObject);
                }
            }
            // Layer 3 is the layer for "Environment" objects
            if (other.gameObject.layer == 3)
            {
                StartCoroutine(Bullet());
            }
        }

        /// <summary>
        /// Coroutine to destroy the bullet after a certain amount of time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Bullet()
        {
            // Edit this to add a sound effect to when the bullet hits the environment
            // AudioManager.Instance.PlaySound(27);
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }

    }
}
