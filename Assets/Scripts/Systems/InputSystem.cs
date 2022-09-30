using UnityEngine;

namespace Systems
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] private KeyCode fire1Key = KeyCode.Mouse0;
        [SerializeField] private KeyCode fire2Key = KeyCode.Mouse1;
        [SerializeField] private KeyCode grenadeKey = KeyCode.F;
        [SerializeField] private KeyCode healKey = KeyCode.V;

        public Vector2 Movement {get; private set;}
        public float Dash { get; private set; }
        
        public bool Fire1 { get; private set; }
        public bool Fire2 { get; private set; }
        public bool Grenade { get; private set; }
        public bool Heal { get; private set; }
        
        private void Update()
        {
            HandleMovementInputs();
            HandleActionInputs();
        }

        private void HandleMovementInputs()
        {
            MovementInputs();
            DashInputs();
        }

        private void MovementInputs()
        {
            var vertical = Input.GetAxis(GlobalAxis.VerticalAxis);
            var horizontal = Input.GetAxis(GlobalAxis.HorizontalAxis);

            Movement = new Vector2(vertical, horizontal);
        }

        private void DashInputs()
        {
            if (Input.GetKeyDown(KeyCode.E))
                Dash = 1;
            else if (Input.GetKeyDown(KeyCode.Q))
                Dash = -1;
        }

        private void HandleActionInputs()
        {
            Fire1 = Input.GetKeyDown(fire1Key);
            Fire2 = Input.GetKeyDown(fire2Key);
            Grenade = Input.GetKeyDown(grenadeKey);
            Heal = Input.GetKeyDown(healKey);
        }
    }
}