using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using SaveSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentPanel;

    [SerializeField]
    private Button loadGameButton;

    [SerializeField]
    private Slider numberOfUnitsSlider;

    [SerializeField]
    private Toggle aStarToggle;

    [SerializeField]
    private GameModel gameModel;

    public void StartNewGame()
    {
        gameModel.IsNewGame = true;
        SceneManager.LoadSceneAsync("Main");
    }

    public void LoadGame()
    {
        gameModel.IsNewGame = false;
        SceneManager.LoadSceneAsync("Main");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OpenPanel(CanvasGroup group)
    {
        currentPanel.DOFade(0f, 0.25f);
        group.DOFade(1f, 0.25f);

        currentPanel = group;
    }

    private void Start()
    {
        loadGameButton.interactable = SaveManager.Instance.IsGameSaved();
        numberOfUnitsSlider.value =gameModel.NumberOfUnits;
        aStarToggle.SetIsOnWithoutNotify(gameModel.IsUsingCustomAStar);
    }
}
