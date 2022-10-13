using System;
using System.Collections;
using Interfaces;
using ScriptableObjects;
using Stats;
using Systems;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerStates : MonoBehaviour, IHaveFightState
    {
		[Header("Checks")]
		[SerializeField] private Transform _groundCheckPoint;
		[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
		[Space(5)]
		[SerializeField] private Transform _frontWallCheckPoint;
		[SerializeField] private Transform _backWallCheckPoint;
		[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

		[Header("Layers & Tags")]
		[SerializeField] private LayerMask _groundLayer;
		
		[Header("References")]
		[SerializeField] private InputSystem inputSystem;
		[SerializeField] private PlayerMoveData moveData;
		[SerializeField] private PlayerEnergyData energyData;

		public bool IsFacingRight { get; private set; }
		public bool IsJumping { get; private set; }
		public bool IsWallJumping { get; private set; }
		public bool IsSliding { get; private set; }

		public float LastOnGroundTime { get; private set; }
		public float LastOnWallTime { get; private set; }
		public float LastOnWallRightTime { get; private set; }
		public float LastOnWallLeftTime { get; private set; }

		//Jump
		public float LastPressedJumpTime { get; private set; }

		public bool IsJumpCut { get; private set; }
		public bool IsJumpFalling { get; private set; }

		public float MovementDirection { get; private set; }

		//Wall Jump
		public float WallJumpStartTime { get; private set; }
		public int LastWallJumpDir { get; private set; }
		
		//Dash
		public bool IsDashing { get; private set; }
		public float DashDirection { get; private set; }

		// Energy use in fight
		public bool IsFighting {get; private set; }
		public bool fighting;
		public bool Inactive;

		public event Action OnJumpStateStart;
		public event Action<int> OnWallJumpStateStart;

		private Rigidbody2D rigidBody;
		private Energy energy;
		private bool isDashCooled = true;

		[Inject]
		private void Construct(Energy energy)
		{
			rigidBody = GetComponent<Rigidbody2D>();
			this.energy = energy;
			IsFacingRight = true;
		}

		public void SetFighState(bool stateValue)
		{
			IsFighting = stateValue;
			fighting = stateValue;
		}
		
		private void Update()
        {
			UpdateTimers();
			HandleInputs();
			CheckCollisions();
			CheckJumps();
			CheckSlide();
        }

		private void UpdateTimers()
        {
			LastOnGroundTime -= Time.deltaTime;
			LastOnWallTime -= Time.deltaTime;
			LastOnWallRightTime -= Time.deltaTime;
			LastOnWallLeftTime -= Time.deltaTime;
			LastPressedJumpTime -= Time.deltaTime;
		}

		private void HandleInputs()
        {
			MovementDirection = inputSystem.Movement.x;

			if (MovementDirection != 0)
            {
				CheckDirectionToFace(MovementDirection > 0);
			}

			if (inputSystem.Dash != 0)
			{
				OnDashInput();
			}
			else if (inputSystem.Jump)
			{
				OnJumpInput();
				OnJumpUpInput();
			}

			HandleInactiveState();
        }

		private void CheckCollisions()
        {
			if (!IsJumping)
			{
				//Ground Check
				if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
				{
					LastOnGroundTime = moveData.coyoteTime; //if so sets the lastGrounded to coyoteTime
				}

				//Right Wall Check
				if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
						|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
					LastOnWallRightTime = moveData.coyoteTime;

				//Right Wall Check
				if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
					|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
					LastOnWallLeftTime = moveData.coyoteTime;

				//Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
				LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
			}
		}

		private void CheckJumps()
        {
			if (IsJumping && rigidBody.velocity.y < 0)
			{
				IsJumping = false;

				if (!IsWallJumping)
					IsJumpFalling = true;
			}

			if (IsWallJumping && Time.time - WallJumpStartTime > moveData.wallJumpTime)
			{
				IsWallJumping = false;
			}

			if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
			{
				IsJumpCut = false;
				IsJumpFalling = false;
			}

			//Jump
			if (CanJump())
			{
				IsJumping = true;
				IsWallJumping = false;
				IsJumpCut = false;
				IsJumpFalling = false;

				LastPressedJumpTime = 0;
				LastOnGroundTime = 0;

				OnJumpStateStart?.Invoke();
			}
			//WALL JUMP
			else if (CanWallJump())
			{
				IsWallJumping = true;
				IsJumping = false;
				IsJumpCut = false;
				IsJumpFalling = false;
				WallJumpStartTime = Time.time;
				LastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

				LastPressedJumpTime = 0;
				LastOnGroundTime = 0;
				LastOnWallRightTime = 0;
				LastOnWallLeftTime = 0;

				OnWallJumpStateStart?.Invoke(LastWallJumpDir);
			}
		}

		private void CheckSlide()
		{
			if (CanSlide() && ((LastOnWallLeftTime > 0 && MovementDirection < 0) || (LastOnWallRightTime > 0 && MovementDirection > 0)))
				IsSliding = true;
			else
				IsSliding = false;
		}
		
		private void HandleInactiveState()
		{
			Inactive = !(IsDashing || IsJumping || IsWallJumping);
		}
		
		#region INPUT CALLBACKS
		private void OnJumpInput()
		{
			LastPressedJumpTime = moveData.jumpInputBufferTime;
		}

		private void OnJumpUpInput()
		{
			if (CanJumpCut() || CanWallJumpCut())
				IsJumpCut = true;
		}

		private void OnDashInput()
        {
	        if (isDashCooled)
            {
	            if (IsFighting)
	            {
		            var inAir = LastOnGroundTime > 0;
		            var dashCost = inAir ? energyData.dashCost : energyData.airDashCost;
		            if (!energy.TryUseEnergy(dashCost))
			            return;
	            }
		            
	            DashDirection = inputSystem.Dash;
	            StartCoroutine(DashCooldownAsync());
            }
        }

		private IEnumerator DashCooldownAsync()
        {
			isDashCooled = false;
			IsDashing = true;
			yield return new WaitForSeconds(moveData.dashTime);
			IsDashing = false;
			yield return new WaitForSeconds(moveData.dashCooldown);
			isDashCooled = true;
        }
		#endregion

		#region CHECK METHODS
		private bool CanJump()
		{
			var defaultCheck = LastOnGroundTime > 0 && !IsJumping && LastPressedJumpTime > 0;
			
			return IsFighting ? (defaultCheck && energy.TryUseEnergy(energyData.defaultJumpCost)) : defaultCheck;
		}

		private bool CanWallJump()
		{
			var defaultCheck = LastPressedJumpTime > 0
			                   && LastOnWallTime > 0
			                   && LastOnGroundTime <= 0 
			                   && (!IsWallJumping ||
			                       (LastOnWallRightTime > 0 && LastWallJumpDir == 1) ||
			                       (LastOnWallLeftTime > 0 && LastWallJumpDir == -1));
			
			return  IsFighting ? (defaultCheck && energy.TryUseEnergy(energyData.wallJumpCost)) : defaultCheck;
		}

		private bool CanJumpCut()
		{
			return IsJumping && rigidBody.velocity.y > 0;
		}

		private bool CanWallJumpCut()
		{
			return IsWallJumping && rigidBody.velocity.y > 0;
		}

		public bool CanSlide()
		{
			return LastOnWallTime > 0 && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0;
		}

		private void CheckDirectionToFace(bool isMovingRight)
		{
			if (isMovingRight != IsFacingRight)
				Turn();
		}
		#endregion

		private void Turn()
		{
			//stores scale and flips the player along the x axis, 
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;

			IsFacingRight = !IsFacingRight;
		}

		#region EDITOR METHODS
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
			Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
		}
		#endregion
    }
}
