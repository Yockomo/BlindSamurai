using Systems;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    
    [Space(8)]
    [SerializeField] private float jumpForce;

    [Space(8)] 
    [SerializeField] private float fallGravityMultiplaier;

    [Header("References"),]
    [SerializeField] private InputSystem inputSystem;

    private Rigidbody2D playerRigidbody;
    private float gravityScale;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        gravityScale = playerRigidbody.gravityScale;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
        HandleJumpGravity();
    }

    private void HandleMovement()
    {
        if (Mathf.Abs(inputSystem.Movement.x) > 0.05f)
        {
            float targetSpeed = inputSystem.Movement.x * moveSpeed; 
            var speedDif = targetSpeed - playerRigidbody.velocity.x;
            var accelRate = Mathf.Abs(inputSystem.Movement.x) > 0.01f ? acceleration: decceleration;
            var movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) *Mathf.Sign(speedDif);
           
            playerRigidbody.AddForce(movement*Vector2.right);
        }
    }

    private void HandleJump()
    {
        if (inputSystem.Movement.y > 0.01f && inputSystem.Grounded)
        {
            Debug.Log(Vector2.up*jumpForce);
            playerRigidbody.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
        }
    }

    private void HandleJumpGravity()
    {
        if (playerRigidbody.velocity.y < 0)
        {
            playerRigidbody.gravityScale = gravityScale * fallGravityMultiplaier;
        }
        else
        {
            playerRigidbody.gravityScale = gravityScale;
        }
    }
}
