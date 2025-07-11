using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    [SerializeField] private string shootStyle;    // possible values -> single, multiple
    [SerializeField] private int bulletsPerRound;    // (only required in multiple shoot style)
    public float bulletCooldown;       // time to wait before spawning again
    public float timeBetweenBulletsForMultiple;    // time b/w multiple bullets of a single round (only for multiple shoot style)

    private void Start()
    {
        shootStyle = shootStyle.ToLower();

        if (shootStyle == "single")    // single means shooting a single bullet after a specified cooldown
        {
            StartCoroutine(StartShootingSingleStyle());
        }
        else if (shootStyle == "multiple")   // multiple means shooting a burst of bullets then wait for the cooldown
        {
            StartCoroutine(StartShootingMultipleStyle());
        }
    }

    // coroutine for shooting bullets in single style
    private IEnumerator StartShootingSingleStyle()
    {
        while (true)
        {
            SpawnBullet(bullet);    // spawning the bullet
            yield return new WaitForSeconds(bulletCooldown);  // wait for the cooldown
        }
    }

    // coroutine for shooting bullets in multiple style
    private IEnumerator StartShootingMultipleStyle()
    {
        while (true)
        {
            yield return StartCoroutine(ShootMultipleBullets());  // wait till the burst of bullets shooting complete
            yield return new WaitForSeconds(bulletCooldown);  // wait for the cooldown
        }
    }

    // coroutine for actually shoot the burst of bullets for multiple style
    private IEnumerator ShootMultipleBullets()
    {
        for (int i=0; i<bulletsPerRound; i++)    // shoot the bullet bulletsPerRound times in one burst
        {
            SpawnBullet(bullet);     // spawning the bullet
            yield return new WaitForSeconds(timeBetweenBulletsForMultiple);  // waiting for given amount of seconds
        }
    }


    // function to spawn the bullet
    private void SpawnBullet(GameObject bullet)
    {
        if (bullet != null)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogError("Error! Bullet object can't be null!");
        }
    }
}
