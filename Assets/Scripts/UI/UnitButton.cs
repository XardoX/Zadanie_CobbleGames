using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;

    [SerializeField]
    private Image icon;

    public Toggle Toggle => toggle;

    public void SetIconColor(Color color) => icon.color = color;
}
