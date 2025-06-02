using System;
using FishNet.Object;
using UnityEngine;

namespace HotPotato.Player
{
    public class PlayerFaceChanger : NetworkBehaviour
    {
        private enum PlayerFace
        {
            NotOnTurn,
            OnTurn,
            Dead
        }
        
        [SerializeField] private MeshRenderer _faceMeshRenderer;

        private Material _faceMaterial;
        
        private EventBinding<LoseRoundEvent> _loseRoundEventBinding;
        private EventBinding<RoundStartedEvent> _roundStartedEventBinding;
        private EventBinding<TurnOwnerChangedEvent> _turnOwnerChangedEventBinding;
        
        private bool _hasLost = false;
        
        public override void OnStartClient()
        {
            _faceMaterial = _faceMeshRenderer.materials[1];
            
            if (!IsOwner) return;
            
            RegisterEvents();
        }

        public override void OnStopClient()
        {
            DeregisterEvents();
        }

        private void RegisterEvents()
        {
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(HandleRoundStartedEvent);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
            
            _turnOwnerChangedEventBinding = new EventBinding<TurnOwnerChangedEvent>(HandleTurnOwnerChangedEvent);
            EventBus<TurnOwnerChangedEvent>.Register(_turnOwnerChangedEventBinding);
            
            _loseRoundEventBinding = new EventBinding<LoseRoundEvent>(HandleLoseRoundEvent);
            EventBus<LoseRoundEvent>.Register(_loseRoundEventBinding);
        }

        private void DeregisterEvents()
        {
            EventBus<RoundStartedEvent>.Deregister(_roundStartedEventBinding);
            EventBus<TurnOwnerChangedEvent>.Deregister(_turnOwnerChangedEventBinding);
            EventBus<LoseRoundEvent>.Deregister(_loseRoundEventBinding);
        }
        
        private void HandleRoundStartedEvent(RoundStartedEvent obj)
        {
            _hasLost = false;
            
            SetCurrentFaceServerRPC(PlayerFace.NotOnTurn);
        }
        
        private void HandleTurnOwnerChangedEvent(TurnOwnerChangedEvent obj)
        {
            if (_hasLost) return;
            
            PlayerFace currentFace = obj.IsMyTurn ? PlayerFace.OnTurn : PlayerFace.NotOnTurn;
            
            SetCurrentFaceServerRPC(currentFace);
        }
        
        private void HandleLoseRoundEvent(LoseRoundEvent obj)
        {
            _hasLost = true;
            SetCurrentFaceServerRPC(PlayerFace.Dead);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetCurrentFaceServerRPC(PlayerFace face)
        {
            SetCurrentFaceObserversRPC(face);
        }

        [ObserversRpc]
        private void SetCurrentFaceObserversRPC(PlayerFace face)
        {
            _faceMaterial.SetFloat("_CurrentFace", (float)face);
        }
    }
}