using UnityEngine;

public class EnemyCollisions : MonoBehaviour
{
    public EnemyShipScript enemyShip;
    public GameObject smallExplosion;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.CompareTag("PlayerBullet"))
        {
            // play impact sound
            levelManager.PlayImpactSound();

            // play small explosion animation (for bullet collision)
            GameObject smallExplosionGameObject = Instantiate(smallExplosion, collision.transform.position, collision.transform.rotation, transform);

            smallExplosionGameObject.GetComponent<Animator>().speed = 2f;

            // decrease enemy health
            enemyShip.TakeDamage(collision.gameObject.GetComponent<BulletScript>().damage);

            // destroy player bullet
            Destroy(collision.transform.parent.gameObject);
        }
    }
}
