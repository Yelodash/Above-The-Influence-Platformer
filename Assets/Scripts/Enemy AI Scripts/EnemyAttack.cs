using System.Collections;
using Audio;
using UnityEngine;

namespace Enemy_AI_Scripts
{
    /// <summary>
    /// Manages the enemy attack in the game.
    /// </summary>
    public class EnemyAttack
    {
        private readonly EnemyManager enemyManager;
        
        private static readonly int IsPatrolling = Animator.StringToHash("isPatrolling");
        private static readonly int IsWaiting = Animator.StringToHash("isWaiting");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");


        /// <summary>
        /// Constructor for the EnemyAttack class.
        /// </summary>
        /// <param name="enemyManager"></param>
        public EnemyAttack(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        /// <summary>
        ///Attacks the player.
        /// </summary>
        public void AttackPlayer()
        {

            if (!enemyManager.isAttackingPlayer)
            {
                enemyManager.distanceToPlayer = Vector3.Distance(enemyManager.transform.position,
                    enemyManager.playerTransform.position);
                // Check if the player is within attack range
                if (enemyManager.distanceToPlayer <= enemyManager.attackRange)
                {
                    enemyManager.enemyAnimator.SetBool(IsAttacking, true);
                    enemyManager.isAttackingPlayer = true;
                    Shoot();
                    enemyManager.StartCoroutine(FireAtPlayer());

                    // make gun face player
                    enemyManager.directionToPlayer = enemyManager.endpoint.transform.position - enemyManager.transform.position;
                    
                    enemyManager.transform.rotation = Quaternion.LookRotation(enemyManager.directionToPlayer);
                    
                }
                else
                {
                    enemyManager.enemyAnimator.SetBool(IsAttacking, false);
                }
            }
        }

        /// <summary>
        /// Coroutine to fire at the player.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FireAtPlayer()
        {
            // Set _isAttackingPlayer to true to prevent multiple coroutines running simultaneously
            enemyManager.isAttackingPlayer = true;

            while (true)
            {
                // Check if the player is within attack range
                if (Vector3.Distance(enemyManager.transform.position, enemyManager.playerTransform.position) <=
                    enemyManager.attackRange)
                {
                    //Debug.Log("Fire At Player");
                    Shoot();
                    enemyManager.enemyAnimator.SetBool(IsAttacking, true);
                    enemyManager.enemyAnimator.SetBool(IsWaiting, false);
                    enemyManager.enemyAnimator.SetBool(IsPatrolling, false);
                }

                // Wait for the specified shoot duration before firing again
                yield return new WaitForSeconds(enemyManager.shootDuration);
            }
        }

        
        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        ///  Shoots a projectile at the player.
        /// </summary>
        private void Shoot()
        {
            // Check if the projectile and endpoint are assigned
            if (enemyManager.projectile != null && enemyManager.endpoint != null)
            {
                // Calculate the direction from the endpoint to the player
                Vector3 directionToPlayer =
                    enemyManager.playerTransform.position - enemyManager.endpoint.transform.position;

                // Raycast to detect if the player is within range
                if (Physics.Raycast(enemyManager.endpoint.transform.position, directionToPlayer, out var hit,
                        enemyManager.attackRange))
                {
                    // Check if the raycast hit the player layer
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        // Debug.Log("Player is in range");
                        // Instantiate the projectile at the endpoint's position and rotation
                        GameObject newProjectile = Object.Instantiate(enemyManager.projectile,
                            enemyManager.endpoint.transform.position, enemyManager.endpoint.transform.rotation);
                        // Set the new projectile to be active
                        newProjectile.SetActive(true);

                        // Play sound effect
                        AudioManager.Instance.PlaySoundAtPoint(9, enemyManager.player.transform.position);

                        // Add velocity to the bullet (if it has a Rigidbody component)
                        Rigidbody bulletRb = newProjectile.GetComponent<Rigidbody>();
                        if (bulletRb != null)
                        {
                            bulletRb.velocity = directionToPlayer.normalized * enemyManager.bulletSpeed;
                        }
                    }
                }
            }
            else
            {
                //Debug.LogWarning("Projectile or endpoint object is not assigned.");
            }
        }
    }
}