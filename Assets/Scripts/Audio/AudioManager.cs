using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Audio

{
    /// <summary>
    /// Manages the audio sources in the game.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [SerializeField] private List<AudioSource> audioSources;
        [SerializeField] public List<AudioSource> soundtrackSources;
        [SerializeField] public float globalVolume = 0.5f;
        [SerializeField] public float soundtrackVolume = 0.4f;
        
        private Dictionary<AudioSource, float> originalVolumes = new();
        private bool isChangingVolume;
        private Coroutine currentCoroutine;
        private bool enemyVoicelinePlaying;


        /// <summary>
        /// Ensure that there is only one instance of the AudioManager.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
         
        /// <summary>
        /// If the scene is the main menu, play the soundtrack.
        /// </summary>
        private void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                PlaySoundtrack(1);
            }
        }

        /// <summary>
        /// Plays the sound at the specified index.
        /// </summary>
        /// <param name="i">Sound num</param>
        public void PlaySound(int i)
        {
            if (i >= audioSources.Count)
            {
                return;
            }

            AudioSource audioSource = audioSources[i];

            // If not playing or special case, adjust volume and play
            if (!audioSource.isPlaying || IsSpecialCase(i))
            {
                // Adjust volume if not already adjusting
                if (!isChangingVolume)
                {
                    isChangingVolume = true;
                    AdjustVolume(audioSource);
                }

                audioSource.Play();
            }
        }

        /// <summary>
        ///Plays the soundtrack at the specified index.
        /// </summary>
        /// <param name="i">Soundtrack num</param>
        public void PlaySoundtrack(int i)
        {
            if (i >= soundtrackSources.Count)
            {
                return;
            }

            AudioSource soundtrackSource = soundtrackSources[i];

            if (!soundtrackSource.isPlaying)
            {
                AdjustVolume(soundtrackSource);
                soundtrackSource.Play();
            }
        }

        /// <summary>
        /// Plays the sound at the specified index at the specified position.
        /// </summary>
        /// <param name="i">Sound num</param>
        /// <param name="position">World position</param>
        public void PlaySoundAtPoint(int i, Vector3 position)
        {
            if (i >= audioSources.Count || i < 0)
            {
                return;
            }

            AudioSource audioSource = audioSources[i];
            AdjustVolume(audioSource);

            // Calculate the volume for the one-shot audio
            float volumeMultiplier = originalVolumes[audioSource] * globalVolume;

            // Play the one-shot audio with the calculated volume
            AudioSource.PlayClipAtPoint(audioSource.clip, position, volumeMultiplier);
        }

        
        /// <summary>
        /// Plays the looping sound at the specified index at the specified position.
        /// </summary>
        /// <param name="i">Sound num</param>
        public void PlaySoundAtPointLoop(int i)
        {
            if (i >= audioSources.Count)
            {
                return;
            }

            AudioSource audioSource = audioSources[i];
            
            AdjustVolume(audioSource);

            // Play the looped sound
            if (!audioSource.isPlaying) 
            {
                audioSource.Play();
            }
        }
        
        /// <summary>
        /// Stops the sound at the specified index.
        /// </summary>
        /// <param name="i">Sound num</param>
        public void StopSound(int i)
        {
            if (i <= audioSources.Count && audioSources[i].isPlaying)
            {
                audioSources[i].Stop();
            }
        }

        /// <summary>
        /// Stops the soundtrack at the specified index.
        /// </summary>
        /// <param name="i">Sound num</param>
        public void StopSoundtrack(int i)
        {
            if (i <= soundtrackSources.Count && soundtrackSources[i].isPlaying)
            {
                soundtrackSources[i].Stop();
            }
        }
        /// <summary>
        /// plays a one-shot sound at the specified index at the specified position.
        /// Add a coroutine to play the sound after the delay.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="position"></param>
        public void PlayOneShot(int i, Vector3 position)
        {
            if (i < 0 || i >= audioSources.Count)
            {
                return;
            }

            if (!enemyVoicelinePlaying)
            {
                AudioSource audioSource = audioSources[i];
                AdjustVolume(audioSource);
                
                float clipLength = audioSources[i].clip.length;
                StartCoroutine(PlayDelayedOneShot(i, position, clipLength));
            }
        }
        
        /// <summary>
        /// Plays a delayed one-shot sound at the specified index at the specified position.
        /// </summary>
        /// <param name="i">Sound to be played</param>
        /// <param name="position">World position</param>
        /// <param name="delay">Delay in seconds</param>
        /// <returns></returns>
        private IEnumerator PlayDelayedOneShot(int i, Vector3 position, float delay)
        {
            enemyVoicelinePlaying = true;
            
            AudioSource audioSource = audioSources[i];
            float volumeMultiplier = originalVolumes[audioSource] * globalVolume;
            AudioSource.PlayClipAtPoint(audioSource.clip, position, volumeMultiplier);
            
            yield return new WaitForSeconds(delay);
            
            enemyVoicelinePlaying = false;
        }
        
        /// <summary>
        /// Adjusts the volume of the audio source based on whether it's a soundtrack source or not.
        /// </summary>
        /// <param name="source">AudioSource GameObject</param>
        private void AdjustVolume(AudioSource source)
        {
            if (!originalVolumes.ContainsKey(source))
            {
                // Store original volume if not already stored
                originalVolumes[source] = source.volume;
            }

            float volumeMultiplier = 1.0f;
    
            // Apply global volume multiplier based on whether it's a soundtrack source or not
            if (soundtrackSources.Contains(source))
            {
                volumeMultiplier = soundtrackVolume;
            }
            else
            {
                volumeMultiplier = globalVolume;
            }

            // Apply global volume
            source.volume = originalVolumes[source] * volumeMultiplier;
            isChangingVolume = false;
        }
        
        /// <summary>
        /// Checks if the audio source at the specified index is a special case.
        /// </summary>
        /// <param name="index">Sound num</param>
        /// <returns></returns>
        private bool IsSpecialCase(int index)
        {
            return index >= 5 && index <= 7 || index == 9 || index >= 23 && index <= 25;
        }
    }
}
