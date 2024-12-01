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
    /// Tommy Gun class that inherits from the GunBaseClass.
    /// </summary>
    public class TommyGun : GunBaseClass
    {
        [SerializeField] private int maxAmmo = 33;
        [SerializeField] private float fireRate = 10f;
        [SerializeField] private float range = 100f;
        private int damage;
        [SerializeField] private LayerMask wantedLayer;
        [SerializeField] private TextMeshProUGUI ammoCounter;
        [SerializeField] private new ParticleSystem particleSystem;
        private bool isFiring = false;
        [SerializeField] private GameObject tommyGunObject;
    
    
        /// <summary>
        /// Subscribes to the relevant event
        /// Initialises the gun
        /// </summary>
        private void Start()
        {
            GameManager.OnTommyGunEnabled += IsTommyGunEnabled;
            InitialiseGun(maxAmmo, fireRate, range, damage, maxAmmo, wantedLayer.value, ammoCounter);
        }
   
        /// <summary>
        /// Unsubscribe from the relevant events
        /// </summary>
        private void OnDestroy()
        {
            GameManager.OnTommyGunEnabled -= IsTommyGunEnabled;
        }
   
        /// <summary>
        /// Check if the tommy gun is enabled and handle logic for whether it is or isn't
        /// </summary>
        /// <param name="isTommyGunEnabled">bool that represents if the tommy gun is enabled or not</param>
        private void IsTommyGunEnabled(bool isTommyGunEnabled)
        {
            if (isTommyGunEnabled)
            {
                tommyGunObject.SetActive(true);
                ammoCounter.gameObject.SetActive(true);
                CheckIfPlayerWantsToFire();
            }
            else
            {
                tommyGunObject.SetActive(false);
                ammoCounter.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Checks if the player wants to fire the gun.
        /// </summary>
        private void CheckIfPlayerWantsToFire()
        {
            if (Input.GetMouseButtonDown(0) && CurrentAmmo > 0)
            {
                isFiring = true;
                StartCoroutine(AutomaticFire());
            }
            else if (Input.GetMouseButtonUp(0) || CurrentAmmo <= 0)
            {
                isFiring = false;
            }
        }

        /// <summary>
        /// Set the damage to the value in the AttributeManager
        /// Call the DisplayAmmo method
        /// </summary>
        private void Update()
        {
            damage = AttributeManager.Instance.attackPowerGun;
            DisplayAmmo();
        }
    
        /// <summary>
        /// Coroutine that handles the automatic fire of the gun.
        /// </summary>
        /// <returns></returns>
        IEnumerator AutomaticFire()
        {
            while (isFiring)
            {
                Shoot();
                AudioManager.Instance.PlaySound(9);
                particleSystem.Play();
                StartCoroutine(StopParticleSystem());
                yield return new WaitForSeconds(1f / fireRate);
            }
        }
    
        /// <summary>
        /// Stops the particle system after a certain amount of time.
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
            // Create a ray from the centre point of the screen
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
    }
}
