using UnityEngine;

namespace Player
{
    internal class GravityController
    {
		private readonly PlayerStates playerStates;
		private readonly PlayerMoveData gravityData;
		private readonly Rigidbody2D rigidBody;

		public GravityController(PlayerStates _playerStates, PlayerMoveData _gravityData, Rigidbody2D _rigidBody)
        {
			playerStates = _playerStates;
			gravityData = _gravityData;
			rigidBody = _rigidBody;
			SetGravityScale(gravityData.gravityScale);
		}

        public void Tick()
        {
			UpdateGravity();
        }

        private void UpdateGravity()
        {
			//Higher gravity if we've released the jump input or are falling
			if (playerStates.IsSliding)
			{
				SetGravityScale(0);
			}
			else if (playerStates.IsJumpCut)
			{
				//Higher gravity if jump button released
				SetGravityScale(gravityData.gravityScale * gravityData.jumpCutGravityMult);
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, - gravityData.maxFallSpeed));
			}
			else if ((playerStates.IsJumping || playerStates.IsWallJumping || playerStates.IsJumpFalling) && Mathf.Abs(rigidBody.velocity.y) < gravityData.jumpHangTimeThreshold)
			{
				SetGravityScale(gravityData.gravityScale * gravityData.jumpHangGravityMult);
			}
			else if (rigidBody.velocity.y < 0)
			{
				//Higher gravity if falling
				SetGravityScale(gravityData.gravityScale * gravityData.fallGravityMult);
				//Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
				rigidBody.velocity = new Vector2(rigidBody.velocity.x, Mathf.Max(rigidBody.velocity.y, - gravityData.maxFallSpeed));
			}
			else
			{
				//Default gravity if standing on a platform or moving upwards
				SetGravityScale(gravityData.gravityScale);
			}
		}

		private void SetGravityScale(float scale)
		{
			rigidBody.gravityScale = scale;
		}
	}
}
