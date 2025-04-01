using UnityEngine;

namespace HotPotato.GameFlow.StateMachine.ConcreteStates
{
    public class BombTickingState : TurnState 
    {
        public BombTickingState() : base(TurnStateMachine.TurnState.BombTicking) { }

        protected override void SubscribeToEvents()
        {
            
        }
        
        protected override void UnsubscribeToEvents()
        {
            
        }
        
        public override void UpdateState()
        {
            base.UpdateState();
            Debug.Log(Time.time);
        }

        public override void EnterState()
        {
            base.EnterState();
        }
        
        public override void ExitState()
        {
            base.ExitState();
        }
    }
}