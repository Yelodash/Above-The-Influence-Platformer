using HealthSystem;
using Managers;
using UnityEngine;

namespace Barrel_System


{
    /// <summary>
    ///The Barrel class is responsible for the behavior of the barrel object.
    /// </summary>
    public class Barrel : MonoBehaviour
    {
        [SerializeField] private float lifetime = 5f;
        [SerializeField] private int damage = 20;
        [SerializeField] private int initialSpeed = 10;
    
        /// <summary>
        ///The Rigidbody component of the barrel.
        /// </summary>
        private Rigidbody rb;
    
        /// <summary>
        /// Handles destroying the barrel after the specified lifetime.
        /// Gets the rigidbody component and applies force to the barrel.
        /// </summary>
        private void Start()
        {
            Destroy(gameObject, lifetime);
        
            rb = GetComponent<Rigidbody>();
        
            // Check if the Rigidbody component exists and apply force
            if (rb != null)
            {
                rb.AddForce(transform.forward * initialSpeed, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Handles the collision of the barrel with other objects.
        /// </summary>
        /// <param name="other">The object collided with</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                UnitHealth unitHealth = other.GetComponent<UnitHealth>();
                unitHealth.TakeDamage(damage);
                HealthBarScript.Instance.SetHealth(GameManager.Instance.playerHealth.Health);
                Destroy(gameObject);
            }
        }
    }
}
