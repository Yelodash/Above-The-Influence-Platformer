/*
using System.Collections;
using System.Collections.Generic;
using Audio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Enemy_AI_Scripts
{
    public class EnemyManagerOld : MonoBehaviour
    {
        // public Collider projectileCollider;
        public NavMeshAgent NavMeshAgent;
        public GameObject player;
        public Vector3 directionToPlayer;
        [Header("AttackSettings")]
        private bool _isAttackingPlayer;
       // public float delayInSeconds;
        public float shootDuration = 5f;
        public float attackRange = 10f;
        public float bulletSpeed = 3f;
      //  public GameObject gun;
        public GameObject endpoint;
        public GameObject projectile;
        
        [Header("EnemySettings")]
        [FormerlySerializedAs("PlayerTransform")] 
        public Transform playerTransform;

        public float detectionRadius;
        [SerializeField] 
        private float distanceToPlayer;
        public float minDistanceToPlayer;
        public Transform[] waypoints;
        private int _currentWaypointIndex;

        [SerializeField] // test to see if is patrolling is working
        private bool isPatrolling = true;
        private bool coroutineAlreadyRunning;
        [FormerlySerializedAs("_animator")] public Animator animator;

        private readonly EnemyInvestigating enemyInvestigating;

        public EnemyManagerOld()
        {
            enemyInvestigating = new EnemyInvestigating(this);
        }

        // private bool hasDetectedPlayer = false;
        private void Start()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            //SetNextWaypoint();
            animator = GetComponentInChildren<Animator>();

        }

        private void Update()
        {
            if (isPatrolling)
            {
                Patrol();
                animator.SetBool("isPatrolling", true);
                // Debug.Log("is patrolling");
            }
            else
            {
                FollowPlayer();
                animator.SetBool("isPatrolling", false);
                // Debug.Log("follow player");
            }
            
            // Set the rotation while locking the x-axis rotation to 0
            Quaternion targetRotation = Quaternion.Euler(0f, gameObject.transform.rotation.eulerAngles.y, 0f);
            gameObject.transform.rotation = targetRotation;

        }


        #region Movement

        void Patrol()
        {
            if (!NavMeshAgent.pathPending && NavMeshAgent.remainingDistance < 0.1f)
            {
                enemyInvestigating.SetNextWaypoint();
            }

            // float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            float toPlayer = Vector3.Distance(transform.position, playerTransform.position);
            // Check if the player is within the detection radius then set is patrolling to false
            if (toPlayer < detectionRadius)
            {
                isPatrolling = false;
            }

        }
        
        void FollowPlayer()
        {
            var distance = Vector3.Distance(transform.position, playerTransform.position);

            {
                if (distance < detectionRadius)
                {
                    NavMeshAgent.destination = playerTransform.position;

                    if (distance < minDistanceToPlayer)
                    {
                        // Stop the AI from getting too close
                        StopAI();
                        AttackPlayer();
                    }
                    else

                    {
                        ResumeAI();
                    }
                    
                }
                
                if (distance > detectionRadius)
                {
                    ReturnToPatrol();
                }

            }

        }

        void StopAI()
        {
            NavMeshAgent.isStopped = true;

            directionToPlayer = playerTransform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
            animator.SetBool("isWaiting", true);
        }

        void ResumeAI()
        {
            NavMeshAgent.isStopped = false;
            animator.SetBool("isAttacking", false);
            animator.SetBool("isPatrolling", true);
        }

        void AttackPlayer()
        {
            
            if (!_isAttackingPlayer)
            {
                distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                // Check if the player is within attack range
                if (distanceToPlayer <= attackRange)
                {
                    animator.SetBool("isAttacking", true);
                    _isAttackingPlayer = true;
                    Shoot();
                    StartCoroutine(FireAtPlayer());

                    Debug.Log("Getting Ready To Attack");

                    // make gun face player
                    directionToPlayer = endpoint.transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(directionToPlayer);

                    // directionToPlayer = gun.transform.position - transform.position;
                    // transform.rotation = Quaternion.LookRotation(directionToPlayer);
                }
                else
                {
                    animator.SetBool("isAttacking", false);
                }
            }
        }

        private IEnumerator FireAtPlayer()
        {
            // Set _isAttackingPlayer to true to prevent multiple coroutines running simultaneously
            _isAttackingPlayer = true;

            while (true)
            {
                // Check if the player is within attack range
                if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
                {
                    //Debug.Log("Fire At Player");
                    Shoot();
                    animator.SetBool("isAttacking", true);
                    animator.SetBool("iswaiting", false);
                    animator.SetBool("isPatrolling", false);
                }
                // Wait for the specified shoot duration before firing again
                yield return new WaitForSeconds(shootDuration);
            }
        }

        void Shoot()
        {
           // projectile.SetActive(value: true);

            if (projectile != null && endpoint != null)
            {
                // Instantiate the projectile at the endpoint's position and rotation
                var newProjectile = Instantiate(original: projectile, position: endpoint.transform.position,
                    rotation: endpoint.transform.rotation);
                // set the new projectile to be active
                newProjectile.SetActive(value: true);

                distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                if (newProjectile != null)
                {
                    AudioManager.Instance.PlaySoundAtPoint(9, player.transform.position);
                }

                /*  if (player.c.CompareTag("Player"))
                  {
                      Debug.Log("Play audio when player is hit");
                      Destroy(gameObject); // Destroy the projectile
                      AudioManager.Instance.PlaySound(27); // Play hit sound
                  } #1#


                // Add velocity to the bullet (if it has a Rigidbody component)
                if (newProjectile != null)
                {
                    var bulletRb = newProjectile.GetComponent<Rigidbody>();

                    if (bulletRb == null) return;

                    var position = player.transform.position;

                    directionToPlayer = (playerTransform.position - newProjectile.transform.position).normalized;
                    var forwardDirection = position;
                    bulletRb.AddForce(directionToPlayer * bulletSpeed, ForceMode.VelocityChange);
                    // Set the velocity of the projectile to move towards the player
                    // bulletRb.velocity = directionToPlayer * bulletSpeed;
                }
            }
            else
            {
                Debug.LogWarning(message: "Projectile or endpoint object is not assigned.");
            }
        }

        void ReturnToPatrol()
        {
            if (!isPatrolling)
            {
                isPatrolling = true;
                
                Debug.Log("Return to patrol");
            }

        }

        // private IEnumerator Wait()
        // {
        //     return enemyInvestigating.Wait();
        // }

        // void SetNextWaypoint()
        // {
        //     enemyInvestigating.SetNextWaypoint();
        // }
        
        
        
        #endregion Movement

        // Working on This
        //Separate script for hit

        /*private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);

            // 8 is the layer for "Damageable" objects
            if (other.gameObject.layer == 7)
            {
                UnitHealth unitHealth = other.GetComponent<UnitHealth>();
                if (unitHealth != null)
                {
                    unitHealth.TakeDamage((int)damageAmount);
                }
            }
            // FindObjectOfType<HealthBarScript>().SetHealth(damageAmount);
            Debug.LogWarning("Projectile hit");
            
        }#1#
    }
}
*/
