namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class AbilityPlayedState : TurnState
    {
        public AbilityPlayedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.AbilityPlayed, stateMachineData) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}