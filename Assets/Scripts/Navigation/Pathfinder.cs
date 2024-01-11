using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Unity.Burst;

namespace NavigationSystem
{ 
    public class Pathfinder : MonoBehaviour
    {
        [SerializeField]
        private float maxHeightDifference = 0.5f;

        [SerializeField]
        private List<Node> path;

        private List<Node> debugOpenSet = new();

        public List<Node> Path => path;

        private Node[] occupiedNodes;

        private Node[] localNodes;

        [Button]
        public void Test()
        {
            var targetpos = Vector3.one * 15f;
            targetpos.y = 0;
            path = FindPath(Vector3.zero, targetpos);
        }

        public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            NodeGenerator.Nodes.CopyTo(localNodes, 0);

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
                    path = RetracePath(startNode, targetNode);

                    SetOccupiedNodes(path);

                    return path;
                }

                var debugString = currentNode.position +" n: ";

                foreach (Node neighbor in GetNeighbors(currentNode))
                {

                    if (closedSet.Contains(neighbor))
                        continue;

                    int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                    float newTurnCost = 0;
                    if (currentNode.parent != null)
                        newTurnCost = GetTurnCost(currentNode, neighbor);

                    if (!openSet.Contains(neighbor) || newMovementCostToNeighbor < neighbor.gCost)
                    {
                        neighbor.gCost = newMovementCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        if(currentNode.parent != null)
                            neighbor.tCost = newTurnCost;

                        neighbor.parent = currentNode;
                        debugString += $"\n{neighbor.position} g: {neighbor.gCost} h: {neighbor.hCost} t: {neighbor.tCost}";
                        //Debug.Log(currentNode.position + " Neigbour set " + neighbor.position +" gCost: "+ neighbor.gCost +" hCost: "+ neighbor.hCost);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
                    //Debug.Log(debugString);
            }

            return null;
        }

        private void Start()
        {
            localNodes = new Node[NodeGenerator.Nodes.Length];
            occupiedNodes = new Node[1];
        }

        private List<Node> RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != startNode)
            {
                path.Add(currentNode);
                if (currentNode.parent == null) break;
                //Debug.Log(currentNode.parent.position + " n: " + currentNode.position + " cost: " + GetTurnCost(currentNode.parent, currentNode));
                currentNode = currentNode.parent;
            }

            path.Reverse();
            return path;
        }

        private void SetOccupiedNodes(List<Node> nodes)
        {
            for (int i = 0; i < occupiedNodes.Length; i++)
            {
                if (occupiedNodes[i] == null) continue;
                occupiedNodes[i].isOccupied = false;
                occupiedNodes[i] = null;
            }
            
            for (int i = 0; i < nodes.Count && i < occupiedNodes.Length; i++)
            {
                occupiedNodes[i] = NodeGenerator.Nodes[nodes[i].id];
                occupiedNodes[i].isOccupied = true;
            }
            
        }

        private Node GetClosestNode(Vector3 position)
        {
            float closestDistance = float.MaxValue;
            Node closestNode = null;

            foreach (Node node in localNodes)
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
            Node[] nodes = NodeGenerator.Nodes;

            int gridSize = localNodes.Length;

            int[] dx = { 1, -1, 1, 0, -1, 0,};
            int[] dy = { 1, -1, 0, 1, 0, -1,};
           
            for (int i = 0; i < dx.Length; i++)
            {
                int newX = node.x + dx[i];
                int newY = node.y + dy[i];
                var id = newX + (newY * NodeGenerator.Instance.NodeFieldMaxSize);

                if (id >= localNodes.Length) continue;
                if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize && localNodes[id] != null)
                {
                    if (Mathf.Abs(node.position.y - localNodes[id].position.y) > maxHeightDifference || nodes[id].isOccupied)
                        continue;
                    //Debug.Log(node.position + " n: " + newX + " : " + newY + " p: " + nodes[newX,newY].position);
                    neighbors.Add(localNodes[id]);
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

        private float GetTurnCost(Node nodeA, Node nodeB)
        {
            var dot = Vector3.Dot((nodeA.position - nodeA.parent.position).normalized, (nodeB.position - nodeA.position).normalized);
            return (2 - (dot+ 1)) * 2;
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
            Gizmos.DrawSphere(path[path.Count -1].position + Vector3.up, 0.15f);

            for (int i = 0; i < path.Count; i++)
            {
                
                Gizmos.DrawCube(path[i].position, Vector3.one * 0.26f);
            }
            Gizmos.color = Color.red;

            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i].position, path[i + 1].position);
            }

        }
#endif

    }
}