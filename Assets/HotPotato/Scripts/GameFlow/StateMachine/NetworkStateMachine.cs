using System;
using System.Collections.Generic;
using FishNet.Object;

namespace HotPotato.GameFlow.StateMachine
{
    public class NetworkStateMachine<EState> : NetworkBehaviour where EState : Enum
    {
        protected Dictionary<EState, IBaseState<EState>> States = new ();
        protected IBaseState<EState> CurrentState;
        
        private bool IsTransitioningState = false;

        public override void OnStartServer()
        {
            CurrentState.EnterState();
        }

        private void Update()
        {
            if (IsTransitioningState) return;
            
            EState nextStateKey = CurrentState.NextState;
            
            if (!nextStateKey.Equals(CurrentState.StateKey))
            {
                TransitionToState(nextStateKey);
                return;
            }
            
            CurrentState.UpdateState();
        }
        
        private void FixedUpdate()
        {
            if (IsTransitioningState) return;

            CurrentState.FixedUpdateState();
        }

        protected void TransitionToState(EState nextStateKey)
        {
            IsTransitioningState = true;
            CurrentState.ExitState();
            CurrentState = States[nextStateKey];
            CurrentState.EnterState();
            IsTransitioningState = false;
        }
    }
}