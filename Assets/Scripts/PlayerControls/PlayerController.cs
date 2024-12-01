using Audio;
using Levels;
using Managers;
using Melee_System;
using Powerups;
using UnityEngine;

namespace PlayerControls
{
    /// <summary>
    /// Manages the player's movement and actions.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Header("Gravity")] 
        [SerializeField] private float gravity = 9.81f;

        [Header("Turning Speeds")] 
        [SerializeField] private float turnSpeed = 10f;
        [SerializeField] private float aimTurnSpeed = 30f;

        private bool _isCrouching, _isWalking, _isRunning, _jumpSoundPlayed, _isAiming, _onPlatform, _isSpinning;
        
        private bool gravityApplied;

        private CharacterController _playerCharacterController;

        // Pause flags
        public bool canMove = true;
        private readonly bool _isPaused = false;


        // Directional
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _targetDir = Vector3.zero;
        private Vector2 _input;
        private Quaternion _freeRotation;
        private float _movementDirectionY;

        // Movement
        private float _horizontal, _vertical, _move, _moveSpeed;

        // Jump counter
        private int _jumpCount = 1;
        private Animator _animator;
        private bool justJumped;

        /// <summary>
        /// Subscribe to the relevant events
        /// Get the CharacterController component
        /// Get the Animator component
        /// Lock the cursor
        /// </summary>
        private void Start()
        {

            PlatformCollider.PlayerOnPlatform += PlayerEnterPlatform;
            PlatformCollider.PlayerOffPlatform += PlayerExitPlatform;

            BouncyBin.BinHitEvent += OnBinHit;
            Ladder.LadderClimbEvent += OnLadderClimb;
            SpinAttack.SpinAttackEvent += OnSpinAttack;

            _playerCharacterController = gameObject.GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            _animator = GetComponentInChildren<Animator>();
        }


        /// <summary>
        /// Unsubcribe from the relevant events
        /// </summary>
        private void OnDestroy()
        {
            PlatformCollider.PlayerOnPlatform -= PlayerEnterPlatform;
            PlatformCollider.PlayerOffPlatform -= PlayerExitPlatform;

            BouncyBin.BinHitEvent -= OnBinHit;
            Ladder.LadderClimbEvent -= OnLadderClimb;
            SpinAttack.SpinAttackEvent -= OnSpinAttack;
        }


        /// <summary>
        /// Update the player's movement and actions by calling all the relevant methods if the game is not paused.
        /// </summary>
        private void Update()
        {
            // Check if the game is not paused before updating
            if (!_isPaused && canMove) 
            {
                PlayerMovement();
                PlayerJumping();
                PlayerCrouch();
                PlayerCrouchJump();
                PlayerTargetDirection();
                AimingRotation();
                RotationTowardsTargetDirection();
                MovementSounds();
                CheckHeadBang();
            }

            if (_moveDirection.y < -30)
            {
                
            }
            
            
            // Movement - must be in Update method
            _playerCharacterController.Move(_moveDirection * Time.deltaTime);

            if (GameManager.Instance.isTommyGunEnabled || GameManager.Instance.isPistolEnabled)
            {
                _isAiming = Input.GetKey(KeyCode.Mouse1);
            }
            else
            {
                _isAiming = false;
            }
        }

