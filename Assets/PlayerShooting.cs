using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    }

    public void Bullet1Shooting(InputAction.CallbackContext context)
    {
        if (context.started)    // when space started to be pressed
        {
            if (bulletCoroutineTimer >= bulletTimer)
            {
                // start the coroutine
                co = StartCoroutine(ShootBullet1());
                bulletCoroutineTimer = 0f;
            }
        }

        if (context.canceled)    // when space is released
        {
            // stop the coroutine
            if (co != null)
                StopCoroutine(co);
        }   
    }

    public void Bullet2Shooting(InputAction.CallbackContext context)
    {
        if (context.performed)    // shoot once during performed phase
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
