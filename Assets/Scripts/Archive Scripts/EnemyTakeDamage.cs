/*
using System.Collections;
using Audio;
using UnityEngine;

namespace Enemy_AI_Scripts
{
    public class EnemyTakeDamage : MonoBehaviour
    {

        [SerializeField] private int damage = 10;
        // public HealthBarScript healthBar;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);

            // 8 is the layer for "Damageable" objects
            if (other.gameObject.layer == 8)
            {
                AudioManager.Instance.PlaySound(Random.Range(23, 25));
                // UnitHealth unitHealth = other.GetComponent<UnitHealth>();
                // if (unitHealth != null)
                // {
                //     damageConversion = Mathf.RoundToInt(damage);
                //     unitHealth.TakeDamage(damageConversion);
                // }
                UnitHealth unitHealth = other.GetComponent<UnitHealth>();
                if (unitHealth != null)
                {
                    damage = Mathf.RoundToInt(damage);
                    unitHealth.TakeDamage(damage);
                }
            }
        }
    }
}
*/
