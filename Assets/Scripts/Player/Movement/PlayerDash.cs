using System.Collections;
using Systems;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerDash : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;

    [Header("References")] 
    [SerializeField] private InputSystem playerInput;

    private Rigidbody2D playersRigidbody;
    private bool isCooled = true;


    private void Start()
    {
        playersRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isCooled && playerInput.Dash != 0)
        {
            StartCoroutine(Dash(playerInput.Dash));
        }    
    }

    private IEnumerator Dash(float direction)
    {
        isCooled = false;
        float gravityScale = playersRigidbody.gravityScale;
        SetGravityScale(0);
        
        playersRigidbody.velocity = new Vector2(direction * dashSpeed, 0f);
        yield return new WaitForSeconds(dashTime);
        
        playersRigidbody.velocity = Vector2.zero;
        SetGravityScale(gravityScale);
        yield return new WaitForSeconds(dashCooldown);
        isCooled = true;
    }

    private void SetGravityScale(float value)
    {
        playersRigidbody.gravityScale = value;
    }
}
