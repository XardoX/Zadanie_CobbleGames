using UnityEngine;
public class CharacterGroup : MonoBehaviour
{
    [SerializeField]
    private CharacterUnit[] units;

    [SerializeField]
    private CharacterGroupView view;

    private CharacterUnit selectedUnit;

    public void Init(int numberOfUnits)
    {
        units = GetComponentsInChildren<CharacterUnit>();

        for (int i = 0; i < units.Length; i++)
        {
            var id = i;
            units[i].OnSelected += () => SelectUnit(id);
            view.OnUnitSelected += SelectUnit;
            view.CreateUnitButton(units[i].Model);
            GameManager.Camera.AddTarget(units[i].transform);
        }
    }

    public void SelectUnit(int unitID)
    {
        GameManager.Player.SetSelectedObject(units[unitID]);
        GameManager.Camera.SetMainTarget(unitID);
        selectedUnit = units[unitID];
        view.SetUnitSelected(unitID);
        foreach (var unit in units)
        {
            if (unit == selectedUnit) continue;
            unit.SetFollowTarget(selectedUnit.transform);
        }
    }

    public void MoveTo(Vector3 targetPosition)
    {
        foreach (var unit in units)
        {
            if (unit == selectedUnit) continue;
            unit.SetFollowTarget(selectedUnit.transform);
        }

        selectedUnit.ClearFollowTarget();
        selectedUnit.MoveTo(targetPosition);
    }
}
