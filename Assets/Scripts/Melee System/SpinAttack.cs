using System.Collections;
using Audio;
using Managers;
using PlayerControls;
using UnityEngine;

namespace Melee_System
{
    /// <summary>
    /// Manages the spin attack mechanic in the game.
    /// </summary>
    public class SpinAttack : MonoBehaviour
    {
        public static event OnSpinAttack SpinAttackEvent;
        

        [SerializeField] private float rotationDuration = 0.3f;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private GameObject spinAttackCollider;
        
        private bool isRotating = false;
        
        private Animator _animator;
        private CharacterController _characterController;
        
        /// <summary>
        /// Get the character controller and animator components.
        /// Make sure the spin attack collider is disabled.
        /// </summary>
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            spinAttackCollider.SetActive(false);
            _animator = GetComponentInChildren<Animator>();
        }
    
        /// <summary>
        /// Checks if the player can spin attack and if the player is grounded.
        /// </summary>
        private void Update()
        {
            // Cannot spin attack while the gun is enabled
            if (!GameManager.Instance.areHandsEnabled)
            {
                return;
            }
        
            // F pressed, not rotating, can move, and is grounded
            if (Input.GetKeyDown(KeyCode.F) && !isRotating && playerController.canMove && _characterController.isGrounded)
            {
                // return if already rotating
                if (isRotating)
                {
                    return;
                }
                SpinAttackEvent?.Invoke(true);
            
                if (!isRotating)
                {
                    StartCoroutine(ColliderCoroutine());
                    StartCoroutine(SpinAttackCoroutine());
                }
            }
        }

        /// <summary>
        /// Activates the collider for the spin attack.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ColliderCoroutine()
        {
            spinAttackCollider.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            spinAttackCollider.SetActive(false);
        }
    
        /// <summary>
        /// Spins the player by activating the animation
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator SpinAttackCoroutine()
        {
            AudioManager.Instance.PlaySound(26);
            isRotating = true;
            float elapsedTime = 0f;
            _animator.SetBool("IsSpinning", true);
        
            // timer
            while (elapsedTime < rotationDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            isRotating = false;
            AudioManager.Instance.StopSound(26);
            _animator.SetBool("IsSpinning", false);
            SpinAttackEvent?.Invoke(false);
        }
    }
}
