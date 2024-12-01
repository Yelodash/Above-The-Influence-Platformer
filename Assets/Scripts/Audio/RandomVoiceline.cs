using System.Collections;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Plays a random voiceline every x amount of seconds
    /// </summary>
    public class RandomVoiceline : MonoBehaviour
    {
        [SerializeField] private int waitTime = 20;
        private bool isPlaying = false;

        /// <summary>
        /// Checks if the voiceline is playing, if not, play a random voiceline
        /// </summary>
        private void Update()
        {
            if (!isPlaying)
            {
                StartCoroutine(PlayVoiceline(Random.Range(35, 38)));
            }
        }
        
        /// <summary>
        /// Handles the playing of the voiceline
        /// </summary>
        /// <param name="audioClip">The clip to be played</param>
        /// <returns></returns>
        private IEnumerator PlayVoiceline(int audioClip)
        {
            isPlaying = true;
            AudioManager.Instance.PlaySoundAtPoint(audioClip, gameObject.transform.position);
            
            yield return new WaitForSeconds(waitTime);
            isPlaying = false;
        }
    }
}
