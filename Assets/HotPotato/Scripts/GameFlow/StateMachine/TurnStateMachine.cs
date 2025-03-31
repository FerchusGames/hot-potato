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
            
            // TODO: Implement each state 
        }
    }
}