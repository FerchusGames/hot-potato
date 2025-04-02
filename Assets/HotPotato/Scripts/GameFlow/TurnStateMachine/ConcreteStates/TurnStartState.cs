using HotPotato.Bomb;
using UnityEngine;

namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class TurnStartState : TurnState
    {
        public TurnStartState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.TurnStart, stateMachineData) { }
        
        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }

        public override void EnterState()
        {
            base.EnterState();
            NextState = TurnStateMachine.TurnState.BombTicking;
            EventBus<TurnStartEnterStateEvent>.Raise(new TurnStartEnterStateEvent());
        }

        public override void ExitState()
        {
            base.ExitState();
            EventBus<TurnStartExitStateEvent>.Raise(new TurnStartExitStateEvent());
        }
    }
}