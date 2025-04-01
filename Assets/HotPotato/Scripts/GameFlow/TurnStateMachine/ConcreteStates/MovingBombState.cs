namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class MovingBombState : TurnState
    {
        public MovingBombState() : base(GameFlow.TurnStateMachine.TurnStateMachine.TurnState.MovingBomb) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }
    }
}