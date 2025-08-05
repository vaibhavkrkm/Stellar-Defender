using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;
    public PlayerStats playerStats;

    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite idleSprite;

    private float acceleration;
    private float maxSpeed;

    private Vector2 moveInput;

    private void Start()
    {
        acceleration = playerStats.acceleration;
        maxSpeed = playerStats.maxSpeed;
    }

    private void FixedUpdate()
    {
        float horizontalInput = moveInput.x;

        body.AddForce(Vector2.right * horizontalInput * acceleration);
        if (body.linearVelocity.magnitude > maxSpeed)
        {
            body.linearVelocity = body.linearVelocity.normalized * maxSpeed;
        }

        if (horizontalInput < -0.4)
        {
            spriteRenderer.sprite = leftSprite;
        }
        else if (horizontalInput > 0.4)
        {
            spriteRenderer.sprite = rightSprite;
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    public void MovementPerformed(InputAction.CallbackContext context)
    {
        // read input values from user when movement keys are pressed
        moveInput = context.ReadValue<Vector2>();
    }
}
