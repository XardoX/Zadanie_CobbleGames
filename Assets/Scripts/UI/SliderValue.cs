using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderValue : MonoBehaviour
{
    [SerializeField]
    public Slider slider;

    [SerializeField]
    private TextMeshProUGUI text;

    private void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        if (text == null)
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }
        slider.onValueChanged.AddListener((value) => UpdateSliderText());
    }

    public void UpdateSliderText()
    {
        text.text = slider.value.ToString("0");
    }
    public void SetValue(float value)
    {
        slider.SetValueWithoutNotify(value);
        UpdateSliderText();
    }
}

