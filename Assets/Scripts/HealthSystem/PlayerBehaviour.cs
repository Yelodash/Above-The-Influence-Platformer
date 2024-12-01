// using Managers;
// using UnityEngine;
//
// /// <summary>
// /// Controls the behavior of the player character.
// /// </summary>
// public class PlayerBehaviour : MonoBehaviour
// {
//     [SerializeField] HealthBarScript _healthbar;
//
//     void Update()
//     {
//         // Check for input to simulate player taking damage or healing
//         if (Input.GetKeyDown(KeyCode.Q))
//         {
//           // PlayerTakeDamage(20);
//         }
//
//         if (Input.GetKeyDown(KeyCode.O))
//         {
//             PlayerHeal(20);
//         }
//     }
//
//     /// <summary>
//     /// Simulates the player taking damage.
//     /// </summary>
//     /// <param name="damage">Amount of damage to take.</param>
//     private void PlayerTakeDamage(int damage)
//     {
//         if (GameManager.Instance != null && GameManager.Instance.playerHealth != null)
//         {
//             GameManager.Instance.playerHealth.TakeDamage(damage);
//             Debug.Log("Health after taking damage: " + GameManager.Instance.playerHealth.Health);
//             _healthbar.SetHealth(GameManager.Instance.playerHealth.Health);
//         }
//         else
//         {
//             // Debug.LogError("GameManager or PlayerHealth is null!");
//         }
//     }
//
//     /// <summary>
//     /// Simulates the player healing.
//     /// </summary>
//     /// <param name="healing">Amount of healing to apply.</param>
//     private void PlayerHeal(int healing)
//     {
//         if (GameManager.Instance != null && GameManager.Instance.playerHealth != null)
//         {
//             GameManager.Instance.playerHealth.Heal(healing);
//             Debug.Log("Health after healing: " + GameManager.Instance.playerHealth.Health);
//             _healthbar.SetHealth(GameManager.Instance.playerHealth.Health);
//         }
//         else
//         {
//             Debug.LogError("GameManager or PlayerHealth is null!");
//         }
//     }
// }
