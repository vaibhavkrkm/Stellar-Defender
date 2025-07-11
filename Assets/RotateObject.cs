using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 90f; // Degrees per second

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
