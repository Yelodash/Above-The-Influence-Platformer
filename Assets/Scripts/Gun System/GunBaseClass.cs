using TMPro;
using UnityEngine;

namespace Gun_System
{
    /// <summary>
    /// Base class for all guns in the game.
    /// </summary>
    public class GunBaseClass : MonoBehaviour
    {
        public int CurrentAmmo { get; protected set; }
        public int MaxAmmo { get; protected set; }
        public float FireRate { get; protected set; }
        public float Range { get; protected set; }
        public int Damage { get; protected set; }
        public LayerMask WantedLayer { get; protected set; }
        public TextMeshProUGUI AmmoCounter { get; protected set; }
    
        /// <summary>
        /// Initializes the gun with the given parameters.
        /// </summary>
        /// <param name="maxAmmo">the maximum ammo count</param>
        /// <param name="fireRate">rate of fire of the gun</param>
        /// <param name="range">range of the gun</param>
        /// <param name="damage">damage value of the gun</param>
        /// <param name="currentAmmo">the current ammo count of the gun</param>
        /// <param name="wantedLayer">the layer that damange will be dealt to</param>
        /// <param name="ammoCounter">the UI counter for the ammo</param>
        public void InitialiseGun(int maxAmmo, float fireRate, float range, int damage, int currentAmmo, LayerMask wantedLayer, TextMeshProUGUI ammoCounter)
        {
            MaxAmmo = maxAmmo;
            CurrentAmmo = currentAmmo;
            FireRate = fireRate;
            Range = range;
            Damage = damage;
            WantedLayer = wantedLayer;
            AmmoCounter = ammoCounter;
        }

        /// <summary>
        /// Shoots the gun if there is ammo left.
        /// </summary>
        protected void Shoot()
        {
            Debug.Log("Base was called");
            if (CurrentAmmo > 0)
            {
                CurrentAmmo--;
                Bullet();
                Debug.Log("Ammo was supposedly taken away");
            }
        }

        /// <summary>
        /// Method for overriding the bullet method.
        /// </summary>
        protected virtual void Bullet()
        {
            
        }

        /// <summary>
        /// Method for overriding the reload method.
        /// </summary>
        protected virtual void Reload()
        {
            // Placeholder that the different guns we make if we do have multiple will override
            CurrentAmmo = MaxAmmo;
        }

        /// <summary>
        ///  Displays the ammo count on the screen.
        /// </summary>
        protected void DisplayAmmo()
        {
            AmmoCounter.text = CurrentAmmo + "/" + MaxAmmo;
        }
    }
}