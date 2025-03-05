using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using FishNet.Connection;

public class Outline : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, DrawWithUnity]
    private RenderingLayerMask outlineLayer;

    private Renderer[] renderers;
    private uint originalLayer;
    private bool isOutlineActive;

    private void Start()
    {
        renderers = TryGetComponent<Renderer>(out var meshRenderer)
            ? new[] { meshRenderer }
            : GetComponentsInChildren<Renderer>();
        originalLayer = renderers[0].renderingLayerMask;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplyOutline(true);
        RequestOutlineChange(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ApplyOutline(false);
        RequestOutlineChange(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestOutlineChange(bool enable, NetworkConnection requestingClient = null)
    {
        RpcUpdateOutline(enable, requestingClient);
    }

    [ObserversRpc]
    private void RpcUpdateOutline(bool enable, NetworkConnection requestingClient)
    {
        if (requestingClient == LocalConnection)
            return;

        ApplyOutline(enable);
    }

    private void ApplyOutline(bool enable)
    {
        foreach (var rend in renderers)
        {
            rend.renderingLayerMask = enable ? originalLayer | outlineLayer : originalLayer;
        }
    }
}