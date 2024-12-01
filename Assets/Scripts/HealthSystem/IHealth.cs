/// <summary>
/// Interface for objects that have health properties and methods.
/// </summary>
public interface IHealth
{
    
    /// <summary>
    /// Decreases the unit's health by the specified amount.
    /// </summary>
    /// <param name="dmgAmount">The amount of damage to inflict.</param>
    /// <param name="damageAmount"></param>
    void TakeDamage(int damageAmount);

    /// <summary>
    /// Increases the unit's health by the specified amount.
    /// </summary>
    /// <param name="healAmount">The amount of healing to apply.</param>
    void HealUnit(int healAmount);
}