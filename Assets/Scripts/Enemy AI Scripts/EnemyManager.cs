using UnityEngine;
using UnityEngine.AI;

namespace Enemy_AI_Scripts
{
    /// <summary>
    /// Manages the enemy in the game.
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        [Header("References")]
         public NavMeshAgent navMeshAgent;
        public GameObject player;
        public Vector3 directionToPlayer;
        public GameObject endpoint;
        public GameObject projectile;
        
        [Header("Attack Settings")]
        [HideInInspector]
        public bool isAttackingPlayer;
        public float shootDuration = 5f;
        public float attackRange = 10f;
        public float bulletSpeed = 3f;
        
        [Header("Enemy Settings")]
        public Transform playerTransform;
        [Header("Detection Setting")]
        public float detectionRadius;
        public float defaultSpeed;
        public float alertSpeed;
        [Header("Stopping Distance")]
        public float distanceToPlayer;
        public float minDistanceToPlayer;
        [Header("Navigation Points")]
        public Transform[] waypoints;
        private int _currentWaypointIndex;
        [SerializeField]
        public bool isPatrolling = true;
        private bool coroutineAlreadyRunning;
         public Animator enemyAnimator;

         public readonly EnemyInvestigating EnemyInvestigating;
        public readonly EnemyAttack EnemyAttack;
        public readonly EnemyPatrol EnemyPatrol;
        private readonly EnemyMovement enemyMovement;
        public static readonly int IsPatrolling = Animator.StringToHash("isPatrolling");
        public static readonly int IsWaiting = Animator.StringToHash("isWaiting");

        /// <summary>
        /// Constructor for the EnemyManager class.
        /// </summary>
        public EnemyManager()
        {
            EnemyInvestigating = new EnemyInvestigating(this);
            EnemyAttack = new EnemyAttack(this);
            EnemyPatrol = new EnemyPatrol(this);
            enemyMovement = new EnemyMovement(this);
        }
        
        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            //SetNextWaypoint();
            enemyAnimator = GetComponentInChildren<Animator>();

        }
       /// <summary>
       ///  Update is called once per frame
       /// </summary>
        private void Update()
        {
            if (isPatrolling)
            {
                Patrol();
                enemyAnimator.SetBool(IsPatrolling, true);
                // Debug.Log("is patrolling");
            }
            else
            {
                enemyMovement.FollowPlayer();
                enemyAnimator.SetBool(IsPatrolling, false);
                // Debug.Log("follow player");
            }
            
            // Set the rotation while locking the x-axis rotation to 0
            Quaternion targetRotation = Quaternion.Euler(0f, gameObject.transform.rotation.eulerAngles.y, 0f);
            gameObject.transform.rotation = targetRotation;

        }

        void Patrol()
        {
            EnemyPatrol.Patrol();
        }
    }
}
