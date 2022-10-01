using Systems;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float jumpForce;
    
    [Header("References"), Space(10)]
    [SerializeField] private InputSystem inputSystem;

    private Rigidbody2D playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        if (Mathf.Abs(inputSystem.Movement.x) > 0.05f)
        {
            Move(inputSystem.Movement.x);
        }
    }

    private void Move(float HorizontalMove)
    {
        SetVelocity(speedCurve.Evaluate(HorizontalMove), playerRigidbody.velocity.y);
    }
    
    private void HandleJump()
    {
        if (inputSystem.Movement.y > 0.01f)
        {
            Jump();
        }
    }
    
    private void Jump()
    {
        SetVelocity(playerRigidbody.velocity.x, jumpForce);
    }

    private void SetVelocity(float x, float y)
    {
        playerRigidbody.velocity = new Vector2(x, y);
    }
}
