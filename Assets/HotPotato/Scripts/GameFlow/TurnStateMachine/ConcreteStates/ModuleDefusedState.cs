namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class ModuleDefusedState : TurnState
    {
        public ModuleDefusedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.ModuleDefused, stateMachineData) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}