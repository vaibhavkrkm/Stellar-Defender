using UnityEngine;

public class BackgroundScrollScript : MonoBehaviour
{
    [Range(-1f, 1f)] public float scrollSpeed = 0.5f;
    private float offset;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
