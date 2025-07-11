using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    // function to update healthbar as health changes
    public void SetHealth(int value)
    {
        slider.value = value;
    }

    // function to set healthbar max value
    public void SetMaxHealth(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }
}
