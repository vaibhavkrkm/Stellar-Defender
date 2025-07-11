using UnityEngine;

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

    private void Start()
    {
        acceleration = playerStats.acceleration;
        maxSpeed = playerStats.maxSpeed;
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

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
}
