using System;
using UnityEngine;
using UnityEngine.AI;

public class CharacterUnit : MonoBehaviour, ISelectable
{
    [SerializeField]
    private CharacterModel model;

    [SerializeField]
    private MeshRenderer meshRenderer;

    private UnitMovement movement;

    private NavMeshAgent agent;

    private Transform followTarget;

    public Action OnSelected;

    private bool isUsingCustomAstar, isStaminaRecovering;

    private float currentStamina;

    private Vector3 lastFramePos;

    public CharacterModel Model => model;

    public bool IsUsingCustomAstar { get => isUsingCustomAstar; set => isUsingCustomAstar = value; }
    public float CurrentStamina  => currentStamina;

    public void Init(CharacterModel model)
    {
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<UnitMovement>();
        
        this.model = model;
        currentStamina = model.Stamina;

        SetMoveSpeed(model.Speed);
        agent.angularSpeed = model.Agility;

        var propertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_BaseColor", model.MainColor);
        meshRenderer.SetPropertyBlock(propertyBlock);

        agent.enabled = !isUsingCustomAstar;
        movement.enabled = isUsingCustomAstar;
      
    }

    public void MoveTo(Vector3 destination)
    {
        if(isUsingCustomAstar)
        {
            movement.SetDestination(destination);
        }
        else
        {
            agent.SetDestination(destination);
        }
    }

    public void SetFollowTarget(Transform newTarget)
    {
        followTarget = newTarget;
        if (isUsingCustomAstar)
        {
            movement.stoppingDistance = 2f;
        }
        else 
        { 
            agent.stoppingDistance = 2f;
        }

    }

    public void ClearFollowTarget()
    {
        followTarget = null;
        if (isUsingCustomAstar)
        {
            movement.stoppingDistance = 0f;
        }
        else
        {
            agent.stoppingDistance = 0f;
        }
    }

    public void SetPosAndRot(Vector3 pos, Quaternion rot)
    {
        if (isUsingCustomAstar)
        {
            transform.position = pos;
        }
        else
        {
            agent.Warp(pos);
        }
        transform.rotation = rot;
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

    private void Update()
    {
        Follow();
    }

    private void LateUpdate()
    {
        if(lastFramePos != transform.position)
        {
            if(currentStamina > 0)
            {
                currentStamina -= Time.deltaTime;
                if(currentStamina <= 0)
                {
                    currentStamina = 0;
                    SetMoveSpeed(model.Speed / 2);
                    isStaminaRecovering = true;
                }
            }
        }
        else if (isStaminaRecovering)
        {
            currentStamina += Time.deltaTime;
            if(currentStamina >=model.Stamina)
            {
                currentStamina = model.Stamina;
                SetMoveSpeed(model.Speed);
                isStaminaRecovering = false;
            }
        }

        lastFramePos = transform.position;
    }

    private void Follow()
    {
        if(followTarget != null)
        {
            if (isUsingCustomAstar)
            {
                movement.SetDestination(followTarget.position, false);

            }
            else
            {
                agent.SetDestination(followTarget.position);
            }
        }
    }

    private void SetMoveSpeed(float speed)
    {
        if (isUsingCustomAstar)
        {
            movement.moveSpeed = speed;
        }else
        { 
            agent.speed = speed;
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
