using System.Collections;
using System.Collections.Generic;
using Systems;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputSystem inputSystem;
    
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (inputSystem.Movement.x != 0)
        {
            
        }
    }
}
