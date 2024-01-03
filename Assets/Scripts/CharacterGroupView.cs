using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterGroupView : MonoBehaviour
{
    [SerializeField]
    private Button characterButtonPrefab;

    [SerializeField]
    private TransferFunction characterButtonsParent;

    private Button[] characterButtons;

    /// <summary>
    /// Parameter is an ID of a selected unit
    /// </summary>
    public Action<int> OnUnitSelected;

    public void CreateUnitButtons(int numberOfUnits)
    {
        characterButtons = new Button[numberOfUnits];

        for (int i = 0; i < characterButtons.Length; i++)
        {
            characterButtons[i].onClick.AddListener(() => SelectUnit(i));
            characterButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = $"Unit {i}";
        }

    }

    private void SelectUnit(int unitID) => OnUnitSelected?.Invoke(unitID);
}
