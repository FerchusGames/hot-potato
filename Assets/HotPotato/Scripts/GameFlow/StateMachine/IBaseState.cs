using System;

namespace HotPotato.GameFlow.StateMachine
{
    public interface IBaseState<EState> where EState : Enum
    {
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
        public abstract void FixedUpdateState();
        public EState NextState { get; }
        public EState StateKey { get; }
        public bool IsServer { get; }
    }
}