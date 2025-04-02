using HotPotato.Bomb;
using UnityEngine;

namespace HotPotato.GameFlow.TurnStateMachine.ConcreteStates
{
    public class ModuleInteractedState : TurnState
    {
        public ModuleInteractedState(ITurnStateMachineData stateMachineData) 
            : base(TurnStateMachine.TurnState.ModuleInteracted, stateMachineData) { }
        
        private const float TimeToShowModule = 3f;
        
        private float _timeElapsed;
        
        protected override void SubscribeToEvents()
        {
           
        }
        
        protected override void UnsubscribeToEvents()
        {
         
        }

        public override void EnterState()
        {
            base.EnterState();
            _timeElapsed = 0f;
            EventBus<ModuleInteractedEnterStateEvent>.Raise(new ModuleInteractedEnterStateEvent()
            {
                Settings = _stateMachineData.LastModuleSettings
            });
        }

        public override void ExitState()
        {
            base.ExitState();
            EventBus<ModuleInteractedExitStateEvent>.Raise(new ModuleInteractedExitStateEvent());
        }

        public override void UpdateState()
        {
            if (_timeElapsed < TimeToShowModule)
            {
                _timeElapsed += Time.deltaTime;
                return;
            }

            NextState = TurnStateMachine.TurnState.TurnStart;
        }
    }
}