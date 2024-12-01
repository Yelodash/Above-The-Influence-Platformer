using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Interface_Sliders
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private Slider sfxVolume;
        [SerializeField] private Slider soundtrackVolume;

        private const string SfxVolumePrefsKey = "SFXVolume";
        private const string SoundtrackVolumePrefsKey = "SoundtrackVolume";

        /// <summary>
        /// Loads the volume values from PlayerPrefs and sets the initial values of the sliders
        /// </summary>
        private void Start()
        {
            // Load saved SFX volume or set default value
            float savedVolume = PlayerPrefs.GetFloat(SfxVolumePrefsKey); 
            sfxVolume.value = savedVolume;
            AudioManager.Instance.globalVolume = savedVolume;
            
            // Load saved Soundtrack volume or set default value
            float savedSoundtrackVolume = PlayerPrefs.GetFloat(SoundtrackVolumePrefsKey); 
            soundtrackVolume.value = savedSoundtrackVolume;
            AudioManager.Instance.soundtrackVolume = savedSoundtrackVolume; 
            
            // Add listener to slider value change
            sfxVolume.onValueChanged.AddListener(ChangeSfxVolume);
            soundtrackVolume.onValueChanged.AddListener(ChangeSoundtrackVolume);
        }

        /// <summary>
        /// Removes the listener when the object is destroyed
        /// </summary>
        private void OnDestroy()
        {
            // Remove listener to avoid memory leaks
            sfxVolume.onValueChanged.RemoveListener(ChangeSfxVolume);
            soundtrackVolume.onValueChanged.RemoveListener(ChangeSoundtrackVolume);
        }

        private void ChangeSfxVolume(float newVolume)
        {
            // Clamp volume value between 0.0f and 1.0f for soundtrack volume
            newVolume = Mathf.Clamp(newVolume, 0.1f, 1.0f);

            // Update AudioManager's soundtrack volume
            AudioManager.Instance.globalVolume = newVolume;

            // Save volume value to PlayerPrefs for persistence
            PlayerPrefs.SetFloat(SfxVolumePrefsKey, newVolume);
            PlayerPrefs.Save(); // Save PlayerPrefs immediately
        }
        
        private void ChangeSoundtrackVolume(float newVolume)
        {
            // Clamp volume value between 0.1f and 1.0f for soundtrack volume
            newVolume = Mathf.Clamp(newVolume, 0.01f, 0.7f);

            // Update AudioManager's soundtrack volume
            AudioManager.Instance.soundtrackVolume = newVolume;

            // Update the volume of all soundtrack sources
            foreach (var soundtrackSource in AudioManager.Instance.soundtrackSources)
            {
                if (soundtrackSource.isPlaying)
                {
                    soundtrackSource.volume = newVolume;
                }
            }

            // Save volume value to PlayerPrefs for persistence
            PlayerPrefs.SetFloat(SoundtrackVolumePrefsKey, newVolume);
            PlayerPrefs.Save(); // Save PlayerPrefs immediately
        }

    }
}