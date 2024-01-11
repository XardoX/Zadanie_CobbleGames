using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

namespace NavigationSystem
{
    public class NodeGenerator : MonoBehaviour
    {
        public static NodeGenerator Instance { get; private set; }
        [SerializeField]
        private float raycastStartHeight = 10f, raycastDistance = 11f;

        [SerializeField]
        private float nodeDistance = 1f;

        [SerializeField]
        private int nodeFieldMaxSize = 50;

        [SerializeField]
        private LayerMask layerMask;

        private static Node[,] nodes;

        public static Node[,] Nodes => nodes;

        public static float NodeDistance => Instance.nodeDistance;

        [Button]

        public void GenerateAllNodes()
        {
            nodes = new Node[nodeFieldMaxSize, nodeFieldMaxSize];
            Vector3 origin = new Vector3(-nodeFieldMaxSize/2 * nodeDistance, raycastStartHeight, -nodeFieldMaxSize/2 * nodeDistance);
            bool didHit = true;
            for (int i = 0; i < nodeFieldMaxSize; i++)
            {
                origin.x = i * nodeDistance - nodeFieldMaxSize/2;
                for (int j = 0; j < nodeFieldMaxSize; j++)
                {
                    origin.z = j * nodeDistance - nodeFieldMaxSize/2;
                    didHit = Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastDistance, layerMask);
                    if (didHit)
                    {
                        nodes[i, j] = new Node(i, j, hit.point);
                    }
                }

            }
        }

        public void GenerateNodesWithDelay(float  delay)
        { 
            if(coroutine == null)
            {
                coroutine = StartCoroutine(GenerateAllNodesDelayed(delay));
            }
        }

        Coroutine coroutine;
        private IEnumerator GenerateAllNodesDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);
            GenerateAllNodes();
            coroutine = null;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);

        }

        private void Start()
        {
            GenerateAllNodes();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false) return;
            if(nodes == null) return;
            for (int i = 0; i < nodes.GetLength(0); i++)
            {
                Gizmos.color = Color.magenta;
                for (int j = 0; j < nodes.GetLength(1) - 1; j++)
                {
                    //Debug.Log(i + " "+j);
                    if (nodes[i,j] != null)
                    {
                        Gizmos.DrawSphere(nodes[i,j].position + Vector3.up, 0.10f);
                    Gizmos.color = Color.white;
                    }

                }

            }

        }
    }

    [System.Serializable]
    public class Node
    {
        public int x, y;
        public Vector3 position;
        public int gCost;
        public int hCost;
        public float tCost;
        public Node parent;

        public Node(int x, int y, Vector3 position)
        {
            this.x = x;
            this.y = y;
            this.position = position;
        }

        public float FCost => gCost + hCost + tCost;
    }

#endif
}


