using NavigationSystem;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AreaLoader : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceGameObject area;

    private GameObject instancedArea;

    private NavMeshSurface surface;

    private void Awake()
    {
        surface = GetComponentInParent<NavMeshSurface>();
    }

    private void LoadArea()
    {
        area.InstantiateAsync().Completed += OnAddressableLoaded;
    }

    private void UnloadArea()
    {
        if (instancedArea == null) return;
        area.ReleaseInstance(instancedArea);
    }

    private void OnAddressableLoaded(AsyncOperationHandle<GameObject> operation)
    {
        if (operation.Status == AsyncOperationStatus.Succeeded)
        {
            instancedArea = operation.Result;
            instancedArea.transform.position = transform.position;
            instancedArea.transform.parent = transform;

            if(GameManager.Instance.Model.IsUsingCustomAStar)
            {
                NodeGenerator.Instance.GenerateNodesWithDelay(0.2f);
            }
            else
            {
                surface.BuildNavMesh();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("MainCamera"))
        {
            LoadArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            UnloadArea();
        }
    }
}
