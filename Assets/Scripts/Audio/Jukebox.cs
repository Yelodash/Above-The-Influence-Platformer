using System.Collections;
using Levels;
using Powerups;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Manages the jukebox in the game.
    /// </summary>
    public class Jukebox : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private float minDistanceBeforePlaying = 10f;
        
        private Transform jukeboxTransform;
        private bool alcoholRanOut = true;
        
        /// <summary>
        ///Subscribes to the AlcoholObtained and AlcoholRanOut events.
        /// </summary>
        private void Start()
        {
            Alcohol.AlcoholObtained += AlcoholObtained;
            FilterChange.AlcoholRanOutEvent += AlcoholRanOut;
            jukeboxTransform = transform;
        }

        /// <summary>
        /// Unsubscribes from the AlcoholObtained and AlcoholRanOut events.
        /// </summary>
        private void OnDestroy()
        {
            Alcohol.AlcoholObtained -= AlcoholObtained;
            FilterChange.AlcoholRanOutEvent -= AlcoholRanOut;
        }
        
        /// <summary>
        /// Sets the alcoholRanOut boolean to false.
        /// </summary>
        private void AlcoholObtained()
        {
            alcoholRanOut = false;
        }
    
        /// <summary>
        /// Sets the alcoholRanOut boolean to true.
        /// </summary>
        private void AlcoholRanOut()
        {
            alcoholRanOut = true;
        }
    
        /// <summary>
        /// Updates the track depending on the distance between the player and the jukebox.
        /// </summary>
        private void Update()
        {
            float distance = Vector3.Distance(player.position, jukeboxTransform.position);
        
            if (distance > minDistanceBeforePlaying)
            {
                if(!AudioManager.Instance.soundtrackSources[0].isPlaying && AudioManager.Instance.soundtrackSources[1].isPlaying) return;
                if (!AudioManager.Instance.soundtrackSources[1].isPlaying && AudioManager.Instance.soundtrackSources[0].isPlaying) return;
            
                if (!AudioManager.Instance.soundtrackSources[0].isPlaying && alcoholRanOut)
                {
                    AudioManager.Instance.StopSound(3);
                    AudioManager.Instance.PlaySoundtrack(0);
                }
            
                else if (!AudioManager.Instance.soundtrackSources[1].isPlaying)
                {
                    AudioManager.Instance.StopSound(3);
                    AudioManager.Instance.PlaySoundtrack(1);
                }
                return;
            }
        
        
            if (distance <= minDistanceBeforePlaying)
            {
                AudioManager.Instance.PlaySoundAtPointLoop(3);
                AudioManager.Instance.StopSoundtrack(0);
            
                StartCoroutine(FadeOutTrack());
            
            }
        }

        /// <summary>
        /// Fades out the current track.
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator FadeOutTrack()
        {
            float fadeDuration = 3f;
            float startVolume = AudioManager.Instance.soundtrackSources[1].volume;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeDuration);
                AudioManager.Instance.soundtrackSources[1].volume = volume;
                yield return null;
            }

            AudioManager.Instance.StopSoundtrack(1);
            AudioManager.Instance.soundtrackSources[1].volume = startVolume;
        }
    }
}
