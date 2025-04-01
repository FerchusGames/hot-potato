using HotPotato.GameFlow.StateMachine;
using HotPotato.GameFlow.TurnStateMachine.ConcreteStates;

namespace HotPotato.GameFlow.TurnStateMachine
{
    public class TurnStateMachine : NetworkStateMachine<TurnStateMachine.TurnState>
    {
        public enum TurnState
        {
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
            
            States[TurnState.BombTicking] = new BombTickingState();
            States[TurnState.AbilityPlayed] = new AbilityPlayedState();
            States[TurnState.AbilityBlocked] = new AbilityBlockedState();
            States[TurnState.ModuleInteracted] = new ModuleInteractedState();
            States[TurnState.ModuleDefused] = new ModuleDefusedState();
            States[TurnState.ModuleExploded] = new ModuleExplodedState();
            States[TurnState.MovingBomb] = new MovingBombState();
            
            CurrentState = States[TurnState.BombTicking];
        }
    }
}