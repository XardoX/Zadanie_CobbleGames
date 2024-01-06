using SaveSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameModel model;

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private UIController uiController;

    [SerializeField]
    private CharacterGroup characterGroup;

    public static GameManager Instance { get; private set; }

    public static PlayerController Player => Instance.playerController; 

    public static CameraController Camera => Instance.cameraController;

    bool isPaused;

    public void StartNewGame()
    {
        characterGroup.Init(model.NumberOfUnits);
    }

    public void LoadGame()
    {
        SaveManager.Instance.LoadData();
    }

    public void SetPause(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0 : 1;
        uiController.TogglePauseMenu(pause);
    }

    public void TogglePause()=> SetPause(!isPaused);
    

    public void BackToMenu()
    {
        SaveManager.Instance.SaveData();
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Menu");
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
        SaveManager.Instance.Init();
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
