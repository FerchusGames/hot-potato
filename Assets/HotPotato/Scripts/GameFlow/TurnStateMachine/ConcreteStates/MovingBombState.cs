namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class MovingBombState : TurnState
    {
        public MovingBombState(ITurnStateMachineData stateMachineData) 
            : base(GameFlow.TurnStateMachine.TurnStateMachine.TurnState.MovingBomb, stateMachineData) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}