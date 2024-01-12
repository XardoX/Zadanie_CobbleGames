using NavigationSystem;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed;

    public float stoppingDistance = 2f;

    [SerializeField] 
    Pathfinder pathfinder;

    private Vector3 nextPos, destination;

    public void SetPosition(Vector3 startPos)
    {
        nextPos = startPos;
        destination = startPos;
    }

    public void SetDestination(Vector3 destination, bool recalcatePath = true)
    {
        if(recalcatePath)
        {
            pathfinder.FindPath(transform.position, destination);
        }

        if(pathfinder.Path.Count > 0 )
        {
            nextPos = pathfinder.Path[0].position;
        }

        this.destination = destination;
    }


    private void MoveTowardsNestPos(Vector3 position)
    {
        var speed = moveSpeed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, position, speed);
    }

    private void Awake()
    {
        pathfinder = GetComponent<Pathfinder>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, destination) > stoppingDistance)
        {
            MoveTowardsNestPos(nextPos);
            if(Vector3.Distance(transform.position, nextPos) < 0.5f)
            {
                SetDestination(destination);
            }
        }
    }
}
