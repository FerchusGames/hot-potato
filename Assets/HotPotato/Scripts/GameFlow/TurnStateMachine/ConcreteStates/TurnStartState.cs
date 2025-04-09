using HotPotato.Bomb;
using UnityEngine;

namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class TurnStartState : TurnState
    {
        public TurnStartState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.TurnStart, stateMachineData) { }
        
        private EventBinding<TurnOwnerChangedEvent> _turnOwnerChangedEventBinding;

        public override void EnterState()
        {
            base.EnterState();

            _turnOwnerChangedEventBinding = new EventBinding<TurnOwnerChangedEvent>(GoToNextState);
            EventBus<TurnOwnerChangedEvent>.Register(_turnOwnerChangedEventBinding);
                
            EventBus<TurnStartEnterStateEvent>.Raise(new TurnStartEnterStateEvent());
        }

        public override void ExitState()
        {
            base.ExitState();
            
            EventBus<TurnOwnerChangedEvent>.Deregister(_turnOwnerChangedEventBinding);
            
            EventBus<TurnStartExitStateEvent>.Raise(new TurnStartExitStateEvent());
        }

        private void GoToNextState(TurnOwnerChangedEvent turnOwnerChangedEvent)
        {
            NextState = TurnStateMachine.TurnState.AbilityPlayed;
        }
    }
}