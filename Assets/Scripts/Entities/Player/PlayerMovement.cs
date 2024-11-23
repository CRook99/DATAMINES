using System.Collections;
using UnityEngine;


namespace Entities.Player
{
    public enum Direction
    {
        LEFT,
        RIGHT
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private const float RespawnTime = 1f;
        
        [Header("Movement")]
        public float moveSpeed = 10f;

        [Header("Jumping")]
        public float jumpForce = 15f;
        public float jumpBufferTime = 0.2f;
        public float coyoteTime = 0.2f;

        [Header("Jump Height Control")]
        public float jumpCutMultiplier = 0.5f;

        [Header("Ground Detection")]
        public LayerMask groundLayer;
        public float groundCheckRadius = 0.2f;
        public Transform groundCheck;

        public delegate void SideSwitchHandler(Direction direction);

        public event SideSwitchHandler OnSideSwitch;
        
        
        private Rigidbody2D _rb;
        private bool _isGrounded;
        private float _jumpBufferCounter;
        private float _coyoteTimeCounter;
        private bool _jumpHeld;
        private Vector3 _lastGroundedPosition;
        private bool _canMove = true;
        private Direction _faceDirection;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _faceDirection = Direction.RIGHT;
        }

        void Update()
        {
            if (!_canMove) return;
            
            HandleInput();
            CheckGround();
            ApplyCoyoteTime();
            ApplyJumpBuffer();
        }

        void FixedUpdate()
        {
            if (!_canMove) return;
            
            Move();
            HandleJump();
        }

        private void HandleInput()
        {
            if (Input.GetButtonDown("Jump"))
                _jumpBufferCounter = jumpBufferTime;
            if (Input.GetButtonUp("Jump"))
                _jumpHeld = false;
            if (Input.GetButton("Jump"))
                _jumpHeld = true;
        }

        private void CheckGround()
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (_isGrounded)
            {
                _coyoteTimeCounter = coyoteTime;
                if (_canMove)
                    _lastGroundedPosition = transform.position;
            }
        }

        private void ApplyCoyoteTime()
        {
            if (!_isGrounded)
                _coyoteTimeCounter -= Time.deltaTime;
        }

        private void ApplyJumpBuffer()
        {
            if (_jumpBufferCounter > 0)
                _jumpBufferCounter -= Time.deltaTime;
        }

        private void Move()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) > 0)
            {
                _faceDirection = horizontal > 0 ? Direction.RIGHT : Direction.LEFT;
                OnSideSwitch?.Invoke(_faceDirection);
            }
            _rb.velocity = new Vector2(horizontal * moveSpeed, _rb.velocity.y);
        }

        private void HandleJump()
        {
            if (_jumpBufferCounter > 0 && _coyoteTimeCounter > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _jumpBufferCounter = 0;
            }
            
            if (!_jumpHeld && _rb.velocity.y > 0)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * jumpCutMultiplier);
            }
        }

        // When hitting spike
        public void Respawn()
        {
            ToggleMovement(false);
            StartCoroutine(WaitForRespawn());
            
            IEnumerator WaitForRespawn()
            {
                yield return new WaitForSeconds(RespawnTime);
                ToggleMovement(true);
                transform.position = _lastGroundedPosition;
            }
        }

        public void ToggleMovement(bool toggle)
        {
            _canMove = toggle;
        }

        private void OnDrawGizmosSelected()
        {
            // Draw ground check sphere in editor
            if (groundCheck != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            }
        }
    }
}
