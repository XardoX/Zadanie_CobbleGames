using UnityEngine;
[CreateAssetMenu(fileName = "Game Settings", menuName = "Data/Game Settings")]
public class GameModel : ScriptableObject
{
    [SerializeField]
    private int numberOfUnits = 1;

    [SerializeField]
    private bool isUsingCustomAStar = true;
    [SerializeField] private bool isNewGame;

    public int NumberOfUnits { get => numberOfUnits; set => numberOfUnits = value; }
    public bool IsNewGame { get => isNewGame; set => isNewGame = value; }
    public bool IsUsingCustomAStar { get => isUsingCustomAStar; set => isUsingCustomAStar = value; }

    public void SetNumberOfUnits(float value) => numberOfUnits = (int) value;
}
