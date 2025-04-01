using UnityEngine;

namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class BombTickingState : TurnState 
    {
        public BombTickingState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.BombTicking, stateMachineData) { }
        
        private EventBinding<ModuleInteractedEvent> _moduleExplodedEventBinding;
        
        protected override void SubscribeToEvents()
        {
            _moduleExplodedEventBinding = new EventBinding<ModuleInteractedEvent>(HandleModuleInteractedEvent);
            EventBus<ModuleInteractedEvent>.Register(_moduleExplodedEventBinding);
        }
        
        protected override void UnsubscribeToEvents()
        {
            EventBus<ModuleInteractedEvent>.Deregister(_moduleExplodedEventBinding);
        }
        
        public override void UpdateState()
        {
            base.UpdateState();
            
            EventBus<BombTickingUpdateStateEvent>.Raise(new BombTickingUpdateStateEvent());
        }

        private void HandleModuleInteractedEvent(ModuleInteractedEvent moduleInteractedEvent)
        {
            _stateMachineData.LastModuleSettings = moduleInteractedEvent.Settings;
            NextState = TurnStateMachine.TurnState.ModuleInteracted;
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