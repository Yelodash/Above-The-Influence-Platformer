using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Manages the levels in the game.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        /// <summary>
        /// Ensures that there is only one instance of the LevelManager.
        /// </summary>
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// On level one complete set the PlayPref key to 1.
        /// </summary>
        public void LevelOneComplete()
        {
            PlayerPrefs.SetInt("LevelOneValue", 1);
        }
    
        /// <summary>
        /// On level two complete set the PlayPref key to 1.
        /// </summary>
        public void LevelTwoComplete()
        {
            PlayerPrefs.SetInt("LevelTwoValue", 1);
        }

        /// <summary>
        /// On level three complete set the PlayPref key to 1.
        /// </summary>
        public void LevelThreeComplete()
        {
            PlayerPrefs.SetInt("LevelThreeValue", 1);
        }
    }
}
