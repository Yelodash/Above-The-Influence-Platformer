/// <summary>
/// Interface for the health bar component.
/// </summary>
public interface IHealthBar
{
    /// <summary>
    /// Sets the maximum health value for the health bar.
    /// </summary>
    /// <param name="maxHealth">The maximum health value.</param>
    void SetMaxHealth(int maxHealth);

    /// <summary>
    /// Sets the current health value for the health bar.
    /// </summary>
    /// <param name="health">The current health value.</param>
    void SetHealth(int health);
}
