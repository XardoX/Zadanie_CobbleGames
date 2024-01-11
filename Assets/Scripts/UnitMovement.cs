using NavigationSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed;

    public float stoppingDistance = 2f;

    [SerializeField] 
    Pathfinder pathfinder;

    private Vector3 targetPos, destination;

    public void SetDestination(Vector3 destination, bool recalcatePath = true)
    {
        if(recalcatePath)
        {
            pathfinder.FindPath(transform.position, destination);
        }
        if(pathfinder.Path.Count > 0 )
        {
            targetPos = pathfinder.Path[0].position;
        }

        this.destination = destination;
    }


    private void MoveTowardsTarget(Vector3 targetPos)
    {
        var speed = moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
    }

    private void Awake()
    {
        pathfinder = GetComponent<Pathfinder>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, destination) > stoppingDistance)
        {
            MoveTowardsTarget(targetPos);
            if(Vector3.Distance(transform.position, targetPos) < 0.5f)
            {
                SetDestination(destination);
            }
            

        }
        else if (pathfinder.Path.Count > 0)
        {
           // SetDestination(destination);
        }
    }
}
