using HotPotato.Bomb;
using HotPotato.GameFlow.StateMachine;
using HotPotato.GameFlow.TurnStateMachine.ConcreteStates;

namespace HotPotato.GameFlow.TurnStateMachine
{
    public interface ITurnStateMachineData
    {
        public BombModuleSettings LastModuleSettings { get; set; }
    }
    
    public class TurnStateMachine : NetworkStateMachine<TurnStateMachine.TurnState>, ITurnStateMachineData
    {
        public BombModuleSettings LastModuleSettings { get; set; }
        
        private EventBinding<RoundStartedEvent> _roundStartedEventBinding;
        
        public enum TurnState
        {
            WaitingToStart,
            TurnStart,
            BombTicking,
            AbilityPlayed,
            AbilityBlocked,
            ModuleInteracted,
            ModuleExploded,
            ModuleDefused,
            MovingBomb,
        }

        public override void OnStartNetwork()
        {
            if (!IsServerInitialized) return;
            
            States[TurnState.WaitingToStart] = new WaitingToStartState(this);
            States[TurnState.TurnStart] = new TurnStartState(this);
            States[TurnState.BombTicking] = new BombTickingState(this);
            States[TurnState.AbilityPlayed] = new AbilityPlayedState(this);
            States[TurnState.AbilityBlocked] = new AbilityBlockedState(this);
            States[TurnState.ModuleInteracted] = new ModuleInteractedState(this);
            States[TurnState.ModuleDefused] = new ModuleDefusedState(this);
            States[TurnState.ModuleExploded] = new ModuleExplodedState(this);
            States[TurnState.MovingBomb] = new MovingBombState(this);
            
            CurrentState = States[TurnState.WaitingToStart];
        }

        public override void OnStartServer()
        {
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(HandleRoundStartedEvent);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
        }
        
        public override void OnStopServer()
        {
            EventBus<RoundStartedEvent>.Deregister(_roundStartedEventBinding);
        }

        private void HandleRoundStartedEvent(RoundStartedEvent roundStartedEvent)
        {
            CurrentState.NextState = TurnState.TurnStart;
        }
    }
}