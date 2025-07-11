using UnityEngine;

public class BulletStraightMovement : MonoBehaviour
{
    public int moveSpeed;
    public Vector3 direction = Vector3.down;
    [SerializeField] private Transform spriteTransform;
    public float rotationSpeed;

    void Update()
    {
        transform.position = transform.position + (moveSpeed * direction) * Time.deltaTime;

        if (rotationSpeed != 0f)
        {
            spriteTransform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }
}
