using UnityEngine;

namespace Player
{
    internal class PlayerMove
    {
		private readonly PlayerStates playerStates;
		private readonly PlayerMoveData moveData;
		private readonly Rigidbody2D rigidBody;

		public PlayerMove(PlayerMoveData _moveData, PlayerStates _playerStates, Rigidbody2D _rigidBody)
        {
			moveData = _moveData;
			playerStates = _playerStates;
			rigidBody = _rigidBody;
		}

		public void Tick()
		{
			if (playerStates.IsWallJumping)
				Run(moveData.wallJumpRunLerp);
			else
				Run(1);
			
			if (playerStates.IsSliding)
				Slide();
		}
		
		private void Run(float lerpAmount)
		{
			//Calculate the direction we want to move in and our desired velocity
			float targetSpeed = playerStates.MovementDirection * moveData.runMaxSpeed;
			//We can reduce are control using Lerp() this smooths changes to are direction and speed
			targetSpeed = Mathf.Lerp(rigidBody.velocity.x, targetSpeed, lerpAmount);

			#region Calculate AccelRate
			float accelRate;

			//Gets an acceleration value based on if we are accelerating (includes turning) 
			//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
			if (playerStates.LastOnGroundTime > 0)
				accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? moveData.runAccelAmount : moveData.runDeccelAmount;
			else
				accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? moveData.runAccelAmount * moveData.accelInAir : moveData.runDeccelAmount * moveData.deccelInAir;
			#endregion

			#region Add Bonus Jump Apex Acceleration
			//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
			if ((playerStates.IsJumping || playerStates.IsWallJumping || playerStates.IsJumpFalling) && Mathf.Abs(rigidBody.velocity.y) < moveData.jumpHangTimeThreshold)
			{
				accelRate *= moveData.jumpHangAccelerationMult;
				targetSpeed *= moveData.jumpHangMaxSpeedMult;
			}
			#endregion

			#region Conserve Momentum
			//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
			if (moveData.doConserveMomentum && Mathf.Abs(rigidBody.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rigidBody.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && playerStates.LastOnGroundTime < 0)
			{
				//Prevent any deceleration from happening, or in other words conserve are current momentum
				//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
				accelRate = 0;
			}
			#endregion

			//Calculate difference between current velocity and desired velocity
			float speedDif = targetSpeed - rigidBody.velocity.x;
			//Calculate force along x-axis to apply to thr player

			float movement = speedDif * accelRate;

			//Convert this to a vector and apply to rigidbody
			rigidBody.AddForce(movement * Vector2.right, ForceMode2D.Force);

			/*
			 * For those interested here is what AddForce() will do
			 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
			 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
			*/
		}

		public void Jump()
		{
			//We increase the force applied if we are falling
			//This means we'll always feel like we jump the same amount 
			//(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
			float force = moveData.jumpForce;
			if (rigidBody.velocity.y < 0)
				force -= rigidBody.velocity.y;

			rigidBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		}

		public void WallJump(int dir)
		{
			Vector2 force = new Vector2(moveData.wallJumpForce.x, moveData.wallJumpForce.y);
			force.x *= dir; //apply force in opposite direction of wall

			if (Mathf.Sign(rigidBody.velocity.x) != Mathf.Sign(force.x))
				force.x -= rigidBody.velocity.x;

			if (rigidBody.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
				force.y -= rigidBody.velocity.y;

			//Unlike in the run we want to use the Impulse mode.
			//The default mode will apply are force instantly ignoring masss
			rigidBody.AddForce(force, ForceMode2D.Impulse);
		}

		private void Slide()
		{
			//Works the same as the Run but only in the y-axis
			//THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
			float speedDif = moveData.slideSpeed - rigidBody.velocity.y;
			float movement = speedDif * moveData.slideAccel;
			//So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
			//The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
			movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

			rigidBody.AddForce(movement * Vector2.up);
		}
	}
}
