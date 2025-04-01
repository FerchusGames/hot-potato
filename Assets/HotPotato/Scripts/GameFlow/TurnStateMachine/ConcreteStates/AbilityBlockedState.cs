namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class AbilityBlockedState : TurnState
    {
        public AbilityBlockedState() : base(GameFlow.TurnStateMachine.TurnStateMachine.TurnState.AbilityBlocked) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}