using UnityEngine;

namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class BombTickingState : TurnState 
    {
        public BombTickingState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.BombTicking, stateMachineData) { }
        
        private EventBinding<ModuleInteractedEvent> _moduleExplodedEventBinding;
        private EventBinding<TimerExpiredEvent> _timerExpiredEventBinding;
        
        protected override void SubscribeToEvents()
        {
            _moduleExplodedEventBinding = new EventBinding<ModuleInteractedEvent>(HandleModuleInteractedEvent);
            EventBus<ModuleInteractedEvent>.Register(_moduleExplodedEventBinding);
            
            _timerExpiredEventBinding = new EventBinding<TimerExpiredEvent>(HandleTimerExpiredEvent);
            EventBus<TimerExpiredEvent>.Register(_timerExpiredEventBinding);
        }
        
        protected override void UnsubscribeToEvents()
        {
            EventBus<ModuleInteractedEvent>.Deregister(_moduleExplodedEventBinding);
            EventBus<TimerExpiredEvent>.Deregister(_timerExpiredEventBinding);
        }
        
        public override void UpdateState()
        {
            EventBus<BombTickingUpdateStateEvent>.Raise(new BombTickingUpdateStateEvent());
        }

        private void HandleModuleInteractedEvent(ModuleInteractedEvent moduleInteractedEvent)
        {
            _stateMachineData.LastModuleSettings = moduleInteractedEvent.Settings;
            NextState = TurnStateMachine.TurnState.ModuleInteracted;
        }
        
        private void HandleTimerExpiredEvent()
        {
            NextState = TurnStateMachine.TurnState.ModuleExploded;
        }
        
        public override void EnterState()
        {
            base.EnterState();
            EventBus<BombTickingEnterStateEvent>.Raise(new BombTickingEnterStateEvent());
        }
        
        public override void ExitState()
        {
            base.ExitState();
            EventBus<BombTickingExitStateEvent>.Raise(new BombTickingExitStateEvent());
        }
    }
}