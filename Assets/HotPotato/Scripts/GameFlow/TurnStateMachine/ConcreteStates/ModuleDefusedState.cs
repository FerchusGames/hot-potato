namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class ModuleDefusedState : TurnState
    {
        public ModuleDefusedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.ModuleDefused, stateMachineData) { }

        public override void EnterState()
        {
            base.EnterState();
            NextState = TurnStateMachine.TurnState.MovingBomb;
            EventBus<ModuleDefusedExitStateEvent>.Raise(new ModuleDefusedExitStateEvent());
        }
    }
}