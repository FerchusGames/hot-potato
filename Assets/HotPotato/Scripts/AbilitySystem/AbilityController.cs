using FishNet.Object;
using HotPotato.AbilitySystem.Abilities;
using HotPotato.GameFlow.TurnStateMachine;

namespace HotPotato.AbilitySystem
{
    public class AbilityController : NetworkBehaviour, IAbilityController
    {
        public IAbility CurrentAbility { get; private set; } = new SkipAbility();
        
        private EventBinding<AbilityPlayingEnterStateEvent> _abilityPlayingEnterStateEventBinding;
        
        private bool _canPlayAbility;
        
        public override void OnStartServer()
        {
            RegisterServerEvents();
        }

        public override void OnStopServer()
        {
            DeregisterServerEvents();
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
    }
}