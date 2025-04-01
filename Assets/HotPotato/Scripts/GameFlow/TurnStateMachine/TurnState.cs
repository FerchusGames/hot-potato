using System;
using UnityEngine;

namespace HotPotato.GameFlow.StateMachine
{
    public abstract class TurnState : IBaseState<TurnStateMachine.TurnState>
    {
        public TurnStateMachine.TurnState StateKey { get; }
        public TurnStateMachine.TurnState NextState { get; protected set; }
        public bool IsServer { get; }
        
        protected TurnState(TurnStateMachine.TurnState stateKey)
        {
            StateKey = stateKey;
            NextState = stateKey;
        }

        protected virtual void SubscribeToEvents()
        {
            throw new NotImplementedException();
        }
        
        protected virtual void UnsubscribeToEvents()
        {
            throw new NotImplementedException();
        }
        
        public virtual void EnterState()
        {
            NextState = StateKey;
            Debug.Log($"Entering state: {StateKey}");
            SubscribeToEvents();
        }
        
        public virtual void ExitState()
        {
            Debug.Log($"Exiting state: {StateKey}");
            UnsubscribeToEvents();
        }
        
        public virtual void UpdateState() { }
        
        public virtual void FixedUpdateState() { }
    }
}