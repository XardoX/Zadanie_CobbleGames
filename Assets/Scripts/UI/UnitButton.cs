using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Slider staminaSlider;


    public Toggle Toggle => toggle;

    public void Init(float maxStamina, Color color)
    {
        staminaSlider.maxValue = maxStamina;
        icon.color = color;
    }

    public void SetStaminaSlider(float staminaValue)
    {
        staminaSlider.value = staminaValue;
    }
}
