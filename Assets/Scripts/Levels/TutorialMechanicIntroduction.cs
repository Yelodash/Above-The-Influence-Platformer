using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Manages the tutorial mechanic introduction in the game.
    /// </summary>
    public class TutorialMechanicIntroduction : MonoBehaviour
    {
        [SerializeField] private CharacterController player;
        [SerializeField] private GameObject uiElement;

        /// <summary>
        /// If the player collides with the trigger, the UI element is activated and the player is disabled.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                uiElement.SetActive(true);
                player.enabled = false;
            }
        }

        /// <summary>
        /// While the UI element is active, the player can press the return key to continue.
        /// </summary>
        private void Update()
        {
            if(!uiElement.activeSelf) return;
        
            if (Input.GetKeyDown(KeyCode.Return))
            {
                player.enabled = true;
                uiElement.SetActive(false);
                Destroy(gameObject);
            }
        }
    }
}
