using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
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
        // Buffer jump input
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        if (Input.GetButtonUp("Jump"))
        {
            _jumpHeld = false;
        }

        // Track if jump is held
        if (Input.GetButton("Jump"))
        {
            _jumpHeld = true;
        }
    }

    private void CheckGround()
    {
        // Check if player is on the ground
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset coyote time counter if grounded
        if (_isGrounded)
        {
            _coyoteTimeCounter = coyoteTime;
        }
    }

    private void ApplyCoyoteTime()
    {
        // Decrease coyote time if in the air
        if (!_isGrounded)
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void ApplyJumpBuffer()
    {
        // Decrease jump buffer timer
        if (_jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
    }

    private void Move()
    {
        // Get horizontal input and apply movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(horizontal * moveSpeed, _rb.velocity.y);
    }

    private void HandleJump()
    {
        // Perform jump if jump buffer and coyote time are valid
        if (_jumpBufferCounter > 0 && _coyoteTimeCounter > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            _jumpBufferCounter = 0; // Reset jump buffer after jump
        }

        // Apply jump height control if jump is released
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
