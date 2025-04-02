using System;

namespace HotPotato.GameFlow.StateMachine
{
    public interface IBaseState<EState> where EState : Enum
    {
        public struct EnterStateEvent : IEvent { }
        public struct ExitStateEvent : IEvent { }
        
        public void EnterState();
        public void ExitState();
        public void UpdateState();
        public void FixedUpdateState();
        public EState NextState { get; set; }
        public EState StateKey { get; }
    }
}