using UnityEngine;

namespace Enemy_AI_Scripts
{
    /// <summary>
    /// Manages the enemy movement in the game.
    /// </summary>
    public class EnemyMovement
    {
        private readonly EnemyManager enemyManager;

        public EnemyMovement(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        /// <summary>
        ///  follow player if player is within detection radius
        /// </summary>
        public void FollowPlayer()
        {
            var distance = Vector3.Distance(enemyManager.transform.position, enemyManager.playerTransform.position);
            enemyManager.enemyAnimator.SetBool(EnemyManager.IsWaiting, false);
            {
                if (distance < enemyManager.detectionRadius)
                {
                    enemyManager.navMeshAgent.destination = enemyManager.playerTransform.position;
                    enemyManager.navMeshAgent.speed = enemyManager.alertSpeed ;

                    if (distance < enemyManager.minDistanceToPlayer)
                    {
                        StopAI();
                        enemyManager.EnemyAttack.AttackPlayer();
                    }
                    else
                    {
                        ResumeAI();
                        enemyManager.enemyAnimator.SetBool(EnemyManager.IsWaiting, true);
                    }
                    
                }
                if (distance > enemyManager.detectionRadius)
                {
                    enemyManager.EnemyPatrol.ReturnToPatrol();
                    enemyManager.navMeshAgent.speed = enemyManager.defaultSpeed;
                   // Debug.Log("Returning to patrol"); Debugging when return to patrol wasn't working
                }

            }

        }

/// <summary>
/// Stop AI when player is within attack range
/// </summary>
        private void StopAI()
        {
            enemyManager.navMeshAgent.isStopped = true;

            enemyManager.directionToPlayer = enemyManager.playerTransform.position - enemyManager.transform.position;
            enemyManager.transform.rotation = Quaternion.LookRotation(enemyManager.directionToPlayer);
            enemyManager.enemyAnimator.SetBool(EnemyManager.IsWaiting, true);
        }
/// <summary>
/// Resume AI when player is out of attack range
/// </summary>
        private void ResumeAI()
        {
            enemyManager.navMeshAgent.isStopped = false;
            // animator.SetBool("isAttacking", false);
            enemyManager.enemyAnimator.SetBool(EnemyManager.IsPatrolling, true);
        }
    }
}