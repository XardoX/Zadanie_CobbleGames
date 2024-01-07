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
                    didHit = Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastDistance);
                    if (didHit)
                    {
                        nodes[i, j] = new Node(hit.point);
                    }
                }

            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);

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
        public Vector3 position;
        internal int gCost;
        internal int hCost;
        internal Node parent;

        public Node(Vector3 position)
        {
            this.position = position;
        }

        public int FCost => gCost + hCost;
    }

#endif
}


