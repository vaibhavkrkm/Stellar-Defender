using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Application.targetFrameRate = 60;
        }
    }
}
