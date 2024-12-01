using System.Collections;
using UnityEngine;

namespace Enemy_AI_Scripts
{
    /// <summary>
    /// Manages the enemy investigating in the game.
    /// </summary>
    public class EnemyInvestigating
    {
        private readonly EnemyManager enemyManager;
        private int currentWaypointIndex;
        private bool coroutineAlreadyRunning;
        private static readonly int IsWaiting = Animator.StringToHash("isWaiting");
        private static readonly int IsPatrolling = Animator.StringToHash("isPatrolling");

        public EnemyInvestigating(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
        }

        /// <summary>
        /// Coroutine to wait at the waypoint.
        /// </summary>
        /// <returns></returns>
        public IEnumerator Wait()
        {
            if (!enemyManager.isAttackingPlayer)
            {
                // Set the destination to the next waypoint
                enemyManager.navMeshAgent.SetDestination(enemyManager.waypoints[currentWaypointIndex].position);

                yield return new WaitForSeconds(5f);

                enemyManager.enemyAnimator.SetBool(IsWaiting, true);
                enemyManager.enemyAnimator.SetBool(IsPatrolling, false);
                enemyManager.navMeshAgent.isStopped = true;

                yield return new WaitForSeconds(5f);
                enemyManager.enemyAnimator.SetBool(IsWaiting, false);
                enemyManager.enemyAnimator.SetBool(IsPatrolling, true);
                enemyManager.navMeshAgent.isStopped = false;
                
                currentWaypointIndex = (currentWaypointIndex + 1) % enemyManager.waypoints.Length;
                yield return null;
            }
        }
        /// <summary>
        /// Set the next waypoint for the enemy to go to.
        /// </summary>
        ///  <returns>
        /// The next waypoint for the enemy to go to.
        /// </returns>
        public void SetNextWaypoint()
        {
            if (!coroutineAlreadyRunning)
            {
                enemyManager.StartCoroutine(Wait());
            }
        }
    }
}