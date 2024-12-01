using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The AttributeManager class is a script that is used to manage the player's attributes.
    /// </summary>
    public class AttributeManager : MonoBehaviour
    {
        public static AttributeManager Instance;
    
        [Header("Health")]
        public float maxHealth = 100f;
        public float currentHealth;
        private float previousMaxHealth = 100f;
    
        [Header("Attack")]
        public float attackPower = 1f;
        private float previousAttackPower = 1f;
        public float attackPowerUpgraded = 1f;
    
    
        public int attackPowerGun = 40;
        public float attackPowerMelee = 10f;
        public float attackPowerSpin = 15f;
    
        [Header("Defence")]
        public float defenceMultiplier = 1.1f;
        public float defence = 2f;
    
        [Header("Movement")]
        public float walkSpeed = 4f;
        public float sprintSpeed = 7f;
        public float crouchWalkSpeed = 1.5f;
        public float crouchJumpForce = 8f;
        public float jumpForce = 6f;
    
        /// <summary>
        /// Ensures that there is only one instance of the AttributeManager.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Sets the default values for the player's attributes.
        /// </summary>
        private void Start()
        {
            // Default values
            attackPowerGun = 40;
            attackPowerMelee = 10f;
            attackPowerSpin = 15f;
            previousAttackPower = attackPower;
            currentHealth = maxHealth;
        }
    
        /// <summary>
        /// Call the UpdateAttackPower method.
        /// </summary>
        private void Update()
        {
            UpdateAttackPower();
        }

        /// <summary>
        /// Updates the attack power values.
        /// </summary>
        private void UpdateAttackPower()
        {
            if (attackPower == previousAttackPower)
            {
                return;
            }
        
            attackPowerGun *= (int)attackPower;
            attackPowerMelee *= attackPower;
            attackPowerSpin *= attackPower;
            previousAttackPower = attackPower;
        
            // Set to the new Attack Power - For the shop
            attackPower = attackPowerUpgraded;
        }

        /// <summary>
        /// Returns the attack power values to their default values.
        /// </summary>
        public void ReturnAttackPowerValuesToDefault()
        {
            attackPowerGun /= (int)attackPower;
            attackPowerMelee /= attackPower;
            attackPowerSpin /= attackPower;
        }
    }
}
