using System.Collections;
using Audio;
using Managers;
using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Load the relevant levels and set the player prefs values
    /// </summary>
    public class LevelLoader : MonoBehaviour
    {
        private int levelOneComplete;
        private int levelTwoComplete;
        private int levelThreeComplete;

        private bool hasBeenDelayed = false;
        
        [SerializeField] private GameObject[] lockedDoorObjects;
    
        /// <summary>
        /// Set the player prefs values and unlock the relevant doors
        /// </summary>
        private void Start()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Main Menu")
            {
                return;
            }

            StartCoroutine(DelayTriggerEvents());
            
            SetPrefValues();
            UnlockDoors();
        }

        /// <summary>
        /// Delay the trigger events to prevent the player from accidentally triggering them
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayTriggerEvents()
        {
            yield return new WaitForSeconds(4);
            hasBeenDelayed = true;
        }
        
        /// <summary>
        /// Dev unlocker and resetter
        /// </summary>
        private void Update()
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Main Menu")
            {
                return;
            }

            // Resetter
            if (Input.GetKeyDown(KeyCode.F11))
            {
                // Set values
                ResetSaveData();
                LockDoors();
            
                // Alert
                AudioManager.Instance.PlaySound(33);
            }

            // Unlock tool
            if (Input.GetKeyDown(KeyCode.F12))
            {
                // Set values
                PlayerPrefs.SetInt("LevelOneValue", 1);
                PlayerPrefs.SetInt("LevelTwoValue", 1);
                PlayerPrefs.SetInt("LevelThreeValue", 1);
            
                SetPrefValues();
                UnlockDoors();
            
                // Alert
                AudioManager.Instance.PlaySound(33);
            }
        }

        /// <summary>
        /// Load the values from the PlayerPrefs
        /// </summary>
        private void SetPrefValues()
        {
            levelOneComplete = PlayerPrefs.GetInt("LevelOneValue");
            levelTwoComplete = PlayerPrefs.GetInt("LevelTwoValue");
            levelThreeComplete = PlayerPrefs.GetInt("LevelThreeValue"); 
        }

        /// <summary>
        /// Lock all the levels and set them
        /// </summary>
        public void ResetSaveData()
        {
            PlayerPrefs.SetInt("LevelOneValue", 0);
            PlayerPrefs.SetInt("LevelTwoValue", 0);
            PlayerPrefs.SetInt("LevelThreeValue", 0);
            SetPrefValues();
        }

        /// <summary>
        /// Set the door states depending on the PlayerPrefs values
        /// </summary>
        private void UnlockDoors()
        {
            foreach (var doorObj in lockedDoorObjects)
            {
                if (doorObj != null)
                {
                    if (levelOneComplete == 1)
                    {
                        lockedDoorObjects[0].SetActive(false);
                    }
        
                    if (levelTwoComplete == 1)
                    {
                        lockedDoorObjects[1].SetActive(false);
                    } 
                }
            }
        }

        /// <summary>
        /// Enable the locks on the doors defined in the list
        /// </summary>
        private void LockDoors()
        {
            foreach (var doorObj in lockedDoorObjects)
            {
                if (doorObj != null)
                {
                    lockedDoorObjects[0].SetActive(true);
                    lockedDoorObjects[1].SetActive(true);
                }
            }
        }

        /// <summary>
        /// Takes the player to a level depending on the layer of the collider it hits
        /// </summary>
        /// <param name="other">other is the object that is collided with</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 12 && hasBeenDelayed)
            {
                GameManager.Instance.LoadLevelOne();
                Debug.Log("Loading level one...");
            }
        
            if (other.gameObject.layer == 13 && levelOneComplete == 1 && hasBeenDelayed)
            {
                GameManager.Instance.LoadLevelTwo();
                Debug.Log("Loading level two...");
            }
        
            if (other.gameObject.layer == 14 && levelTwoComplete == 1 && hasBeenDelayed)
            {
                GameManager.Instance.LoadLevelThree();
                Debug.Log("Loading level three...");
            }

            if (other.gameObject.layer == 18 && hasBeenDelayed)
            {
                GameManager.Instance.LoadEndScene();
                Debug.Log("Loading the end...");
            }

            if (other.gameObject.layer == 16 && hasBeenDelayed)
            {
                GameManager.Instance.LoadTutorial();
            }

            if (other.gameObject.layer == 15 && hasBeenDelayed)
            {
                var currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                if (currentScene == "Level One")
                {
                    LevelManager.Instance.LevelOneComplete();
                }
                else if (currentScene == "Level Two")
                {
                    LevelManager.Instance.LevelTwoComplete();
                }
                else if (currentScene == "Level Three")
                {
                    LevelManager.Instance.LevelThreeComplete();
                }
                
                GameManager.Instance.LoadHub();
                Debug.Log("Loading the hub...");
            }
        }
    }
}
