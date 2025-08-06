using UnityEngine;

public class MobileControlManager : MonoBehaviour
{
    public GameObject mobileControlsUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            mobileControlsUI.SetActive(true);
        }
        else
        {
            mobileControlsUI.SetActive(false);
        }
    }
}
