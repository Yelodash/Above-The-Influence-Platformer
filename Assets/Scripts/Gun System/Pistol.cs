using System.Collections;
using Audio;
using HealthSystem;
using Managers;
using Powerups;
using TMPro;
using UnityEngine;

namespace Gun_System
{
    /// <summary>
    /// Pistol class that inherits from the GunBaseClass.
    /// </summary>
    public class Pistol : GunBaseClass
    {
        [SerializeField] private int maxAmmo = 30;
        [SerializeField] private float fireRate = 0.1f;
        [SerializeField] private float range = 100f;
        private int damage;
        [SerializeField] private LayerMask wantedLayer;
        [SerializeField] private TextMeshProUGUI ammoCounter;
        [SerializeField] private new ParticleSystem particleSystem;
    
        [SerializeField] private GameObject gunPistolObject;

        public FriendlyFireCheck friendlyFireCheck;

    
        /// <summary>
        /// Subscribes to the events and initialises the gun.   
        /// </summary>
        private void Start()
        {
            PistolPowerup.FixupAmmo += ResetAmmoToDefault;
            GameManager.OnPistolEnabled += IsPistolEnabled;
            InitialiseGun(maxAmmo, fireRate, range, damage, maxAmmo, wantedLayer.value, ammoCounter);
        }
        
        /// <summary>
        /// Unsubscribes from the events.
        /// </summary>
        private void OnDestroy()
        {
            PistolPowerup.FixupAmmo -= ResetAmmoToDefault;
            GameManager.OnPistolEnabled -= IsPistolEnabled;
        }

        /// <summary>
        /// Resets the ammo to the default value.
        /// </summary>
        private void ResetAmmoToDefault()
        {
            CurrentAmmo = MaxAmmo;
        }
    
        /// <summary>
        /// Sets the damage to the current attack power of the player.
        /// Displays the ammo count.
        /// </summary>
        private void Update()
        {
            damage = AttributeManager.Instance.attackPowerGun;
            DisplayAmmo();
        }

        /// <summary>
        /// Handles the logic for when the pistol is enabled and disabled.
        /// </summary>
        /// <param name="isPistolEnabled">bool that represents if the tommy gun is enabled or not</param>
        private void IsPistolEnabled(bool isPistolEnabled)
        {
            if (isPistolEnabled)
            {
                gunPistolObject.SetActive(true);
                ammoCounter.gameObject.SetActive(true);
                CheckIfPlayerWantsToFireOrReload();
            }
            else
            {
                gunPistolObject.SetActive(false);
                ammoCounter.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Checks if the player wants to fire the gun.
        /// </summary>
        private void CheckIfPlayerWantsToFireOrReload()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (friendlyFireCheck.canFire)
                {
                    Shoot(); 
                    particleSystem.Play();
                    AudioManager.Instance.PlaySound(9);
                    StartCoroutine(StopParticleSystem());
                }
            }
        
            if (Input.GetKeyDown(KeyCode.R))
            {
                //Not all guns will have the ability to reload
                //Just here in case we need it
                Reload();
            }
        }
    
        /// <summary>
        /// Stops the particle system after a set amount of time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator StopParticleSystem()
        {
            yield return new WaitForSeconds(1);
            particleSystem.Stop();
        }
        
        /// <summary>
        /// Method that overrides the base for bullet, handles the raycast logic and dealing of damage
        /// </summary>
        protected override void Bullet()
        {

            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, range))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.layer == 8)
                {
                    UnitHealth unitHealth = hit.collider.gameObject.GetComponent<UnitHealth>();
                    if (unitHealth != null)
                    {
                        unitHealth.TakeDamage(damage);
                        Debug.Log("Took Health away from " + hit.collider.name);
                    }
                }
            }
        }
    
        /// <summary>
        /// Reloads the gun.
        /// </summary>
        protected override void Reload()
        {
            base.Reload();
            // Animation logic that is unique to current guns reload if necessary 
            Debug.Log("Reload called");
        }
    }
}
