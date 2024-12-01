using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Manages the environmental sounds in the game. 
    /// </summary>
    public class EnvironmentalSounds : MonoBehaviour
    {
        [SerializeField] private int soundNum;
        [SerializeField] private bool isLooping;
        
        /// <summary>
        /// Handles the playing of a sound depending on the bool state.
        /// </summary>
        void Start()
        {
            if (!isLooping)
            {
                var position = transform.position;
                AudioManager.Instance.PlaySoundAtPoint(soundNum, position); 
            }

            if (isLooping)
            {
                AudioManager.Instance.PlaySoundAtPointLoop(soundNum);
            }
        }
    }
}
