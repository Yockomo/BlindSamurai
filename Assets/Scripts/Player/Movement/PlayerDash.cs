using Player;
using UnityEngine;

internal class PlayerDash
{
        private enum DashState { Waiting, Dashing,}

        private Rigidbody2D rigidbody;
        private PlayerMoveData dashData;
        private PlayerStates playerStates;

        private DashState dashState;
        private Vector2 savedVelocity;

        public PlayerDash(Rigidbody2D _rigidBody, PlayerMoveData _dashData, PlayerStates _playerStates)
        {
            rigidbody = _rigidBody;
            dashData = _dashData;
            playerStates = _playerStates;
            
            dashState = DashState.Waiting;
        }

        public void Tick()
        {
            switch (dashState)
            {
                case DashState.Waiting:
                    if (playerStates.IsDashing)
                    {
                        savedVelocity = new Vector2(rigidbody.velocity.x, 0);
                        rigidbody.velocity = Vector2.zero;
                        dashState = DashState.Dashing;
                    }
                    break;

                case DashState.Dashing:
                    rigidbody.AddForce( new Vector2(playerStates.DashDirection * dashData.dashSpeed, 0));
                    if (!playerStates.IsDashing)
                    {
                        rigidbody.velocity = savedVelocity;
                        dashState = DashState.Waiting;
                    }
                    break;
            }
        }
}