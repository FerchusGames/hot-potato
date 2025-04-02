namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class WaitingToStartState : TurnState
    {
        public WaitingToStartState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.WaitingToStart, stateMachineData) { }
        
        private EventBinding<RoundStartedEvent> _roundStartedEventBinding;
        
        protected override void SubscribeToEvents()
        {
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(StartRound);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
        }
        
        protected override void UnsubscribeToEvents()
        {
            EventBus<RoundStartedEvent>.Deregister(_roundStartedEventBinding);
        }

        private void StartRound()
        {
            NextState = TurnStateMachine.TurnState.TurnStart;
        }
    }
}