        /// <summary>
        /// If the gun is enabled, rotate the player to face the center of the screen when aiming.
        /// </summary>
        private void AimingRotation()
        {
            // Rotate the player to the center of the screen if they shoot or punch
            if ((_isAiming && Input.GetKey(KeyCode.Mouse1)) ||
                (!GameManager.Instance.isPistolEnabled && Input.GetKey(KeyCode.Mouse0)) ||
                (GameManager.Instance.isPistolEnabled && Input.GetKey(KeyCode.Mouse0)))
            {
                if (Camera.main != null)
                {
                    Vector3 lookDirection = Camera.main.transform.forward;
                    // Ensure no rotation in the vertical axis
                    lookDirection.y = 0f; 
                    _freeRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, _freeRotation, aimTurnSpeed * Time.deltaTime);
                }
            }
        }
        
        /// <summary>
        /// If the player is not aiming, rotate the player towards the target direction.
        /// </summary>
        private void RotationTowardsTargetDirection()
        {
            if (!_isAiming && (_horizontal != 0 || _vertical != 0))
            {
                Vector3 moveDirection = _moveDirection.normalized;
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }
        }
        
        /// <summary>
        /// Handle player movement based on input.
        /// </summary>
        private void PlayerMovement()
        {
            _isRunning = Input.GetKey(KeyCode.LeftShift);

            // Can only sprint when grounded
            if (!_playerCharacterController.isGrounded)
            {
                _isRunning = false;
                _isWalking = false;
            }

            // Set movement speed based on player state
            if (canMove && _isRunning && !_isSpinning)
            {
                _moveSpeed = AttributeManager.Instance.sprintSpeed;
            }
            else if (canMove && _isCrouching && !_isSpinning)
            {
                _moveSpeed = AttributeManager.Instance.crouchWalkSpeed;
            }
            else if (canMove && !_isSpinning)
            {
                _moveSpeed = AttributeManager.Instance.walkSpeed;
            }

            // Check if player is walking
            if (canMove && _moveSpeed == AttributeManager.Instance.walkSpeed && (_horizontal != 0 || _vertical != 0))
            {
                _isWalking = true;
            }
            else
            {
                _isWalking = false;
            }

            _movementDirectionY = _moveDirection.y;

            // Get input for movement
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");

            // Calculate movement direction based on the aiming state
            
            // Use player's forward and right directions
            if (_isAiming) 
            {
                Vector3 moveDirection = transform.forward * _vertical + transform.right * _horizontal;
                moveDirection.Normalize();
                _moveDirection = moveDirection * _moveSpeed;
            }
            // Use forward direction based on camera
            else 
            {
                Vector3 cameraForward = Camera.main.transform.forward;
                // Ensure no rotation in the vertical axis
                cameraForward.y = 0f; 
                Vector3 moveDirection = cameraForward * _vertical + Camera.main.transform.right * _horizontal;
                moveDirection.Normalize();
                _moveDirection = moveDirection * _moveSpeed;
            }

            // Animation handling
            _animator.SetBool("IsMoving", _isWalking || _isRunning);
            _animator.SetFloat("ForwardSpeed", _moveDirection.magnitude);
        }
        
        /// <summary>
        /// Jump logic for the player.
        /// </summary>
        private void PlayerJumping()
        {
            justJumped = false;
            if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.LeftControl) && canMove && _jumpCount > 0)
            {
                _moveDirection.y = AttributeManager.Instance.jumpForce;
                _jumpCount--;
                JumpSound();
                _animator.SetBool("IsJumping", true);
                justJumped = true;
            }
            else
            {
                _moveDirection.y = _movementDirectionY;
            }

            switch (_playerCharacterController.isGrounded)
            {
                case true:
                    if (!justJumped)
                    {
                        _animator.SetBool("IsJumping", false);
                        justJumped = false;
                    }

                    if (gravityApplied && _moveDirection.y < -15) 
                    {
                        _moveDirection.y = AttributeManager.Instance.jumpForce;
                        gravityApplied = false;
                    }
                    _jumpCount = 1;
                    _jumpSoundPlayed = false;
                    break;
                
                // Gravity
                case false:
                    _moveDirection.y -= gravity * Time.deltaTime;
                    gravityApplied = true;
                    break;
            }
        }
        
        /// <summary>
        /// Set the player's state to be on the platform.
        /// </summary>
        private void PlayerEnterPlatform()
        {
            _onPlatform = true;
            Debug.Log("Player is on the platform!");
        }

        /// <summary>
        /// Set the player's state to be off the platform.
        /// </summary>
        private void PlayerExitPlatform()
        {
            _onPlatform = false;
            Debug.Log("Player is off the platform!");
        }

        /// <summary>
        /// Handle player crouching.
        /// If on a platform, the player cannot crouch.
        /// </summary>
        private void PlayerCrouch()
        {
            if (Input.GetKey(KeyCode.LeftControl) && !_onPlatform)
            {
                _isCrouching = true;
                _animator.SetBool("IsCrouching", true);
                gameObject.transform.localScale = new Vector3(1, 0.75f, 1);
            }

            else if (!Input.GetKey(KeyCode.LeftControl) && !_onPlatform)
            {
                _isCrouching = false;
                _animator.SetBool("IsCrouching", false);
                gameObject.transform.localScale = new Vector3(1, 1f, 1);
            }
        }

        /// <summary>
        /// Handle player crouch jumping.
        /// </summary>
        private void PlayerCrouchJump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.LeftControl) && canMove &&
                _playerCharacterController.isGrounded)
            {
                JumpSound();
                _moveDirection.y = AttributeManager.Instance.crouchJumpForce;
            }
        }

        /// <summary>
        /// Get the target direction based on player input.
        /// </summary>
        private void PlayerTargetDirection()
        {
            var forward = Camera.main.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            var right = Camera.main.transform.TransformDirection(Vector3.right);

            _targetDir = _input.x * right + _input.y * forward;
        }

        /// <summary>
        /// Play movement sounds based on player state.
        /// </summary>
        private void MovementSounds()
        {
            if (_isRunning && _playerCharacterController.isGrounded)
            {
                AudioManager.Instance.PlaySound(11);
            }
            else if (!_isRunning)
            {
                AudioManager.Instance.StopSound(11);
            }

            if (_isWalking && _playerCharacterController.isGrounded)
            {
                AudioManager.Instance.PlaySound(8);
            }
            else if (!_isWalking)
            {
                AudioManager.Instance.StopSound(8);
            }
        }

        /// <summary>
        /// Play a jump sound when the player jumps.
        /// </summary>
        private void JumpSound()
        {
            if (!_jumpSoundPlayed)
            {
                AudioManager.Instance.PlaySound(Random.Range(5, 7));
                _jumpSoundPlayed = true;
            }
        }

        /// <summary>
        /// Check if the player is head banging and adjust the movement direction accordingly.
        /// </summary>
        private void CheckHeadBang()
        {
            Vector3 headOffset = new Vector3(0, .55f, 0);
            Vector3 headPosition = transform.position + headOffset;
            var headRadius = .5f;
            var maxHeadCheckDistance = 0.1f;
            if (Physics.SphereCast(headPosition, headRadius, Vector3.up, out RaycastHit hit, maxHeadCheckDistance))
            {
                _moveDirection.y -= gravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// Draw a wire sphere to visualise the head position.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Visualize the sphere cast
            Gizmos.color = Color.yellow;
            Vector3 headOffset = new Vector3(0, .55f, 0);
            Vector3 headPosition = transform.position + headOffset;
            float headRadius = .5f;
            float maxHeadCheckDistance = .1f;
            Gizmos.DrawWireSphere(headPosition, headRadius);
        }

        /// <summary>
        /// Handle the player's movement when they hit a bouncy bin.
        /// </summary>
        /// <param name="bounceForce">force to be applied</param>
        private void OnBinHit(int bounceForce)
        {
            _moveDirection.y = bounceForce;
        }

        /// <summary>
        /// Handle the player's movement when they climb a ladder.
        /// </summary>
        /// <param name="climbSpeed">force to be applied</param>
        private void OnLadderClimb(float climbSpeed)
        {
            _moveDirection.y = climbSpeed;
        }

        /// <summary>
        /// Change the player's movement speed when they are spinning.
        /// </summary>
        /// <param name="isSpinning">bool to manage spinning state</param>
        private void OnSpinAttack(bool isSpinning)
        {
            _isSpinning = isSpinning;
            if (_isSpinning)
            {
                _moveSpeed = AttributeManager.Instance.walkSpeed / 2;
            }
        }
    }
}