namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class MovingBombState : TurnState
    {
        public MovingBombState(ITurnStateMachineData stateMachineData) 
            : base(GameFlow.TurnStateMachine.TurnStateMachine.TurnState.MovingBomb, stateMachineData) { }

        EventBinding<BombHasReachedPlayerEvent> _bombHasReachedPlayerEventBinding;
        
        protected override void SubscribeToEvents()
        {
           _bombHasReachedPlayerEventBinding = new EventBinding<BombHasReachedPlayerEvent>(StartTurn);
           EventBus<BombHasReachedPlayerEvent>.Register(_bombHasReachedPlayerEventBinding);
        }
        
        protected override void UnsubscribeToEvents()
        {
            EventBus<BombHasReachedPlayerEvent>.Deregister(_bombHasReachedPlayerEventBinding);
        }

        public override void EnterState()
        {
            base.EnterState();
            EventBus<MovingBombEnterStateEvent>.Raise(new MovingBombEnterStateEvent());
        }

        private void StartTurn()
        {
            NextState = TurnStateMachine.TurnState.TurnStart;
        }
    }
}