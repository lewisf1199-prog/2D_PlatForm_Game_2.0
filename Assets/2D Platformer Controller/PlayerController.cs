using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float acceleration = 1;
    public float decelerationMultiplier = 1;
    public float sprintSpeedMultiplier = 2;
    public float jumpSpeed = 1;
    public int numberOfAirJumps = 1;
    public float maxJumpTime = 0.1f;
    public LayerMask groundLayers;

    private Rigidbody2D rb;
    private CapsuleCollider2D collider;

    private Vector2 moveInput;
    private bool sprintInput;
    private bool jumpInput;
    private bool jumpPressed;
    private bool jumpReleased;

    private bool isWalledRight;
    private bool isWalledLeft;
    private bool isGrounded;
    private bool isWallJumping;

    private float gravityScale;
    private bool canJump;
    private int jumpsLeft;
    private float xVelocity;
    private float timeInJump;

    #region Unity Messages
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        gravityScale = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        // Reset jumping if on ground
        if (isGrounded)
        {
            ResetJumps();
            isWallJumping = false;
        }

        // UPDATED LINE: Only wall-hang when in the air (!isGrounded)
        if ((isWalledLeft || isWalledRight) && !isGrounded)
        {
            HandleWallHangFixed();
        }
        else if (jumpInput || jumpReleased)
        {
            HandleJumpFixed();
        }

        // Horizontal movement
        if (!isWallJumping)
        {
            if (moveInput.x == 0 || (Mathf.Abs(rb.linearVelocityX) > 0.1f && Mathf.Sign(moveInput.x) != Mathf.Sign(rb.linearVelocityX)) && isGrounded)
            {
                if (isGrounded)
                {
                    DecelerateFixed();
                }
            }
            else
            {
                MoveFixed(moveInput.x);
            }
        }

        jumpPressed = false;
        jumpReleased = false;
    }
    #endregion


    #region Input Messages
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        jumpInput = value.Get<float>() > 0;

        if (jumpInput)
            jumpPressed = true;
        else
            jumpReleased = true;
    }

    private void OnSprint(InputValue value)
    {
        sprintInput = value.Get<float>() > 0;
    }
    #endregion

    public void OnWallTriggerEnter(Direction direction)
    {
        isWallJumping = false;

        switch (direction)
        {
            case Direction.Left:
                isWalledLeft = true;
                break;

            case Direction.Right:
                isWalledRight = true;
                break;

            default:
                break;
        }
    }

    public void OnWallTriggerExit(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                isWalledLeft = false;
                rb.gravityScale = gravityScale;
                break;

            case Direction.Right:
                isWalledRight = false;
                rb.gravityScale = gravityScale;
                break;

            default:
                break;
        }
    }

    private void MoveFixed(float direction)
    {
        direction = Mathf.Sign(direction);

        rb.AddForce(new Vector2(direction * acceleration * (sprintInput ? sprintSpeedMultiplier : 1), 0));
        float maxSpeed = sprintInput || !isGrounded ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
        rb.linearVelocityX = Mathf.Clamp(rb.linearVelocityX, -maxSpeed, maxSpeed);
    }

    private void SetMovement(float direction)
    {
        direction = Mathf.Sign(direction);

        rb.linearVelocityX = direction * moveSpeed * (sprintInput ? sprintSpeedMultiplier : 1);
    }

    private void DecelerateFixed()
    {
        float sign = Mathf.Sign(rb.linearVelocityX);
        float decelAmount = rb.linearVelocityX * (1 - decelerationMultiplier);
        rb.linearVelocityX += decelAmount * Time.fixedDeltaTime;
        if (Mathf.Sign(rb.linearVelocityX) != sign)
            rb.linearVelocityX = 0;
    }

    private void Jump()
    {
        rb.linearVelocityY = jumpSpeed;
        timeInJump += Time.fixedDeltaTime;
    }

    private void ResetJumps()
    {
        timeInJump = 0;
        jumpsLeft = numberOfAirJumps;
    }

    private void HandleWallHangFixed()
    {
        if (isGrounded)
            return;

        if ((isWalledLeft && moveInput.x < 0) || (isWalledRight && moveInput.x > 0))
        {
            rb.gravityScale = 0;
            rb.linearVelocityY = 0;
            ResetJumps();

            if (jumpPressed)
            {
                isWallJumping = true;
                Jump();
                SetMovement(-moveInput.x);
            }
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void HandleJumpFixed()
    {
        if (jumpReleased)
        {
            timeInJump = maxJumpTime;
        }

        if (jumpPressed)
        {
            if (isGrounded)
            {
                Jump();
                timeInJump = 0;
            }
            else if (jumpsLeft > 0)
            {
                Jump();
                jumpsLeft--;
                timeInJump = 0;
                isWallJumping = false;
            }
        }
        else
        {
            if (timeInJump < maxJumpTime)
            {
                Jump();
            }
        }
    }

    private void CheckGrounded()
    {
        Vector2 rayStart = collider.bounds.center;
        float rayRadius = (collider.size.x / 2 * transform.localScale.x) * 0.95f;
        float rayLength = 0.55f * (collider.size.y / 2) * transform.localScale.y;
        RaycastHit2D hit = Physics2D.CircleCast(rayStart, rayRadius, Vector2.down,
            rayLength, groundLayers);
        isGrounded = hit.collider != null;
    }
}

public enum Direction
{
    Left, Right, Up, Down
}