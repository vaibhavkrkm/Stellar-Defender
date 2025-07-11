using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollisions : MonoBehaviour
{
    public PlayerStats playerStats;
    private LevelManager levelManager;

    // powerups which drop down for player to pickup
    public GameObject shield;
    public GameObject magnet;

    // powerup effects gameobjects to spawn on the player
    public GameObject shieldPowerup;
    public GameObject magnetPowerup;

    private Coroutine shieldCoroutine;
    private Coroutine magnetCoroutine;

    private int playerDamageMultiplier = (int)(Global.selectedLevel / 6) + 1;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent != null && collision.transform.parent.CompareTag("EnemyBullet"))
        {
            if (!Global.shieldsUp)
            {
                // play hurt sound and show red effect
                levelManager.PlayPlayerHurtEffect();
                // decrease player health
                playerStats.TakeDamage(collision.gameObject.GetComponent<BulletScript>().damage * playerDamageMultiplier);
                // destroy enemy bullet
                Destroy(collision.transform.parent.gameObject);
            }
        }
        else if (collision.transform.parent != null && collision.transform.parent.CompareTag("Enemy"))
        {
            EnemyShipScript enemy = collision.gameObject.GetComponent<EnemyShipScript>();
            if (!Global.shieldsUp)
            {
                //play hurt sound and show red effect
                levelManager.PlayPlayerHurtEffect();
                // reduce player health proportional to health of the enemy collided
                playerStats.TakeDamage(enemy.maxHealth * 3 * playerDamageMultiplier);
            }
            // destroy the enemy
            enemy.DestroyEnemy();
        }
        else if (collision != null && collision.CompareTag("Collectible"))
        {
            Destroy(collision.gameObject);

            // checking if the collectible is a coin set
            if (collision.gameObject.TryGetComponent<CoinManager>(out CoinManager coinManager))
            {
                levelManager.PlayCoinSound();
                Global.coins += coinManager.coinValue;
                levelManager.CoinSetCollected();
            }
            else if (collision.gameObject.TryGetComponent<PowerupManager>(out PowerupManager powerupManager))
            {
                levelManager.PlayPowerupSound();
                if (powerupManager.powerupName == "shield")
                {
                    if (shieldCoroutine != null)
                    {
                        StopCoroutine(shieldCoroutine);
                        Destroy(shieldPowerup);
                    }
                    shieldCoroutine = StartCoroutine(ShieldEffect());
                }
                else if (powerupManager.powerupName == "magnet")
                {
                    if (magnetCoroutine != null)
                    {
                        StopCoroutine(magnetCoroutine);
                        Destroy(magnetPowerup);
                    }
                    magnetCoroutine = StartCoroutine(MagnetEffect());
                }
                else if (powerupManager.powerupName == "strong_bullet")
                {
                    Global.strongBullets += 3;
                    levelManager.updateStrongBulletText();
                }
            }
        }
    }

    private IEnumerator ShieldEffect()
    {
        if (shield != null)
        {
            // activate shield
            shieldPowerup = Instantiate(shield, transform.position, transform.rotation, transform);
            Global.shieldsUp = true;

            // wait
            yield return new WaitForSeconds(playerStats.shieldDuration);

            // deactivate shield
            Global.shieldsUp = false;
            Destroy(shieldPowerup);
            shieldCoroutine = null;
        }
        else
        {
            Debug.LogError("shield not initialized!");
            yield break;
        }
    }

    private IEnumerator MagnetEffect()
    {
        if (magnet != null)
        {
            // activate shield
            magnetPowerup = Instantiate(magnet, transform.position, transform.rotation, transform);
            Global.magnetMultiplier = Global.magnetPower;

            // wait
            yield return new WaitForSeconds(playerStats.magnetDuration);

            // deactivate shield
            Global.magnetMultiplier = 1;
            Destroy(magnetPowerup);
            magnetCoroutine = null;
        }
        else
        {
            Debug.LogError("magnet not initialized!");
            yield break;
        }
    }
}
