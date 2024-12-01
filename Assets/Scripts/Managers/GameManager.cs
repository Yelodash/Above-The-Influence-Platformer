using HealthSystem;
using Powerups;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Managers
{
    /// <summary>
    /// Event delegate for when the pistol is enabled.
    /// </summary>
    public delegate void PistolEnabledChangedEventDelegate(bool isPistolEnabled);
    
    /// <summary>
    /// Event delegate for when the tommy gun is enabled.
    /// </summary>
    public delegate void TommyGunEnabledChangedEventDelegate(bool isTommyGunEnabled);
    
    /// <summary>
    /// Event delegate for when the player picks up an attack powerup.
    /// </summary>
    public delegate void AttackPowerupEventHandler();
    
    /// <summary>
    /// Event delegate for when the player picks up an instakill powerup.
    /// </summary>
    public delegate void InstakillPowerupEventHandler();
    
    /// <summary>
    /// Event delegate for when the player picks up a pistol powerup.
    /// </summary>
    public delegate void PistolPowerupEventHandler();
    
    /// <summary>
    /// Event delegate for when the player picks up a tommygun powerup.
    /// </summary>
    public delegate void TommyGunPowerupEventHandler();
    
    /// <summary>
    /// Event delegate for when the player picks up a health powerup.
    /// </summary>
    public delegate void HealthPowerupEventHandler();
    
    /// <summary>
    /// Event delegate for when the player picks up a defence powerup.
    /// </summary>
    public delegate void DefencePowerupEventHandler();
    
    /// <summary>
    /// Event delegate for when the player picks up a speed powerup.
    /// </summary>
    public delegate void SpeedPowerupEventHandler();
    
    /// <summary>
    /// Event delegate for when the player picks up a jump powerup.
    /// </summary>
    public delegate void JumpForcePowerupEventHandler();
    
    /// <summary>
    /// Event delegate for when the player picks up an alcohol bottle.
    /// </summary>
    public delegate void AlcoholEventHandler();
    
    /// <summary>
    /// Event delegate for when the player runs alcohol timer runs out.
    /// </summary>
    public delegate void AlcoholRanOut();
    
    /// <summary>
    /// Event delegate for when the player is on a platform
    /// </summary>
    public delegate void PlayerOnPlatform();
    
    /// <summary>
    /// Event delegate for when the player is off a platform
    /// </summary>
    public delegate void PlayerOffPlatform();
    
    /// <summary>
    /// Event delegate to fix the player's ammo.
    /// </summary>
    public delegate void FixupAmmo();
    
    /// <summary>
    /// Event delegate for when the player jumps on a bin
    /// </summary>
    public delegate void OnBinHit(int bounceForce);
    
    /// <summary>
    /// Event delegate for when the player jumps on a ladder
    /// </summary>
    public delegate void OnLadderClimb(float climbSpeed);
    
    /// <summary>
    /// Event delegate for when the player initiates a spin attack
    /// </summary>
    public delegate void OnSpinAttack(bool isSpinning);

    /// <summary>
    /// Manages the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static event PistolEnabledChangedEventDelegate OnPistolEnabled;
        public static event TommyGunEnabledChangedEventDelegate OnTommyGunEnabled;
        public static event TommyGunPowerupEventHandler OnTommyGunPowerup;
    
        public static GameManager Instance;

        public GameObject hotbar;
        private bool _isPaused, _isCursorEnabled = false;
        /// <summary>
        /// 
        /// </summary>
        public bool isPistolEnabled = true;
        public bool areHandsEnabled = false;
        public bool isTommyGunEnabled = false;
        [FormerlySerializedAs("testBool")] public bool isShopOpen;
        
        [SerializeField] private GameObject pauseMenuCanvas;
        [SerializeField] private GameObject settingsMenuCanvas;
        [SerializeField] private GameObject mainMenuCanvas;

        [FormerlySerializedAs("PlayerHealth")] public UnitHealth playerHealth;
        [SerializeField] private Animator playerAnimator;
    
        [SerializeField] private GameObject pistol;
        [SerializeField] private GameObject tommyGun;
    
    
        /// <summary>
        /// The initial player health.
        /// </summary>
        public int initialPlayerHealth = 100;

        /// <summary>
        /// Ensures that only one instance of the GameManager exists.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Subscribe to the relevant events
        /// Ensure that the main menu canvas is enabled when the game starts if the scene is the main menu
        /// Set the initial checkpoint position and player health
        /// </summary>
        private void Start()
        {
            PistolPowerup.PistolPowerupObtained += EnablePistol;
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                mainMenuCanvas.SetActive(true);
            }
            
            if (SceneManager.GetActiveScene().name != "Main Menu" && SceneManager.GetActiveScene().name != "Game End")
            {
                CheckpointManager.Instance.SetCheckpoint(Vector3.zero, initialPlayerHealth);
                Debug.Log("Initial Player Health: " + initialPlayerHealth);
            }
        }

        /// <summary>
        /// Unsubscribe from the relevant events
        /// </summary>
        private void OnDestroy()
        {
            PistolPowerup.PistolPowerupObtained -= EnablePistol;
        }

        /// <summary>
        /// Enables the pistol.
        /// </summary>
        private void EnablePistol()
        {
            isPistolEnabled = true; // Set the flag to enable the gun
        }
    
        /// <summary>
        /// Enables the tommy gun.
        /// </summary>
        private void EnableTommyGun()
        {
            isTommyGunEnabled = true; // Set the flag to enable the gun
        }

        /// <summary>
        /// Invoke events to handle the state of the guns.
        /// </summary>
        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name == "Game End")
            {
                return;
            }

            // Debug log for checking if the escape key is pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Toggle the pause state of the game
                TogglePause();
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                ToggleCursor();
            }

            OnPistolEnabled?.Invoke(isPistolEnabled);
            OnTommyGunEnabled?.Invoke(isTommyGunEnabled);
        
            if (isPistolEnabled)
            {
                isTommyGunEnabled = false;
                areHandsEnabled = false;
                playerAnimator.SetBool("IsGunEnabled", true);
            
            }
            else if (isTommyGunEnabled)
            {
                isPistolEnabled = false;
                areHandsEnabled = false;
                playerAnimator.SetBool("IsGunEnabled", true);
            }
            else if (areHandsEnabled)
            {
                isPistolEnabled = false;
                isTommyGunEnabled = false;
                playerAnimator.SetBool("IsGunEnabled", false);
            }
        
            if (!isPistolEnabled && !isTommyGunEnabled)
            {
                areHandsEnabled = true;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                EnableTommyGun();
                OnTommyGunPowerup?.Invoke();
            }

            if (pistol.activeSelf == false && tommyGun.activeSelf == false)
            {
                playerAnimator.SetBool("IsGunEnabled", false);
            }
        }

        /// <summary>
        /// Toggles the pause state of the game.
        /// </summary>
        private void TogglePause()
        {
            _isPaused = !_isPaused; // Invert the pause state

            if (_isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PauseTheGame();
                // Add other actions to perform when the game is paused
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ResumeTheGame();
                // Add other actions to perform when the game resumes from pause
            }
        }

        /// <summary>
        /// Toggle the cursor visibility.
        /// </summary>
        private void ToggleCursor()
        {
           _isCursorEnabled = !_isCursorEnabled;

           if (!_isPaused)
           {
               if (_isCursorEnabled)
               {
                   Cursor.lockState = CursorLockMode.None;
                   Cursor.visible = true;
               }
               else
               {
                   Cursor.lockState = CursorLockMode.Locked;
                   Cursor.visible = false;
               }
           }
        }

        /// <summary>
        /// Starts the game by loading the Hub scene
        /// </summary>
        public void StartTheGame()
        {
            Debug.Log("Loading the Hub!");
            SceneManager.LoadScene("Hub");
        }

        /// <summary>
        /// Quits the game.
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("Application would have quit here.");
            Application.Quit();
        }

        /// <summary>
        /// Exits to the game title screen.
        /// </summary>
        public void ExitToGameTitleScreen()
        {
            Debug.Log("Loading the Main Menu scene!");
            Time.timeScale = 1;
            SceneManager.LoadScene("Main Menu");
        }

        /// <summary>
        /// Pauses the game.
        /// </summary>
        private void PauseTheGame()
        {
            settingsMenuCanvas.SetActive(false);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            pauseMenuCanvas.SetActive(true);
        }

        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void ResumeTheGame()
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenuCanvas.SetActive(false);
        }

        /// <summary>
        /// Opens the settings menu.
        /// </summary>
        public void OpenSettingsMenu()
        {
            settingsMenuCanvas.SetActive(true);
            mainMenuCanvas.SetActive(false);
        }

        /// <summary>
        /// Closes the settings menu.
        /// </summary>
        public void CloseSettingsMenu()
        {
            settingsMenuCanvas.SetActive(false);
            mainMenuCanvas.SetActive(true);
        }

        /// <summary>
        /// Loads the first level.
        /// </summary>
        public void LoadLevelOne()
        {
            SceneManager.LoadScene("Level One");
            Debug.Log("Loading level one...");
        }

        /// <summary>
        /// Loads the second level.
        /// </summary>
        public void LoadLevelTwo()
        {
            SceneManager.LoadScene("Level Two");
            Debug.Log("Loading level one...");
        }

        /// <summary>
        /// Loads the third level.
        /// </summary>
        public void LoadLevelThree()
        {
            SceneManager.LoadScene("Level Three");
            Debug.Log("Loading level one...");
        }

        /// <summary>
        /// Loads the game end scene.
        /// </summary>
        public void LoadEndScene()
        {
            SceneManager.LoadScene("Game End");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("Loading the game end scene...");
        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void RestartLevel()
        {
            var scene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(scene);
        }
    
        /// <summary>
        /// Loads the hub scene.
        /// </summary>
        public void LoadHub()
        {
            SceneManager.LoadScene("Hub");
        }
        
        /// <summary>
        /// Loads the tutorial level.
        /// </summary>
        public void LoadTutorial()
        {
            SceneManager.LoadScene("Tutorial Level");
            Debug.Log("Loading the tutorial level...");
        }
    }
}
