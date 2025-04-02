using System;
using HotPotato.GameFlow.StateMachine;
using UnityEngine;

namespace HotPotato.GameFlow.TurnStateMachine
{
    public abstract class TurnState : IBaseState<TurnStateMachine.TurnState>
    {
        public TurnStateMachine.TurnState StateKey { get; }
        public TurnStateMachine.TurnState NextState { get; protected set; }
        protected readonly ITurnStateMachineData _stateMachineData;
        
        protected TurnState(TurnStateMachine.TurnState stateKey, ITurnStateMachineData stateMachineData)
        {
            StateKey = stateKey;
            NextState = stateKey;
            _stateMachineData = stateMachineData;
        }

        protected virtual void SubscribeToEvents() { }
        
        protected virtual void UnsubscribeToEvents() { }
        
        public virtual void EnterState()
        {
            NextState = StateKey;
            Debug.Log($"Entering state: {StateKey}"); // TODO: Change to LiveWatch
            SubscribeToEvents();
        }
        
        public virtual void ExitState()
        {
            Debug.Log($"Exiting state: {StateKey}"); // TODO: Change to LiveWatch
            UnsubscribeToEvents();
        }
        
        public virtual void UpdateState() { }
        
        public virtual void FixedUpdateState() { }
    }
}