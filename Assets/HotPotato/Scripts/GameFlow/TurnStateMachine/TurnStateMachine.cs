using HotPotato.GameFlow.StateMachine.ConcreteStates;

namespace HotPotato.GameFlow.StateMachine
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
            
            CurrentState = States[TurnState.BombTicking];
        }
    }
}