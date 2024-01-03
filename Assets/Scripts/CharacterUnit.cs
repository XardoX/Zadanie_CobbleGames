using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CharacterUnit : MonoBehaviour
{
    private NavMeshAgent agent;
    private Controls input;


    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        input = new();

        AssignInputs();
    }

    private void OnEnable() => input.Enable();

    private void OnDisable() => input.Disable();

    private void AssignInputs()
    {
        input.Player.Move.performed += ctx => ClickToMove();
    }

    private void ClickToMove()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            MoveTo(hit.point);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;
        if (agent.path == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(agent.pathEndPosition + Vector3.up, 0.25f);

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            if (i == 1)
            {
                Gizmos.color = Color.blue;
            }
            Gizmos.DrawSphere(agent.path.corners[i], 0.10f);
        }
        for (int i = 0; i < agent.path.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
        }
    }
#endif

}
