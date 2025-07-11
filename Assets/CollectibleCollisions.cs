using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CollectibleCollisions : MonoBehaviour
{
    public CollectibleMovement collectibleMovement;
    public float collisionSensitivity;    // more means it can collide from a large distance
    public float acceleration;
    private float speed;
    private GameObject player;
    [HideInInspector] public bool isAttracted = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = collectibleMovement.moveSpeed;
    }

    private void Update()
    {
        if (player == null || isAttracted) return;  // return if the player is null or its already being attracted towards the player

        Vector3 playerPos = player.transform.position;
        Vector3 collectiblePos = transform.position;

        float sqrDistance = (playerPos - collectiblePos).sqrMagnitude;

        if (sqrDistance < collisionSensitivity * Global.magnetMultiplier)
        {
            // start attract coroutine
            StartCoroutine(AttractToPlayer());
        }
    }

    private IEnumerator AttractToPlayer()
    {
        // IMPORTANT: this coroutine relies on the fact that collectible is destroyed from PlayerCollisions script
        isAttracted = true;

        while (true)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            speed += acceleration * Time.deltaTime;

            // move the collectible
            transform.Translate(direction * speed * Time.deltaTime);

            yield return null;
        }
    }
}
