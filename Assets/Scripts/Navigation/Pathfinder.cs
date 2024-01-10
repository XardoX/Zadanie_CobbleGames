using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace NavigationSystem
{ 
    public class Pathfinder : MonoBehaviour
    {
        [SerializeField]
        private float maxHeightDifference = 0.5f;

        [SerializeField]
        private List<Vector3> path;

        private List<Node> debugOpenSet = new();

        public List<Vector3> Path => path;

        [Button]
        public void Test()
        {
            var targetpos = Vector3.one * 15f;
            targetpos.y = 0;
            path = FindPath(Vector3.zero, targetpos);
        }

        public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
#if UNITY_EDITOR
            debugOpenSet.Clear();
#endif

            if (NodeGenerator.Nodes == null) return null;

            Node startNode = GetClosestNode(startPos);
            Node targetNode = GetClosestNode(targetPos);

            if (startNode == null || targetNode == null)
            {
                Debug.LogError("Cannot find valid start or target node.");
                return null;
            }


            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost))
                    {
                        currentNode = openSet[i];
                        //Debug.Log("Set Current node to: " + currentNode.position);
                        break;
                    }
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
#if UNITY_EDITOR
                debugOpenSet.Add(currentNode);
#endif

                if (currentNode == targetNode)
                {
                    return path = RetracePath(startNode, targetNode);
                }

                foreach (Node neighbor in GetNeighbors(currentNode))
                {

                    if (closedSet.Contains(neighbor))
                        continue;

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                    if (!openSet.Contains(neighbor) || newMovementCostToNeighbor < neighbor.gCost)
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;
                        //Debug.Log(currentNode.position + " Neigbour set " + neighbor.position +" gCost: "+ neighbor.gCost +" hCost: "+ neighbor.hCost);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            return null;
        }

        private List<Vector3> RetracePath(Node startNode, Node endNode)
        {
            List<Vector3> path = new List<Vector3>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode.position);
                if (currentNode.parent == null) break;
                currentNode = currentNode.parent;
            }

            path.Reverse();
            return path;
        }

        private Node GetClosestNode(Vector3 position)
        {
            Node[,] nodes = NodeGenerator.Nodes;
            float closestDistance = float.MaxValue;
            Node closestNode = null;

            foreach (Node node in nodes)
            {
                if (node != null)
                {
                    float distance = Vector3.Distance(position, node.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNode = node;
                    }
                }
            }

            return closestNode;
        }

        private List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();
            Node[,] nodes = NodeGenerator.Nodes;

            int gridSizeX = nodes.GetLength(0);
            int gridSizeY = nodes.GetLength(1);

            int[] dx = { 1, 0, -1, 0, 0, 1, -1 };
            int[] dy = { 0, 1, 0, -1, 0, 1, -1 };
           
            for (int i = 0; i < dx.Length; i++)
            {
                int newX = (int)(node.position.x + gridSizeX/2) + dx[i];
                int newY = (int)(node.position.z + gridSizeY/2)+ dy[i];
                if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY && nodes[newX, newY] != null)
                {
                    if (Mathf.Abs(node.position.y - nodes[newX, newY].position.y) > maxHeightDifference)
                        continue;
                    //Debug.Log(node.position + " n: " + newX + " : " + newY + " p: " + nodes[newX,newY].position);
                    neighbors.Add(nodes[newX, newY]);
                }
            }

            return neighbors;
        }

        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = (int)Mathf.Abs(nodeA.position.x - nodeB.position.x);
            int distY = (int)Mathf.Abs(nodeA.position.z - nodeB.position.z);

            return distX + distY;
        }
#if UNITY_EDITOR
    private void OnDrawGizmos()
        {
            if (Application.isPlaying == false) return;
            Gizmos.color = Color.red;
            foreach (var item in debugOpenSet)
            {
                Gizmos.DrawCube(item.position, Vector3.one * 0.25f);
            }
            if (path == null || path.Count == 0) return;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(path[path.Count -1] + Vector3.up, 0.15f);

            for (int i = 0; i < path.Count; i++)
            {
                
                Gizmos.DrawCube(path[i], Vector3.one * 0.26f);
            }
            Gizmos.color = Color.red;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }

        }
#endif

    }
}