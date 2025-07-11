using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bullet1;
    public GameObject bullet2;

    public float bulletTimer;    // time between each bullet1 spawn
    Coroutine co = null;       // storing coroutine for shooting bullet1 to be able to stop it later
    private float bulletCoroutineTimer = 0f;    // timer to make sure the new ShootBullet1 coroutine runs only after 'bulletTimer' amount of seconds
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void Update()
    {
        if (bulletCoroutineTimer < bulletTimer)
        {
            bulletCoroutineTimer += Time.deltaTime;
        }
        // for bullet1 shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bulletCoroutineTimer >= bulletTimer)
            {
                // start the coroutine when space starts to be pressed
                co = StartCoroutine(ShootBullet1());
                bulletCoroutineTimer = 0f;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // stop the coroutine when space is released
            if (co != null)
                StopCoroutine(co);
        }

        // for bullet2 shooting
        if (!levelManager.isPaused)    // only shoot when game not paused
        {
            if (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (Global.strongBullets > 0)
                {
                    levelManager.PlayStrongBulletSound();
                    SpawnBullet(bullet2);
                    Global.strongBullets -= 1;
                    levelManager.updateStrongBulletText();
                }
            }
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (co != null)
            StopCoroutine(co);
    }

    // coroutine for shooting bullet1
    private IEnumerator ShootBullet1()
    {
        while (true)
        {
            levelManager.PlayBulletSound();
            SpawnBullet(bullet1);
            yield return new WaitForSeconds(bulletTimer);    // wait for few seconds before another shot
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
