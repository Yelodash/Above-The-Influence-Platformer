using System.Collections;
using System.Collections.Generic;
using Gun_System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Powerups
{
    /// <summary>
    /// This script manages the powerups in the game.
    /// </summary>
    public class PowerupManager : MonoBehaviour
    {
        public static PowerupManager Instance;
        [SerializeField] private Pistol pistol;
        [SerializeField] private TommyGun tommyGun;

        // For all the UI Timers
        [SerializeField] private List<Image> fillImageElements;

        [Header("Powerup Durations - In Seconds")]
        [SerializeField] private int attackPowerupDuration;
        [SerializeField] private int instakillPowerupDuration;
        [SerializeField] private int healthPowerupDuration;
        [SerializeField] private int defencePowerupDuration;
        [SerializeField] private int speedPowerupDuration;
        [SerializeField] private int jumpPowerupDuration;
    
        [Header("Powerup Multipliers")]
        public float healthPowerupMultiplier = 2f;
        public float defencePowerupMultiplier = 4f;
        public float speedPowerupMultiplier = 1.3f;
        public float jumpPowerupMultiplier = 1.3f;

        /// <summary>
        /// Ensures that only one instance of the PowerupManager exists.
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
        /// Subscribes to all the relevant events
        /// </summary>
        private void Start()
        {
            // Subscribe to the events
            AttackPlusPowerup.AttackPowerupObtained += AttackPowerupHandler;
            InstakillPowerup.InstakillPowerupObtained += InstakillPowerupHandler;
            PistolPowerup.PistolPowerupObtained += PistolPowerupHandler;
            GameManager.OnTommyGunPowerup += TommyGunPowerupHandler;
            HealthPowerup.HealthPowerupObtained += HealthPowerupHandler;
            DefencePowerup.DefencePowerupObtained += DefencePowerupHandler;
            SpeedPowerup.SpeedPowerupObtained += SpeedPowerupHandler;
            JumpPowerup.JumpForcePowerupObtained += JumpPowerupHandler;
        }
        
        /// <summary>
        /// Unsubscribe from all the relevant events
        /// </summary>
        private void OnDestroy()
        {
            // Unsubscribe to avoid memory leaks
            AttackPlusPowerup.AttackPowerupObtained -= AttackPowerupHandler;
            InstakillPowerup.InstakillPowerupObtained -= InstakillPowerupHandler;
            PistolPowerup.PistolPowerupObtained -= PistolPowerupHandler;
            GameManager.OnTommyGunPowerup -= TommyGunPowerupHandler;
            HealthPowerup.HealthPowerupObtained -= HealthPowerupHandler;
            DefencePowerup.DefencePowerupObtained -= DefencePowerupHandler;
            SpeedPowerup.SpeedPowerupObtained -= SpeedPowerupHandler;
            JumpPowerup.JumpForcePowerupObtained -= JumpPowerupHandler;
        }
        
        /// <summary>
        /// Handles the Attack powerup
        /// </summary>
        private void AttackPowerupHandler()
        {
            fillImageElements[0].gameObject.SetActive(true);
            fillImageElements[0].transform.parent.gameObject.SetActive(true);
            StartCoroutine(AttackPowerupTimer());
        }
    
        /// <summary>
        /// Handles the Instakill powerup
        /// </summary>
        private void InstakillPowerupHandler()
        {
            fillImageElements[1].gameObject.SetActive(true);
            fillImageElements[1].transform.parent.gameObject.SetActive(true);
            StartCoroutine(InstakillPowerupTimer());
        }
    
        /// <summary>
        /// Handles the Pistol powerup
        /// </summary>
        private void PistolPowerupHandler()
        {
            GameManager.Instance.isPistolEnabled = true;
            StartCoroutine(GunPowerupHandle());
        }
    
        /// <summary>
        /// Handles the Tommy Gun powerup
        /// </summary>
        private void TommyGunPowerupHandler()
        {
            GameManager.Instance.isTommyGunEnabled = true;
            StartCoroutine(TommyGunPowerupHandle());
        }
    
        /// <summary>
        /// Handles the Health powerup
        /// </summary>
        private void HealthPowerupHandler()
        {
            fillImageElements[2].gameObject.SetActive(true);
            fillImageElements[2].transform.parent.gameObject.SetActive(true);
            StartCoroutine(HealthPowerupTimer());
        }
    
        /// <summary>
        /// Handles the Defence powerup
        /// </summary>
        private void DefencePowerupHandler()
        {
            fillImageElements[3].gameObject.SetActive(true);
            fillImageElements[3].transform.parent.gameObject.SetActive(true);
            StartCoroutine(DefencePowerupTimer());
        }
    
        /// <summary>
        /// Handles the Speed powerup
        /// </summary>
        private void SpeedPowerupHandler()
        {
            fillImageElements[4].gameObject.SetActive(true);
            fillImageElements[4].transform.parent.gameObject.SetActive(true);
            StartCoroutine(SpeedPowerupTimer());
        }
    
        /// <summary>
        /// Handles the Jump powerup
        /// </summary>
        private void JumpPowerupHandler()
        {
            fillImageElements[5].gameObject.SetActive(true);
            fillImageElements[5].transform.parent.gameObject.SetActive(true);
            StartCoroutine(JumpPowerupTimer());
        }

        /// <summary>
        /// Handles the Attack powerup
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator AttackPowerupTimer()
        {
            float elapsedTime = 0f;

            while (elapsedTime < attackPowerupDuration)
            {
                fillImageElements[0].fillAmount = elapsedTime / attackPowerupDuration;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Attack Powerup timer ended!");
            AttributeManager.Instance.ReturnAttackPowerValuesToDefault();
            AttributeManager.Instance.attackPower = AttributeManager.Instance.attackPowerUpgraded;
        
            fillImageElements[0].gameObject.SetActive(false);
            fillImageElements[0].transform.parent.gameObject.SetActive(false);
        }
    
        /// <summary>
        /// Handles the Instakill powerup
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator InstakillPowerupTimer()
        {
            float elapsedTime = 0f;

            while (elapsedTime < instakillPowerupDuration)
            {
                fillImageElements[1].fillAmount = elapsedTime / instakillPowerupDuration;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Instakill Powerup timer ended!");
            AttributeManager.Instance.ReturnAttackPowerValuesToDefault();
            AttributeManager.Instance.attackPower = AttributeManager.Instance.attackPowerUpgraded;
        
            fillImageElements[1].gameObject.SetActive(false);
            fillImageElements[1].transform.parent.gameObject.SetActive(false);
        }
    
        /// <summary>
        /// Handles the Health powerup
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator HealthPowerupTimer()
        {
            // Timer
            float elapsedTime = 0f;

            while (elapsedTime < healthPowerupDuration)
            {
                fillImageElements[2].fillAmount = elapsedTime / healthPowerupDuration;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Health Powerup timer ended!");
            AttributeManager.Instance.maxHealth /= healthPowerupMultiplier;
            
            fillImageElements[2].gameObject.SetActive(false);
            fillImageElements[2].transform.parent.gameObject.SetActive(false);
        
            if (AttributeManager.Instance.currentHealth > AttributeManager.Instance.maxHealth)
            {
                AttributeManager.Instance.currentHealth = AttributeManager.Instance.maxHealth;
            }
        }
    
        /// <summary>
        /// Handles the Defence powerup
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator DefencePowerupTimer()
        {
            float elapsedTime = 0f;

            while (elapsedTime < defencePowerupDuration)
            {
                fillImageElements[3].fillAmount = elapsedTime / defencePowerupDuration;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            Debug.Log("Defence Powerup timer ended!");
        
            // Value should correspond to that in the DefencePowerup script
            AttributeManager.Instance.defenceMultiplier /= defencePowerupMultiplier;
        
            fillImageElements[3].gameObject.SetActive(false);
            fillImageElements[3].transform.parent.gameObject.SetActive(false);
        }
    
        /// <summary>
        /// Handles the Speed powerup.
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator SpeedPowerupTimer()
        {
            float elapsedTime = 0f;

            while (elapsedTime < speedPowerupDuration)
            {
                fillImageElements[4].fillAmount = elapsedTime / speedPowerupDuration;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            Debug.Log("Speed Powerup timer ended!");
        
            AttributeManager.Instance.walkSpeed /= speedPowerupMultiplier;
            AttributeManager.Instance.sprintSpeed /= speedPowerupMultiplier;
            AttributeManager.Instance.crouchWalkSpeed /= speedPowerupMultiplier;
        
            fillImageElements[4].gameObject.SetActive(false);
            fillImageElements[4].transform.parent.gameObject.SetActive(false);
        
        }
    
        /// <summary>
        /// Handles the Jump powerup.
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator JumpPowerupTimer()
        {
            float elapsedTime = 0f;

            while (elapsedTime < jumpPowerupDuration)
            {
                fillImageElements[5].fillAmount = elapsedTime / jumpPowerupDuration;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            Debug.Log("Jump Powerup timer ended!");
        
            AttributeManager.Instance.jumpForce /= jumpPowerupMultiplier;
            AttributeManager.Instance.crouchJumpForce /= jumpPowerupMultiplier;
        
            fillImageElements[5].gameObject.SetActive(false);
            fillImageElements[5].transform.parent.gameObject.SetActive(false);
        }
    
        /// <summary>
        /// Handles the Pistol powerup
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator GunPowerupHandle()
        {
            while (pistol.CurrentAmmo > 0)
            {
                yield return null;
            }
        
            GameManager.Instance.isPistolEnabled = false;
        }
        
        /// <summary>
        /// Handles the Tommy Gun powerup
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator TommyGunPowerupHandle()
        {
            while (tommyGun.CurrentAmmo > 0)
            {
                yield return null;
            }
        
            GameManager.Instance.isTommyGunEnabled = false;
        }
    }
}
