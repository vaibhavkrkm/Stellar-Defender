using System.Collections;
using UnityEngine;

public class ZigZagMovement : MonoBehaviour
{
    // variables for downward movement
    public int moveSpeed;
    public int distanceToMove;

    // variables for zig zag movement (left right movement)
    public float leftDuration = 2f;
    public float rightDuration = 2f;
    public float maxSpeed = 5f;
    public float acceleration = 2f;
    public float deceleration = 2f;

    private float startYPos;
    private bool coroutineStarted = false;

    private void Start()
    {
        startYPos = transform.position.y;
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.y - startYPos) < distanceToMove)
        {
            // moving the ship down if it hasn't crossed distanceToMove
            transform.position = transform.position + (moveSpeed * Vector3.down) * Time.deltaTime;
        }
        else
        {
            // if it crossed distanceToMove, starting its zig zag movement
            if (!coroutineStarted)
            {
                coroutineStarted = true;
                StartCoroutine(MoveCoroutine());
            }
        }
    }

    private IEnumerator MoveCoroutine()
    {
        // main coroutine for handling left right movement
        while (true)
        {
            while (Time.timeScale == 0)
            {
                yield return null; // Wait until unpaused
            }
            yield return MoveLeft(leftDuration);

            while (Time.timeScale == 0)
            {
                yield return null; // Wait until unpaused
            }
            yield return MoveRight(rightDuration);
        }
    }

    private IEnumerator MoveLeft(float duration)
    {
        float elapsedTime = 0f;
        float currentSpeed = 0f;

        while (elapsedTime < duration)       // moving left till it reaches 'duration' in seconds
        {
            while (Time.timeScale == 0)
            {
                yield return null; // Wait until unpaused
            }
            if (elapsedTime < duration/2)     // accelerating for the half of the duration
            {
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            }
            else       // decelerating for another half of the duration
            {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            }

            // moving the ship
            transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

            //updating elapsedTime
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveRight(float duration)
    {
        float elapsedTime = 0f;
        float currentSpeed = 0f;

        while (elapsedTime < duration)       // moving right till it reaches 'duration' in seconds
        {
            while (Time.timeScale == 0)
            {
                yield return null; // Wait until unpaused
            }
            if (elapsedTime < duration / 2)     // accelerating for the half of the duration
            {
                currentSpeed += acceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            }
            else       // decelerating for another half of the duration
            {
                currentSpeed -= deceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
            }

            // moving the ship
            transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);

            //updating elapsedTime
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
