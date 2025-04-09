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
           
            _abilityFinishedEventBinding = new EventBinding<AbilityFinishedEvent>(GoToNextState);
            EventBus<AbilityFinishedEvent>.Register(_abilityFinishedEventBinding);

            EventBus<AbilityPlayingEnterStateEvent>.Raise(new AbilityPlayingEnterStateEvent { });
        }

        public override void ExitState()
        {
            EventBus<AbilityFinishedEvent>.Deregister(_abilityFinishedEventBinding);
            
            base.ExitState();
        }

        private void GoToNextState()
        {
            NextState = TurnStateMachine.TurnState.BombTicking;
        }

        private void SkipTurn()
        {
            NextState = TurnStateMachine.TurnState.ModuleDefused;
        }
    }
}