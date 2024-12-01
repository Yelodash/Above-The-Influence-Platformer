using UnityEngine;
/// <summary>
/// The AttributeData class is responsible for storing the attributes of the player.
/// </summary>
[CreateAssetMenu(fileName = "New Attribute Data", menuName = "Game/Attribute Data")]


public class AttributeData : ScriptableObject
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    [Header("Attack")]
    public float attackPower;
    public int attackPowerGun;
    public float attackPowerMelee;
    public float attackPowerSpin;
    [Header("Defence")]
    public float defence;
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchWalkSpeed;
    public float crouchJumpForce;
    public float jumpForce;
}