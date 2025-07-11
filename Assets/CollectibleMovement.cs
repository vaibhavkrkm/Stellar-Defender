using UnityEngine;

public class CollectibleMovement : MonoBehaviour
{
    public int moveSpeed;
    public CollectibleCollisions collectibleCollisions;

    private void Update()
    {
        if (collectibleCollisions.isAttracted) return;   // don't do anything if attracted towards the player

        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);

        if (IsOutsideLimits())
        {
            Destroy(gameObject);
        }
    }

    private bool IsOutsideLimits()
    {
        float leftLimit = -18f;
        float rightLimit = 18f;
        float bottomLimit = -10f;

        if (transform.position.x < leftLimit
            || transform.position.x > rightLimit
            || transform.position.y < bottomLimit)
        {
            return true;
        }

        return false;
    }
}
