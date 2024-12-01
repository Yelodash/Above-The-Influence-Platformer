using System.Net;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gun_System
{
    /// <summary>
    /// Manages the aiming camera in the game.
    /// </summary>
    public class AimingCameraSwapper : MonoBehaviour
    {
        public static AimingCameraSwapper Instance;

        /// <summary>
        /// Ensure that there is only one instance of the class.
        /// </summary>
        private void Awake()
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
        
        [Header("Field of View Settings")]
        [SerializeField] private int adsFieldOfView;
        [SerializeField] private int normalFieldOfView;
    
        [Header("Camera + Crosshair Settings")]
        [SerializeField] private GameObject crosshair;
        [SerializeField] private CinemachineVirtualCamera mainCinemachineCam;
    
        [Header("Default Camera Sensitivity Settings")]
        [SerializeField] public float defaultCamX = 1500f;
        [SerializeField] public float defaultCamY = 1500f;
    
        [Header("Aiming Camera Sensitivity Settings")]
        [SerializeField] public float adsCamX = 1500f;
        [SerializeField] public float adsCamY = 1500f;
    
        /// <summary>
        /// Constantly call the CheckAimState method.
        /// </summary>
        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Main Menu")
            {
                return;
            }
            
            CheckAimState();
        }

        /// <summary>
        ///Checks the aim state of the player.
        /// </summary>
        private void CheckAimState()
        {
            var camSens = mainCinemachineCam.GetCinemachineComponent<CinemachinePOV>();
            
            
            if (Input.GetKey(KeyCode.Mouse1))
            {
                // Aiming
                Debug.Log("shouldve changed");
                crosshair.SetActive(true);
                mainCinemachineCam.m_Lens.FieldOfView = adsFieldOfView;
                mainCinemachineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x = 1f;
                camSens.m_HorizontalAxis.m_MaxSpeed = adsCamX;
                camSens.m_VerticalAxis.m_MaxSpeed = adsCamY;
           
            }
            else
            {
                // Not aiming
                crosshair.SetActive(false);
                mainCinemachineCam.m_Lens.FieldOfView = normalFieldOfView;
                mainCinemachineCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset.x = 0f;
                camSens.m_HorizontalAxis.m_MaxSpeed = defaultCamX;
                camSens.m_VerticalAxis.m_MaxSpeed = defaultCamY;
            }
        }
    }
}