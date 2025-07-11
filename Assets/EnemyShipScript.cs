using UnityEngine;
using System.Collections.Generic;

public class EnemyShipScript : MonoBehaviour
{
    [Range(1, 4)] public int tier;
    public int maxHealth;
    public GameObject explosion;
    public float explosionScale = 1;
    private LevelManager levelManager;
    [HideInInspector] public Healthbar healthbar;
    [HideInInspector] public int health;
    public List<GameObject> coinSetList = new List<GameObject>();

    private void Awake()
    {
        health = maxHealth;

        if (tier == 4)
        {
            healthbar = GameObject.Find("HealthbarUIBoss").GetComponent<Healthbar>();
            healthbar.SetMaxHealth(health);
        }
    }

    private void Start()
    {
        // initializing level manager script reference
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void Update()
    {
        // destroying if ship is outside the bounndaries
        if (IsOutsideLimits())
        {
            DestroyEnemy(explode: false);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (healthbar != null)
            healthbar.SetHealth(health);

        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    public void DestroyEnemy(bool explode=true)
    {
        if (explode == true)
            // modify explosion scale to fit the ship
            explosion.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);
            // play explosion animation
            Instantiate(explosion, transform.position, transform.rotation);

        Destroy(transform.parent.gameObject);
        levelManager.EnemyDestroyed(transform.parent.gameObject, explode);
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
