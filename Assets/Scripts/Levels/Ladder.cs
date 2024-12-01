using Audio;
using Managers;
using UnityEngine;

namespace Levels
{
    /// <summary>
    /// This script is used to make the player climb a ladder.
    /// </summary>
    public class Ladder : MonoBehaviour
    {
        public static event OnLadderClimb LadderClimbEvent;
    
        [SerializeField] private float climbSpeed = 10.5f;
        private Animator _animator;
    
        /// <summary>
        /// Gets the animator component.
        /// </summary>
        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        /// <summary>
        /// Called when the player stays on the ladder.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == 10 && Input.GetKey(KeyCode.W))
            {
                _animator.SetBool("IsOnLadder", true);
            
                LadderClimbEvent?.Invoke(climbSpeed);

                if (other.CompareTag("WoodenLadder"))
                {
                    AudioManager.Instance.PlaySound(21);
                }
                else if (other.CompareTag("MetalLadder"))
                {
                    AudioManager.Instance.PlaySound(22);
                }
                else
                {
                    Debug.Log("Ladder has no tag and no sound was played as a result!");
                }
            }
        }

        /// <summary>
        /// Called when the player exits the ladder.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == 10)
            {
            
                _animator.SetBool("IsOnLadder", false);
            
                AudioManager.Instance.StopSound(21);
                AudioManager.Instance.StopSound(22);
            }
        }
    }
}