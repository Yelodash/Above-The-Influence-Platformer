using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Barrel_System
{
    
    /// <summary>
    /// Manages the spawning of barrels in the game.
    /// </summary>
    public class BarrelSpawner : MonoBehaviour
    {
        // Barrel stuff
        [SerializeField] private List<GameObject> spawnLocations;
        [SerializeField] private GameObject barrelPrefab;
        [SerializeField] private float maxCooldown;

        // Rotations
        [SerializeField] private float xRotation;
        [SerializeField] private float yRotation;
        [SerializeField] private float zRotation = 90;
    
        // Coroutine handler
        private bool _isExecuting;

        /// <summary>
        /// Handling of the coroutine for spawning barrels depending on bool state
        /// </summary>
        private void Update() 
        {
            if (!_isExecuting)
            {
                StartCoroutine(Cooldown());
            }
        }
    
        /// <summary>
        /// Handling of the cooldown for spawning barrels
        /// </summary>
        /// <returns>cooldown</returns>
        private IEnumerator Cooldown()
        {
            _isExecuting = true;
            SpawnBarrel();
            yield return new WaitForSeconds(maxCooldown);
            _isExecuting = false;
        }
    
        /// <summary>
        /// Handles the instantiation of barrels at the spawn locations with the given rotations
        /// </summary>
        private void SpawnBarrel()
        {
            foreach (var barrelSpawner in spawnLocations)
            {
                Instantiate(barrelPrefab, barrelSpawner.transform.position, Quaternion.Euler(xRotation, yRotation, zRotation));
            }
        }
    }
}
