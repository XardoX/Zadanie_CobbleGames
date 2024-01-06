using UnityEngine;
using UnityEngine.AI;
using SaveSystem;

public class CharacterGroup : MonoBehaviour, ISaveable
{
    [SerializeField]
    private CharacterUnit unitPrefab;
    [SerializeField]
    private CharacterUnit[] units;

    [SerializeField]
    private CharacterGroupView view;

    private CharacterUnit selectedUnit;

    public void Init(int numberOfUnits, bool isNewGame = true)
    {
        units = new CharacterUnit[numberOfUnits];

        view.OnUnitSelected += SelectUnit;
        for (int i = 0; i < units.Length; i++)
        {
            var id = i;
            units[i] = Instantiate(unitPrefab, transform);
            units[i].transform.position = transform.position + Vector3.right * id;
            units[i].OnSelected += () => SelectUnit(id);
            GameManager.Camera.AddTarget(units[i].transform);

            if (isNewGame)
            {
                var model = ScriptableObject.CreateInstance<CharacterModel>();
                model.RandomizeStats();
                units[i].Init(model);
                view.CreateUnitButton(model);
            }
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

    public void LoadData(SaveData data)
    {
        Init(data.units.Count, false);
        for (int i = 0; i < data.units.Count; i++)
        {
            units[i].Init(data.units[i].CharacterModel);
            units[i].SetPosAndRot(data.units[i].position, data.units[i].rotation);
            view.CreateUnitButton(units[i].Model);
        }
        if(units.Length > 0)
        {
            units[0].Select();
        }
    }

    public void SaveData(ref SaveData data)
    {
        data.units.Clear();
        foreach (var unit in units)
        {
            if(unit == null) continue;
            data.units.Add(new UnitData(unit.Model, unit.transform.position, unit.transform.rotation));
        }
    }
}
