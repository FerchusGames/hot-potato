using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.ApplicationLifecycle;
using HotPotato.Managers;
using HotPotato.Player;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotPotato.Bomb
{
    public class BombModule : NetworkBehaviour, IPointerClickHandler
    {
        private readonly SyncVar<BombModuleSettings> _settings = new();
        
        [SerializeField, Required] private MeshRenderer _meshRenderer;
        [SerializeField, Required] private TextMeshProUGUI _text;
        
        private GameManager GameManager => base.NetworkManager.GetInstance<GameManager>();

        [ShowInInspector, ReadOnly] public bool IsTrap { get; private set; } = false;

        public override void OnStartServer()
        {
            GameManager.OnRoundStarted += Despawn;
        }

        public override void OnStopServer()
        {
            GameManager.OnRoundStarted -= Despawn;
        }

        private void Start()
        {
            ApplySettings(_settings.Value);
        }

        [Server]
        private void Despawn()
        {
            base.Despawn();
        }

        [Server]
        public void SetSettings(BombModuleSettings settings)
        {
            _settings.Value = settings;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            GameManager.InteractWithModuleServerRpc(this);
            OwnedPlayerManager.Instance.DisableModuleInteractivity();
        }

        private void ApplySettings(BombModuleSettings settings)
        {
            _meshRenderer.material.color = GetModuleColor(settings);
            _text.text = GetModuleText(settings);
            if (IsServerInitialized) IsTrap = settings.IsTrap;
        }

        [ObserversRpc]
        public void ExplodeObserversRpc()
        {
            Debug.Log("This module just exploded!");
        }
        
        private static Color GetModuleColor(BombModuleSettings settings)
        {
            return ApplicationManager.Instance.AccessibilitySettings.ColorScheme.GetColor(settings.ColorIndex);
        }

        private string GetModuleText(BombModuleSettings settings)
        {
            return GetNumberStringFromIndex(settings.NumberIndex) + GetLetterStringFromIndex(settings.LetterIndex);
        }
        
        private string GetNumberStringFromIndex(int index)
        {
            return (index + 1).ToString();
        }
        
        private string GetLetterStringFromIndex(int index)
        {
            return ((char)('A' + index)).ToString();
        }
    }
    
    public struct BombModuleSettings
    {
        public int ModuleTypeIndex;
        public int ColorIndex;
        public int NumberIndex;
        public int LetterIndex;
        public bool IsTrap;
    }
}