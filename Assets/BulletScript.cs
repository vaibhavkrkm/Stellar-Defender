using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        if (transform.parent.CompareTag("PlayerBullet"))
        {
            // updating the player bullet damage as per the upgrades
            damage = (int)Mathf.Round(damage * Global.playerBulletMultiplier);
        }
    }

    private void Update()
    {
        if (IsOutsideLimits())
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private bool IsOutsideLimits()
    {
        float leftLimit = -18f;
        float rightLimit = 18f;
        float bottomLimit = -10f;
        float topLimit = 10f;

        if (transform.position.x < leftLimit
            || transform.position.x > rightLimit
            || transform.position.y < bottomLimit
            || transform.position.y > topLimit)
        {
            return true;
        }

        return false;
    }
}
