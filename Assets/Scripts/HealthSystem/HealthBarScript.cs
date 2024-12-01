using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Implements the IHealthBar interface to control a UI slider representing health.
/// </summary>
public class HealthBarScript : MonoBehaviour, IHealthBar
{
    private static HealthBarScript _instance;
    
    /// <summary>
    /// Gets the instance of the HealthBarScript singleton.
    /// </summary>
    public static HealthBarScript Instance { get { return _instance; } }

    /// <summary>
    ///  The slider component representing the health bar.
    /// </summary>
    private Slider _healthSlider;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Ensures only one instance of HealthBarScript exists
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        _healthSlider = GetComponent<Slider>();
    }

    

    /// <summary>
    /// Sets the maximum health value of the health bar.
    /// </summary>
    /// <param name="maxHealth">The maximum health value.</param>
    public void SetMaxHealth(int maxHealth)
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = maxHealth;
    }

    /// <summary>
    /// Sets the current health value of the health bar.
    /// </summary>
    /// <param name="health">The current health value.</param>
    public void SetHealth(int health)
    {
        _healthSlider.value = health;
    }
}