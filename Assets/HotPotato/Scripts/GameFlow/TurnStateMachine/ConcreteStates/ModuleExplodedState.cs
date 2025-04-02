namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class ModuleExplodedState : TurnState
    {
        public ModuleExplodedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.ModuleExploded, stateMachineData) { }
        
        public override void EnterState()
        {
            base.EnterState();
            NextState = TurnStateMachine.TurnState.TurnStart;
            EventBus<ModuleExplodedExitStateEvent>.Raise(new ModuleExplodedExitStateEvent());
        }
    }
}