using HotPotato.AbilitySystem;
using HotPotato.AbilitySystem.Abilities;

namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class AbilityPlayedState : TurnState
    {
        public AbilityPlayedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.AbilityPlayed, stateMachineData) { }

        private EventBinding<AbilityFinishedEvent> _abilityFinishedEventBinding;
        
        public override void EnterState()
        {
            base.EnterState();
           
            RegisterEvents();

            EventBus<AbilityPlayingEnterStateEvent>.Raise(new AbilityPlayingEnterStateEvent { });
        }

        public override void ExitState()
        {
            DeregisterEvents();

            base.ExitState();
        }
        
        private void RegisterEvents()
        {
            _abilityFinishedEventBinding = new EventBinding<AbilityFinishedEvent>(GoToNextState);
            EventBus<AbilityFinishedEvent>.Register(_abilityFinishedEventBinding);
        }
        
        private void DeregisterEvents()
        {
            EventBus<AbilityFinishedEvent>.Deregister(_abilityFinishedEventBinding);
        }
        
        private void GoToNextState(AbilityFinishedEvent abilityFinishedEvent)
        {
            if (abilityFinishedEvent.AbilityType == AbilityType.SkipTurn)
            {
                NextState = TurnStateMachine.TurnState.ModuleDefused;
                return;
            }
            
            NextState = TurnStateMachine.TurnState.BombTicking;
        }
    }
}