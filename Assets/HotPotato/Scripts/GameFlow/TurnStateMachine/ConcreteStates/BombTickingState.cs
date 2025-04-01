using UnityEngine;

namespace HotPotato.GameFlow.StateMachine.ConcreteStates
{
    public class BombTickingState : TurnState 
    {
        public BombTickingState() : base(TurnStateMachine.TurnState.BombTicking) { }
        
        public struct EnterStateEvent : IEvent { }
        public struct ExitStateEvent : IEvent { }
        public struct UpdateStateEvent : IEvent { }

        protected override void SubscribeToEvents()
        {
            
        }
        
        protected override void UnsubscribeToEvents()
        {
            
        }
        
        public override void UpdateState()
        {
            base.UpdateState();
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextState = TurnStateMachine.TurnState.ModuleDefused;
            }
            
            EventBus<UpdateStateEvent>.Raise(new UpdateStateEvent());
        }

        public override void EnterState()
        {
            base.EnterState();
            EventBus<EnterStateEvent>.Raise(new EnterStateEvent());
        }
        
        public override void ExitState()
        {
            base.ExitState();
            EventBus<ExitStateEvent>.Raise(new ExitStateEvent());
        }
    }
}