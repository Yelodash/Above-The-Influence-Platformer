using System.Collections;
using Managers;
using UnityEngine;

namespace Melee_System
{
    /// <summary>
    /// Manages the melee system in the game.
    /// </summary>
    public class Melee : MonoBehaviour
    {
        [SerializeField] private GameObject leftHandObject;
        [SerializeField] private GameObject rightHandObject;
        
        private bool _isPunching;
        private Animator _animator;

        /// <summary>
        /// Gets the animator component.
        /// </summary>
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        /// <summary>
        /// Checks if the player can punch and if the gun is not enabled.
        /// </summary>
        private void Update()
        {
            // Can only use hands if they are enabled and the gun is not enabled
            if (!GameManager.Instance.areHandsEnabled)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !_isPunching)
            {
                _isPunching = true;
                StartCoroutine(Punch());
                StartCoroutine(ColliderEnabler());
            }
        }

        /// <summary>
        /// Coroutine that activates the punch animation.
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator Punch()
        {
            _animator.SetBool("IsPunching", true);
            // changed value to 0.5f from 1.5f
            yield return new WaitForSeconds(0.5f);
            _animator.SetBool("IsPunching", false);
            _isPunching = false;
            yield return null;
        }

        /// <summary>
        /// Enables the collider for the hands.
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator ColliderEnabler()
        {
            leftHandObject.SetActive(true);
            rightHandObject.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            leftHandObject.SetActive(false);
            rightHandObject.SetActive(false);
            yield return null;
        }
    }
}
