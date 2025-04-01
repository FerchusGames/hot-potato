namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class BombTickingState : TurnState 
    {
        public BombTickingState() : base(TurnStateMachine.TurnState.BombTicking) { }
        
        protected override void SubscribeToEvents()
        {
            
        }
        
        protected override void UnsubscribeToEvents()
        {
            
        }
        
        public override void UpdateState()
        {
            base.UpdateState();
            
            EventBus<BombTickingUpdateStateEvent>.Raise(new BombTickingUpdateStateEvent());
        }

        public override void EnterState()
        {
            base.EnterState();
            EventBus<BombTickingEnterStateEvent>.Raise(new BombTickingEnterStateEvent());
        }
        
        public override void ExitState()
        {
            base.ExitState();
            EventBus<BombTickingExitStateEvent>.Raise(new BombTickingExitStateEvent());
        }
    }
}