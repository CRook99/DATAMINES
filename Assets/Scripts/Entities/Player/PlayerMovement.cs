using UnityEngine;


namespace Entities.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
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

        private Rigidbody2D _rb;
        private bool _isGrounded;
        private float _jumpBufferCounter;
        private float _coyoteTimeCounter;
        private bool _jumpHeld;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            HandleInput();
            CheckGround();
            ApplyCoyoteTime();
            ApplyJumpBuffer();
        }

        void FixedUpdate()
        {
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
                _coyoteTimeCounter = coyoteTime;
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
