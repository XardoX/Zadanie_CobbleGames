using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineTargetGroup targetGroup;

    public void AddTarget(Transform transform)
    {
        targetGroup.AddMember(transform, 1f, 1f);
    }

    public void SetMainTarget(int index)
    {
        for (int i = 0; i < targetGroup.m_Targets.Length; i++)
        {
            targetGroup.m_Targets[i].weight = 0.5f;
        }
        targetGroup.m_Targets[index].weight = 1f;
    }
}
