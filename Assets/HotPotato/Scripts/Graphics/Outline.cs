using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class Outline
    : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
{
    [SerializeField, DrawWithUnity]
    private RenderingLayerMask outlineLayer;

    [SerializeField]
    private Activate activate = Activate.OnHover;

    private Renderer[] renderers;
    private uint originalLayer;
    private bool isOutlineActive;

    private enum Activate
    {
        OnHover,
        OnClick
    }

    private void Start()
    {
        renderers = TryGetComponent<Renderer>(out var meshRenderer)
            ? new[] { meshRenderer }
            : GetComponentsInChildren<Renderer>();
        originalLayer = renderers[0].renderingLayerMask;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (activate != Activate.OnHover)
            return;
        SetOutline(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (activate != Activate.OnHover)
            return;
        SetOutline(false);
    }
    
    private void SetOutline(bool enable)
    {
        foreach (var rend in renderers)
        {
            rend.renderingLayerMask = enable ? originalLayer | outlineLayer : originalLayer;
        }
    }
}
