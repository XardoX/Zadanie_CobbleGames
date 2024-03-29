using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterGroupView : MonoBehaviour
{
    [SerializeField]
    private ToggleGroup toggleGroup;

    [SerializeField]
    private UnitButton characterButtonPrefab;

    [SerializeField]
    private Transform characterButtonsParent;

    private List<UnitButton> characterButtons = new();

    /// <summary>
    /// Parameter is an ID of a selected unit
    /// </summary>
    public Action<int> OnUnitSelected;


    public void CreateUnitButton(CharacterModel characterModel)
    {
        var newButton = Instantiate(characterButtonPrefab, characterButtonsParent);
        var index = characterButtons.Count;
        newButton.Toggle.onValueChanged.AddListener((a) => SelectUnit(index));
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Unit {index +1}";
        newButton.Toggle.group = toggleGroup;
        newButton.Init(characterModel.Stamina, characterModel.MainColor);
        characterButtons.Add(newButton);
    }

    public void SetUnitSelected(int index)
    {
        toggleGroup.SetAllTogglesOff(false);
        characterButtons[index].Toggle.SetIsOnWithoutNotify(true);
    }

    public void SetStaminaSlider(int index, float staminaValue)
    {
        if (index >= characterButtons.Count) return;
        characterButtons[index].SetStaminaSlider(staminaValue);
    }

    private void SelectUnit(int unitID) => OnUnitSelected?.Invoke(unitID);
}
