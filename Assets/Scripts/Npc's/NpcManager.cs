using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Npc_s
{
    /// <summary>
    ///  Manages the NPC in the game.
    /// </summary>
    public class NpcManager : MonoBehaviour
    {
        /// <summary>
        ///  The current waypoint index.
        /// </summary>
        ///  <value>The current waypoint index.</value>
        private int currentWaypointIndex;

        /// <summary>
        ///  The NavMeshAgent component.
        /// </summary>
        ///  <value>The NavMeshAgent component.</value>
        public NavMeshAgent navMeshAgent;

        /// <summary>
        ///  The waypoints the NPC will patrol.
        ///  </summary>
        ///  <value>The waypoints the NPC will patrol.</value>
        public Transform[] waypoints;

        /// <summary>
        ///  The NPC animator.
        /// </summary>
        public Animator npcAnimator;

        private bool waitToSpeak;

        public BoxCollider triggerBox;

        private static readonly int IsWaiting = Animator.StringToHash("isWaiting");
        private readonly NpcDialogue npcDialogue;

        /// <summary>
        ///   Constructor for the NpcManager class.
        /// </summary>
        public NpcManager()
        {
            npcDialogue = new NpcDialogue(this);
        }


        /// <summary>
        ///  Set the Npc to navigate to the next waypoint.
        /// </summary>
        public void Update()
        {
            Patrol();
        }

        /// <summary>
        ///  When the player enters the NPC trigger, the NPC will speak.
        /// </summary>
        ///  <param name="other">The object collided with - Should be the player</param>
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                npcDialogue.Dialogue();
                {
                    triggerBox.enabled = false;
                }
                
            }
        }

        private void Patrol()
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.1f)
            {
                SetNextWaypoint();
            }
        }

        private void SetNextWaypoint()
        {
            StartCoroutine(Wait());
        }

        private IEnumerator Wait()
        {
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            yield return new WaitForSeconds(5f);
            navMeshAgent.isStopped = true;
            npcAnimator.SetBool(IsWaiting, true);

            yield return new WaitForSeconds(10f);
            npcAnimator.SetBool(IsWaiting, false);
            navMeshAgent.isStopped = false;
        }
    }
}