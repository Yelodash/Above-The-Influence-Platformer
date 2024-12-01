using System.Collections;
using Audio;
using Managers;
using Powerups;
using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Manages the filter change in the game.
    /// </summary>
    public class FilterChange : MonoBehaviour
    {
        public static event AlcoholRanOut AlcoholRanOutEvent;
    
        [SerializeField] private GameObject filter;
        [SerializeField] private float alcoholFilterDuration = 60f;
    
    
        /// <summary>
        /// Subscribe to the relevant events.
        /// Play the dark filter soundtrack.
        /// </summary>
        void Start()
        {
            Alcohol.AlcoholObtained += ChangeFilter;
            AudioManager.Instance.PlaySoundtrack(0);
        }

        /// <summary>
        /// Unsubscribe from the relevant events.
        /// </summary>
        private void OnDestroy()
        {
            Alcohol.AlcoholObtained -= ChangeFilter;
        }
        
        /// <summary>
        /// Changes the filter when the player obtains alcohol.
        /// </summary>
        private void ChangeFilter()
        {
            filter.SetActive(false);
            StartCoroutine(ChangeFilterCoroutine());
            AudioManager.Instance.StopSoundtrack(0);
            AudioManager.Instance.PlaySoundtrack(1);
        }
    
        /// <summary>
        /// Changes the filter back after the alcohol effects have worn off.
        /// </summary>
        /// <returns>null</returns>
        private IEnumerator ChangeFilterCoroutine()
        {
            float elapsedTime = 0f;

            while (elapsedTime < alcoholFilterDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        
            Debug.Log("Alcohol effects have worn off, filter is back on!");
            filter.SetActive(true);
            AudioManager.Instance.StopSoundtrack(1);
            AudioManager.Instance.PlaySoundtrack(0);
            AlcoholRanOutEvent?.Invoke();
        }
    }
}
