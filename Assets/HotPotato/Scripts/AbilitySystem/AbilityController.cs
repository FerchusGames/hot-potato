using System;
using FishNet.Object;
using HotPotato.AbilitySystem.Abilities;
using HotPotato.GameFlow.TurnStateMachine;

namespace HotPotato.AbilitySystem
{
    public class AbilityController : NetworkBehaviour, IAbilityController
    {
        public IAbility CurrentAbility { get; private set; } = new NoAbility();
        
        private EventBinding<AbilityPlayingEnterStateEvent> _abilityPlayingEnterStateEventBinding;
        
        private EventBinding<AbilitySelectRequestedEvent> _abilitySelectedEventBinding;
        private EventBinding<AbilityDeselectRequestedEvent> _abilityDeselectedEventBinding;
        
        private bool _canPlayAbility;
        
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
            if (!IsOwner) return;
            
            RegisterClientEvents();
        }
        
        public override void OnStopClient()
        {
            if (!IsOwner) return;
            
            DeregisterClientEvents();
        }

        [Server]
        private void RegisterServerEvents()
        {
            _abilityPlayingEnterStateEventBinding = new EventBinding<AbilityPlayingEnterStateEvent>(ExecuteAbility);
            EventBus<AbilityPlayingEnterStateEvent>.Register(_abilityPlayingEnterStateEventBinding);
        }
        
        [Server]
        private void DeregisterServerEvents()
        {
            EventBus<AbilityPlayingEnterStateEvent>.Deregister(_abilityPlayingEnterStateEventBinding);
        }
        
        [Client]
        private void RegisterClientEvents()
        {
            _abilitySelectedEventBinding = new EventBinding<AbilitySelectRequestedEvent>(HandleAbilitySelectedEvent);
            EventBus<AbilitySelectRequestedEvent>.Register(_abilitySelectedEventBinding);
            
            _abilityDeselectedEventBinding = new EventBinding<AbilityDeselectRequestedEvent>(HandleAbilityDeselectedEvent);
            EventBus<AbilityDeselectRequestedEvent>.Register(_abilityDeselectedEventBinding);
        }
        
        [Client]
        private void DeregisterClientEvents()
        {
            EventBus<AbilitySelectRequestedEvent>.Deregister(_abilitySelectedEventBinding);
            EventBus<AbilityDeselectRequestedEvent>.Deregister(_abilityDeselectedEventBinding);
        }
        
        [Server]
        public void EnableAbility()
        {
            _canPlayAbility = true;
        }

        [Server]
        public void SetAbility(IAbility ability)
        {
            CurrentAbility = ability;
        }

        [Server]
        private void ExecuteAbility()
        {
            if (!_canPlayAbility) return;
            _canPlayAbility = false;
            
            CurrentAbility.Execute();
        }
        
        [Client]
        private void HandleAbilitySelectedEvent(AbilitySelectRequestedEvent abilitySelectRequestedEvent)
        {
           SetAbilityServerRpc(abilitySelectRequestedEvent.AbilityType);
        }

        [ServerRpc]
        private void SetAbilityServerRpc(AbilityType abilityType)
        {
            CurrentAbility = GetAbilityFromType(abilityType);
            NotifyAbilitySetObserversRpc(abilityType);
        }
        
        [ObserversRpc]
        private void NotifyAbilitySetObserversRpc(AbilityType abilityType)
        {
            if (!IsOwner) return;
            EventBus<AbilitySelectedEvent>.Raise(new AbilitySelectedEvent
            {
                AbilityType = abilityType,
            });
        }
        
        [Client]
        private void HandleAbilityDeselectedEvent()
        {
            SetAbilityServerRpc(AbilityType.NoAbility);
        }

        private IAbility GetAbilityFromType(AbilityType abilityType)
        {
            return abilityType switch
            {
                AbilityType.NoAbility => new NoAbility(),
                AbilityType.SkipAbility => new SkipAbility(),
                _ => throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null)
            };
        }
    }
}