using UnityEngine;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Data/Game Settings")]
public class GameModel : ScriptableObject
{
    [SerializeField]
    private int numberOfUnits = 1;

    public int NumberOfUnits { get => numberOfUnits; set => numberOfUnits = value; }
}
