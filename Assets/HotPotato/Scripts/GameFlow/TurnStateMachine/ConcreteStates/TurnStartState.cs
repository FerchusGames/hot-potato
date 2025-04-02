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
            EventBus<TurnStartEnterStateEvent>.Raise(new TurnStartEnterStateEvent());
            NextState = TurnStateMachine.TurnState.BombTicking;
        }

        public override void ExitState()
        {
            base.ExitState();
            EventBus<TurnStartExitStateEvent>.Raise(new TurnStartExitStateEvent());
        }
    }
}