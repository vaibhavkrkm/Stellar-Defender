using System.Linq;
using UnityEngine;

public class PlayerBulletGuidedMovement : MonoBehaviour
{
    public int moveSpeed;
    public float rotationSpeed;
    [SerializeField] private Transform spriteTransform;

    private Vector2 velocity = Vector2.zero;
    private GameObject nearestTarget;
    private Vector2 direction = Vector2.up;

    private void Start()
    {
        FindNearestTarget();
    }

    private void Update()
    {
        // Find the nearest target again in every frame
        FindNearestTarget();

        if (nearestTarget != null) // Follow the target if it exists
        {
            direction = DirectionTo(transform.position, nearestTarget.transform.position);
        }

        // updating the velocity according to the direction
        UpdateVelocity(direction);

        // moving the bullet according to the velocity
        transform.Translate(velocity * Time.deltaTime);

        // rotating the bullet sprite
        if (rotationSpeed != 0f)
        {
            spriteTransform.Rotate(rotationSpeed * Time.deltaTime * Vector3.forward);
        }
    }

    // function to find the nearest target among all the enemies
    private void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");  // array with all targets

        if (targets.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            foreach (var target in targets)
            {
                float distance = Vector3.Distance(target.transform.position, transform.position);
                if (distance < shortestDistance)
                {
                    // if the current distance is lesser, updating shortestDistance and nearestTarget accordingly
                    shortestDistance = distance;
                    nearestTarget = target;
                }
            }
        }
        else
        {
            nearestTarget = null;    // no targets found so setting it to null
        }
    }

    private Vector2 DirectionTo(Vector2 first, Vector2 second)
    {
        Vector2 direction = (second - first).normalized;
        return direction;
    }

    public void UpdateVelocity(Vector2 direction)
    {
        velocity = direction * moveSpeed;  // actual velocity calculation
    }
}
