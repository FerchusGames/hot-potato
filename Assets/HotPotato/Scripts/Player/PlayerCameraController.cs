using FishNet.Object;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace HotPotato.Player
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Required]
        [SerializeField] private CinemachineCamera _onTurnCamera;
        
        [Required]
        [SerializeField] private CinemachineCamera _notOnTurnCamera;

        private EventBinding<TurnOwnerChangedEvent> _turnOwnerChangedEventBinding;
        private EventBinding<ModuleClickedEvent> _moduleClickedEventBinding;
        private EventBinding<LoseRoundEvent> _loseRoundEventBinding;
        
        public override void OnStartClient()
        {
            if (!IsOwner)
            {
                gameObject.SetActive(false);
                return;
            }
            
            SetCameraAsLive(_notOnTurnCamera);
            
            RegisterClientEvents();
        }
        
        public override void OnStopClient()
        {
            DeregisterClientEvents();
        }

        private void RegisterClientEvents()
        {
            _turnOwnerChangedEventBinding = new EventBinding<TurnOwnerChangedEvent>(HandleTurnOwnerChangedEvent);
            EventBus<TurnOwnerChangedEvent>.Register(_turnOwnerChangedEventBinding);
            
            _moduleClickedEventBinding = new EventBinding<ModuleClickedEvent>(ReturnToDefaultCamera);
            EventBus<ModuleClickedEvent>.Register(_moduleClickedEventBinding);
            
            _loseRoundEventBinding = new EventBinding<LoseRoundEvent>(ReturnToDefaultCamera);
            EventBus<LoseRoundEvent>.Register(_loseRoundEventBinding);
        }
        
        private void DeregisterClientEvents()
        {
            EventBus<TurnOwnerChangedEvent>.Deregister(_turnOwnerChangedEventBinding);
            EventBus<ModuleClickedEvent>.Deregister(_moduleClickedEventBinding);
            EventBus<LoseRoundEvent>.Deregister(_loseRoundEventBinding);
        }

        private static void SetCameraAsLive(CinemachineCamera camera)
        {
            camera.enabled = false;
            camera.enabled = true;
        }

        private void HandleTurnOwnerChangedEvent(TurnOwnerChangedEvent turnOwnerChangedEvent)
        {
            if (turnOwnerChangedEvent.IsMyTurn)
            {
                SetCameraAsLive(_onTurnCamera);   
                return;
            }
            
            ReturnToDefaultCamera();
        }
        
        private void ReturnToDefaultCamera()
        {
            SetCameraAsLive(_notOnTurnCamera);
        }
    }
}