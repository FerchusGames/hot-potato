﻿using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.ApplicationLifecycle;
using HotPotato.Managers;
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
        
        private EventBinding<RoundStartedEvent> _roundStartedEventBinding; 
            
        [ShowInInspector, ReadOnly] public bool IsTrap { get; private set; } = false;

        public override void OnStartServer()
        {
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(Despawn);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
        }

        public override void OnStopServer()
        {
            EventBus<RoundStartedEvent>.Deregister(_roundStartedEventBinding);
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
            EventBus<ModuleClickedEvent>.Raise(new ModuleClickedEvent
            {
                Module = this
            });
        }

        private void ApplySettings(BombModuleSettings settings)
        {
            _meshRenderer.material.color = GetModuleColor(settings);
            _text.text = GetModuleText(settings);
            if (IsServerInitialized) IsTrap = settings.IsTrap;
        }
        
        public BombModuleSettings GetSettings()
        {
            return _settings.Value;
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