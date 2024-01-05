using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private GameModel model;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private CharacterGroup characterGroup;

    public static PlayerController Player => Instance.playerController; 

    public static CameraController Camera => Instance.cameraController;

    public void StartNewGame()
    {
        characterGroup.Init(model.NumberOfUnits);
    }

    public void LoadGame()
    {

    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if(model.IsNewGame)
        {
            StartNewGame();
        }
        else
        {
            LoadGame();
        }
    }
}
