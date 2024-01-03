using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Controls input;

    private ISelectable selectedObject;
    private void Awake()
    {
        input = new();
        AssignInputs();
    }

    private void AssignInputs()
    {
        input.Player.Move.performed += ctx => Select();
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
