namespace HotPotato.GameFlow.StateMachine.ConcreteStates
{
    public class BombTickingState : TurnState 
    {
        public BombTickingState() : base(TurnStateMachine.TurnState.BombTicking) { }

        protected override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
        }
        
        protected override void UnsubscribeToEvents()
        {
            base.UnsubscribeToEvents();
        }
        
        public override void EnterState()
        {
            base.EnterState();
        }
        
        public override void ExitState()
        {
            base.ExitState();
        }
    }
}