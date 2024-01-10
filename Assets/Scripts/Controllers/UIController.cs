using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIController: MonoBehaviour
{
    [SerializeField]
    private CanvasGroup currentPanel,
        pauseMenuPanel,
        characterGroupPanel;

    public void OpenPanel(CanvasGroup group)
    {
        var oldPanel = currentPanel;
        oldPanel.DOFade(0f, 0.25f).SetUpdate(true).OnComplete(() => oldPanel.gameObject.SetActive(false));

        group.gameObject.SetActive(true);
        group.DOFade(1f, 0.25f).SetUpdate(true);

        currentPanel = group;
    }

    public void TogglePauseMenu(bool toggle)
    {
        if(toggle)
        {
            OpenPanel(pauseMenuPanel);
        }
        else
        {
            OpenPanel(characterGroupPanel);
        }
    }

}
