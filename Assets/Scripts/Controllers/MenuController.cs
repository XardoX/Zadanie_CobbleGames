using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class MenuController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentPanel;
    public void StartNewGame()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void LoadGame()
    {

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
}
