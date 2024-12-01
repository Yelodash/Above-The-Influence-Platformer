using Gun_System;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Interface_Sliders
{
    public class CameraSensitivitySlider : MonoBehaviour
    {
        [SerializeField] private Slider adsSensitivitySlider;
        [SerializeField] private Slider normalSensitivitySlider;

        /// <summary>
        /// Loads the sensitivity values from PlayerPrefs and sets the initial values of the sliders
        /// </summary>
        private void Start()
        {
            // Load sensitivity values from PlayerPrefs
            LoadSensitivities();
            
            // Set the initial value of the sliders to the loaded sensitivity values
            // Division values are the set values at the beginning for the ADS and normal sensitivity
            adsSensitivitySlider.value = AimingCameraSwapper.Instance.adsCamX / 700f;
            normalSensitivitySlider.value = AimingCameraSwapper.Instance.defaultCamX / 800f;
            
            // Add listeners to sliders
            adsSensitivitySlider.onValueChanged.AddListener(ChangeAimSensitivity);
            normalSensitivitySlider.onValueChanged.AddListener(ChangeNormalSensitivity);
        }

        /// <summary>
        /// Removes the listener when the object is destroyed
        /// </summary>
        private void OnDestroy()
        {
            adsSensitivitySlider.onValueChanged.RemoveListener(ChangeAimSensitivity);
            normalSensitivitySlider.onValueChanged.RemoveListener(ChangeNormalSensitivity);
        }

        /// <summary>
        /// Changes the aim sensitivity of the camera
        /// </summary>
        /// <param name="newSensitivity">Sensitivity value</param>
        private static void ChangeAimSensitivity(float newSensitivity)
        {
            newSensitivity *= 700;
            newSensitivity = Mathf.Max(newSensitivity, 100); // Ensure sensitivity doesn't drop below 100
            AimingCameraSwapper.Instance.adsCamX = newSensitivity;
            AimingCameraSwapper.Instance.adsCamY = newSensitivity;
            SaveAimSensitivity(newSensitivity);
        }

        /// <summary>
        /// Changes the normal sensitivity of the camera
        /// </summary>
        /// <param name="newSensitivity">Sensitivity value</param>
        private static void ChangeNormalSensitivity(float newSensitivity)
        {
            newSensitivity *= 800;
            newSensitivity = Mathf.Max(newSensitivity, 100); // Ensure sensitivity doesn't drop below 100
            AimingCameraSwapper.Instance.defaultCamX = newSensitivity;
            AimingCameraSwapper.Instance.defaultCamY = newSensitivity;
            SaveNormalSensitivity(newSensitivity);
        }

        /// <summary>
        /// Saves the new normal sensitivity to PlayerPrefs
        /// </summary>
        /// <param name="newSensitivity"></param>
        private static void SaveNormalSensitivity(float newSensitivity)
        {
            PlayerPrefs.SetFloat("NormalSensitivity", newSensitivity);
        }

        /// <summary>
        /// Saves the new ADS sensitivity to PlayerPrefs
        /// </summary>
        /// <param name="newSensitivity"></param>
        private static void SaveAimSensitivity(float newSensitivity)
        {
            PlayerPrefs.SetFloat("AimSensitivity", newSensitivity);
        }

        /// <summary>
        /// Loads the sensitivity values from PlayerPrefs
        /// </summary>
        private static void LoadSensitivities()
        {
            AimingCameraSwapper.Instance.adsCamX = PlayerPrefs.GetFloat("AimSensitivity", 700f);
            AimingCameraSwapper.Instance.adsCamY = PlayerPrefs.GetFloat("AimSensitivity", 700f);
            AimingCameraSwapper.Instance.defaultCamX = PlayerPrefs.GetFloat("NormalSensitivity", 800f);
            AimingCameraSwapper.Instance.defaultCamY = PlayerPrefs.GetFloat("NormalSensitivity", 800f);
        }
    }
}

