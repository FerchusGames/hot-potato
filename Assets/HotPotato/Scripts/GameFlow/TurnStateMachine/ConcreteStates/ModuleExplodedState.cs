namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class ModuleExplodedState : TurnState
    {
        public ModuleExplodedState() : base(GameFlow.TurnStateMachine.TurnStateMachine.TurnState.ModuleExploded) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}