using Player;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
	[SerializeField] private PlayerMoveData moveData;
	[SerializeField] private PlayerStates playerStates;

	private Rigidbody2D rigidBody;

	private GravityController gravityController;
	private PlayerMove playerMove;
	private PlayerDash playerDash;

    private void OnEnable()
    {
        playerStates.OnJumpStateStart += playerMove.Jump;
        playerStates.OnWallJumpStateStart += playerMove.WallJump;
    }

    private void OnDisable()
    {
        playerStates.OnJumpStateStart -= playerMove.Jump;
        playerStates.OnWallJumpStateStart -= playerMove.WallJump;
    }

    private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		gravityController = new GravityController(playerStates, moveData, rigidBody);
		playerMove = new PlayerMove(moveData, playerStates, rigidBody);
		playerDash = new PlayerDash(rigidBody, moveData, playerStates);
	}

	private void Update()
	{
		gravityController.Tick();
	}

    private void FixedUpdate()
	{
		playerDash.Tick();

		if (playerStates.IsWallJumping)
			playerMove.Run(moveData.wallJumpRunLerp);
		else
			playerMove.Run(1);

		if (playerStates.IsSliding)
			playerMove.Slide();
    }
}