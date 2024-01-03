using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CharacterGroup : MonoBehaviour
{
    [SerializeField]
    private CharacterUnit[] units;

    private CharacterUnit selectedUnit;

    public void SelectUnit(int unitID)
    {
        selectedUnit = units[unitID];
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

    private void Awake()
    {
        units = GetComponentsInChildren<CharacterUnit>();

        for (int i = 0; i < units.Length; i++)
        {
            var id = i;
            units[i].OnSelected += () => SelectUnit(id);
        }
    }
}
