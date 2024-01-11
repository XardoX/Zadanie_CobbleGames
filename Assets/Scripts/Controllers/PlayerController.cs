using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Controls input;

    private ISelectable selectedObject;

    public void SetSelectedObject(ISelectable selectable) => selectedObject = selectable;

    private void Awake()
    { 
        input = new();
        AssignInputs();
    }

    private void AssignInputs()
    {
        input.Player.Move.performed += ctx => Select();
        input.Player.Pause.performed += ctx => GameManager.Instance.TogglePause();
    }

    private void Select()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
        {
            var selectable = hit.collider.GetComponentInParent<ISelectable>();
            if(selectable != null)
            {
                selectable.Select();
                selectedObject = selectable;
            }
            else
            {
                selectedObject?.Action();
            }

        }
    }

    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();
}
