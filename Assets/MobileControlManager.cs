using UnityEngine;

public class MobileControlManager : MonoBehaviour
{
    public GameObject mobileControlsUI;

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
