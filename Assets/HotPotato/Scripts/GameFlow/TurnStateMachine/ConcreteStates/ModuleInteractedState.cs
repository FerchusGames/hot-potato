namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class ModuleInteractedState : TurnState
    {
        public ModuleInteractedState() : base(GameFlow.TurnStateMachine.TurnStateMachine.TurnState.ModuleInteracted) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}