using UnityEngine;

public class BulletGuidedMovement : MonoBehaviour
{
    public int moveSpeed;
    public int followDuration;    // follow time in seconds towards the player
    public float rotationSpeed;

    [SerializeField] private Transform spriteTransform;
    private Vector2 velocity = Vector2.zero;
    private Vector2 playerPos;
    private float followTimer = 0f;

    private void Update()
    {
        if (followTimer < followDuration)
        {
            followTimer += Time.deltaTime;    // updating the timer

            playerPos = GameObject.Find("Player").transform.position;
            // updating velocity to the direction of the player
            UpdateVelocity(playerPos);
        }

        // moving the bullet
        transform.Translate(velocity * Time.deltaTime);
        // rotating the bullet sprite
        if (rotationSpeed != 0f)
        {
            spriteTransform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
        }
    }

    private Vector2 DirectionTo(Vector2 first, Vector2 second)
    {
        Vector2 direction = (second - first).normalized;  // normalizing the resultant vector to get direction
        return direction;
    }

    public void UpdateVelocity(Vector2 playerPos)
    {
        Vector2 direction = DirectionTo(transform.position, playerPos); // direction vector to player
        velocity = direction * moveSpeed;  // actual velocity calculation
    }
}
