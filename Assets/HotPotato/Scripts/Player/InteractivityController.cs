using FishNet.Object;
using HotPotato.GameFlow.TurnStateMachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotPotato.Player
{
    public class InteractivityController : NetworkBehaviour
    {
        [SerializeField] private LayerMask _notOnTurnEventMask;

        private EventBinding<TurnOwnerChangedEvent> _turnOwnerChangedEventBinding;
        private EventBinding<LoseRoundEvent> _loseRoundEventBinding;
        private EventBinding<WinRoundEvent> _winRoundEventBinding;
        private EventBinding<WinMatchEvent> _winMatchEventBinding;
        private EventBinding<ModuleClickedEvent> _moduleClickedEventBinding;
        private EventBinding<BombTickingEnterStateEvent> _bombTickingEnterStateEventBinding;
        
        private PhysicsRaycaster _physicsRaycaster = null;
        private static readonly LayerMask EverythingMask = ~0;
        
        private bool _isMyTurn = false;
        
        private void Awake()
        {   
            _physicsRaycaster = Camera.main.GetComponent<PhysicsRaycaster>();
            _physicsRaycaster.eventMask = _notOnTurnEventMask;
        }
        
        public override void OnStartServer()
        {
            RegisterServerEvents();
        }
        
        public override void OnStopServer()
        {
            DeregisterServerEvents();
        }
        
        public override void OnStartClient()
        {
            RegisterClientEvents();
        }

        public override void OnStopClient()
        {
            DeregisterClientEvents();
        }
        
        [Server]
        private void RegisterServerEvents()
        {
            _bombTickingEnterStateEventBinding = new EventBinding<BombTickingEnterStateEvent>(HandleBombTickingEnterStateEvent);
            EventBus<BombTickingEnterStateEvent>.Register(_bombTickingEnterStateEventBinding);
        }
        
        [Server]
        private void DeregisterServerEvents()
        {
            EventBus<BombTickingEnterStateEvent>.Deregister(_bombTickingEnterStateEventBinding);
        }
        
        [Client]
        private void RegisterClientEvents()
        {
            _turnOwnerChangedEventBinding = new EventBinding<TurnOwnerChangedEvent>(HandleTurnOwnerChangedEvent);
            EventBus<TurnOwnerChangedEvent>.Register(_turnOwnerChangedEventBinding);

            _loseRoundEventBinding = new EventBinding<LoseRoundEvent>(DisableModuleInteractivity);
            EventBus<LoseRoundEvent>.Register(_loseRoundEventBinding);

            _winRoundEventBinding = new EventBinding<WinRoundEvent>(DisableModuleInteractivity);
            EventBus<WinRoundEvent>.Register(_winRoundEventBinding);

            _winMatchEventBinding = new EventBinding<WinMatchEvent>(HandleWinMatchEvent);
            EventBus<WinMatchEvent>.Register(_winMatchEventBinding);

            _moduleClickedEventBinding = new EventBinding<ModuleClickedEvent>(DisableModuleInteractivity);
            EventBus<ModuleClickedEvent>.Register(_moduleClickedEventBinding);
        }

        [Client]
        private void DeregisterClientEvents()
        {
            EventBus<TurnOwnerChangedEvent>.Deregister(_turnOwnerChangedEventBinding);
            EventBus<LoseRoundEvent>.Deregister(_loseRoundEventBinding);
            EventBus<WinRoundEvent>.Deregister(_winRoundEventBinding);
            EventBus<WinMatchEvent>.Deregister(_winMatchEventBinding);
            EventBus<ModuleClickedEvent>.Deregister(_moduleClickedEventBinding);
        }
        
        private void SetModuleInteractivity(bool interactive)
        {
            if (interactive)
            {
                _physicsRaycaster.eventMask = EverythingMask; 
                return;
            }

            _physicsRaycaster.eventMask = _notOnTurnEventMask;
        }
        
        private void HandleTurnOwnerChangedEvent(TurnOwnerChangedEvent turnOwnerChangedEvent)
        {
            _isMyTurn = turnOwnerChangedEvent.IsMyTurn;
        }
        
        [Server]
        private void HandleBombTickingEnterStateEvent()
        {
            HandleBombTickingEnterStateEventObserversRpc();
        }
        
        [ObserversRpc]
        private void HandleBombTickingEnterStateEventObserversRpc()
        {
            SetModuleInteractivity(_isMyTurn);
        }
        
        private void HandleWinMatchEvent(WinMatchEvent winMatchEvent)
        {
            DisableModuleInteractivity();
        }
        
        private void DisableModuleInteractivity()
        {
            SetModuleInteractivity(false);
        }
    }
}