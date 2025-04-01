using HotPotato.Bomb;

namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class ModuleInteractedState : TurnState
    {
        public ModuleInteractedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.ModuleInteracted, stateMachineData) { }

        private BombModuleSettings _moduleSettings;
        
        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}