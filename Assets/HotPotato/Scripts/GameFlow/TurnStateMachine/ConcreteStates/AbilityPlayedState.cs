namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class AbilityPlayedState : TurnState
    {
        public AbilityPlayedState() : base(TurnStateMachine.TurnState.AbilityPlayed) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}