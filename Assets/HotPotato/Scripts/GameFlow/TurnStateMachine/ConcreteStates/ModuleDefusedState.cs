using UnityEngine;

namespace HotPotato.GameFlow.StateMachine.ConcreteStates
{
    public class ModuleDefusedState : TurnState
    {
        public ModuleDefusedState() : base(TurnStateMachine.TurnState.ModuleDefused) { }

        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }

        public override void UpdateState()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextState = TurnStateMachine.TurnState.BombTicking;
            }
        }
    }
}