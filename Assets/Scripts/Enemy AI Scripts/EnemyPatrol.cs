using UnityEngine;

namespace Enemy_AI_Scripts
{
    /// <summary>
    /// anages the enemy movement in the game.
    /// </summary>
    public class EnemyPatrol
    {
        private readonly EnemyManager enemyManager;

        public EnemyPatrol(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        /// <summary>
        ///Patrols the enemy.   
        /// </summary>
        public void Patrol()
        {
            if (!enemyManager.navMeshAgent.pathPending && enemyManager.navMeshAgent.remainingDistance < 0.1f)
            {
                enemyManager.EnemyInvestigating.SetNextWaypoint();
            }

            // float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float toPlayer = Vector3.Distance(enemyManager.transform.position, enemyManager.playerTransform.position);
            // Check if the player is within the detection radius then set is patrolling to false
            if (toPlayer < enemyManager.detectionRadius)
            {
                enemyManager.isPatrolling = false;
            }

        }

        /// <summary>
        ///follow player if player is within detection radius
        /// </summary>
        public void ReturnToPatrol()
        {
            if (!enemyManager.isPatrolling)
            {
                enemyManager.isPatrolling = true;
                enemyManager.isAttackingPlayer = false;
                // added this after thr refactoring the next waypoint would not be set
                 Patrol();
                // enemyManager.EnemyInvestigating.SetNextWaypoint();
                
            }

        }
    }
}