using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;
    public bool isGrounded;

    [Header("Wall Detection")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.1f;
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    public bool isTouchingWall;
    public float moveInput;

    void Start()
    {   
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        // Get Horizontal Input
        moveInput = Input.GetAxis("Horizontal");

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Check for ground and wall collision
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckDistance, wallLayer);

        if (!isTouchingWall || moveInput == 0)
        {
            Vector2 newVelocity = new Vector2(moveInput * moveSpeed, rb.GetLinearVelocity().y);
            rb.SetLinearVelocity(newVelocity);
        }

        
    }

    void Jump()
    {
        Vector2 newVelocity = new Vector2(rb.GetLinearVelocity().x, jumpForce);
        rb.SetLinearVelocity(newVelocity);
    }

    bool CheckTagCollision(Transform checkPoint, string tagToCheck)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPoint.position, groundCheckDistance, groundLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(tagToCheck))
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x - wallCheckDistance, wallCheck.position.y));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        }
    }
}
