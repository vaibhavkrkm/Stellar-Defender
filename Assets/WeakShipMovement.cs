using UnityEngine;

public class WeakShipMovement : MonoBehaviour
{
    public int moveSpeed;

    private void Update()
    {
        transform.position = transform.position + (moveSpeed * Vector3.down) * Time.deltaTime;
    }
}
