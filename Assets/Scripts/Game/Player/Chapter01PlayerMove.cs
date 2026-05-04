using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Chapter01PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 11f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float moveInput;
    private bool jumpRequested;
    private bool isGrounded;

    private void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundCheck = transform.Find("GroundCheck");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (groundCheck == null)
        {
            groundCheck = transform.Find("GroundCheck");
        }

        if (rb == null)
        {
            Debug.LogError("[Chapter01PlayerMove] Rigidbody2D is required.", this);
            enabled = false;
            return;
        }

        if (groundCheck == null)
        {
            Debug.LogError("[Chapter01PlayerMove] GroundCheck child object is required.", this);
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }

        if (spriteRenderer != null)
        {
            if (moveInput > 0.01f)
            {
                spriteRenderer.flipX = false;
            }
            else if (moveInput < -0.01f)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;

        if (jumpRequested)
        {
            if (isGrounded)
            {
                velocity.y = jumpForce;
            }

            jumpRequested = false;
        }

        rb.linearVelocity = velocity;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
