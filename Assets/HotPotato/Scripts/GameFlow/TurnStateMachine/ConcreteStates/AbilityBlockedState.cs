namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class AbilityBlockedState : TurnState
    {
        public AbilityBlockedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.AbilityBlocked, stateMachineData) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}