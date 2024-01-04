using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CharacterUnit : MonoBehaviour, ISelectable
{
    [SerializeField]
    private CharacterModel model;

    private NavMeshAgent agent;

    private MeshRenderer meshRenderer;

    private Transform followTarget;

    public Action OnSelected;

    public CharacterModel Model => model;

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SetFollowTarget(Transform newTarget)
    {
        followTarget = newTarget;
        agent.stoppingDistance = 2f;
    }

    public void ClearFollowTarget()
    {
        followTarget = null;
        agent.stoppingDistance = 0f;
    }

    public void Select() => OnSelected?.Invoke();

    public void Action()
    {
        ClearFollowTarget();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
        {
            MoveTo(hit.point);
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();

        model.RandomizeStats();

        agent.speed = model.Speed;
        agent.angularSpeed = model.Agility;

        var propertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_BaseColor", model.MainColor);
        meshRenderer.SetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        if(followTarget != null)
        {
            agent.SetDestination(followTarget.position);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false) return;
        if (agent.path == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(agent.pathEndPosition + Vector3.up, 0.25f);

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            if (i == 1)
            {
                Gizmos.color = model.MainColor;
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
