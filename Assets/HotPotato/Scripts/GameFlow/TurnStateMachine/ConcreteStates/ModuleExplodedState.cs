namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class ModuleExplodedState : TurnState
    {
        public ModuleExplodedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.ModuleExploded, stateMachineData) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}