using UnityEngine;
using DG.Tweening;
public class TargetCircle : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;


    public void Show(Vector3 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        var propertyBlock = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_Size", 1f);
        var size = 1f;
        DOTween.To(() => size, x => size = x, 0.8f, 0.4f).OnUpdate(() =>
        {
            propertyBlock.SetFloat("_Size", size);
            meshRenderer.SetPropertyBlock(propertyBlock);
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
