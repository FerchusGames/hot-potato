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

            NextState = _stateMachineData.LastModuleSettings.IsTrap 
                ? TurnStateMachine.TurnState.ModuleExploded : TurnStateMachine.TurnState.ModuleDefused;
        }
    }
}