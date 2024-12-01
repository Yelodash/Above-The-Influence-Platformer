using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Play an audio clip when the player picks up an item.
    /// </summary>
    public class PlayAudioLogOnPickup : MonoBehaviour
    {
        [SerializeField] private int soundToBePlayed;
        
        /// <summary>
        /// When the player collides play the sound
        /// </summary>
        /// <param name="other">The game object collided with</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                AudioManager.Instance.PlaySound(soundToBePlayed);
                Debug.Log("Player picked up Log. Playing sound...");
                Destroy(gameObject);
            }
        }
    }
}
