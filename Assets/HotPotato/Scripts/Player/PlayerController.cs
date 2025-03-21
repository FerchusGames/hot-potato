using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour
    {
        public int WinCount => _winCount.Value;

        private readonly SyncVar<int> _winCount = new();

        public override void OnStartClient()
        {
            if (!IsServerInitialized) return;
            
            EventBus<PlayerJoinedEvent>.Raise(new PlayerJoinedEvent
            {
                PlayerController = this
            });
        }

        [Server]
        public void ResetMatchStats()
        {
            _winCount.Value = 0;
            ResetMatchStatsObserversRpc();
        }
        
        [ObserversRpc]
        private void ResetMatchStatsObserversRpc()
        {
            if (!IsOwner) return;
            EventBus<MatchResetEvent>.Raise(new MatchResetEvent());
        }
        
        [ObserversRpc]
        public void StartRoundObserversRpc()
        {
            if (!IsOwner) return;
            EventBus<RoundStartedEvent>.Raise(new RoundStartedEvent());
        }

        [ObserversRpc]
        public void StartTurnObserversRpc()
        {
            EventBus<TurnOwnerChangedEvent>.Raise(new TurnOwnerChangedEvent
            {
                IsMyTurn = IsOwner
            });
        }

        [ObserversRpc]
        public void LoseObserversRpc()
        {
            if (!IsOwner) return;
            EventBus<LoseRoundEvent>.Raise(new LoseRoundEvent());
        }

        [Server]
        public void WinRound()
        {
            _winCount.Value++;
            WinRoundObserversRpc(_winCount.Value);
        }

        [ObserversRpc]
        private void WinRoundObserversRpc(int winCount)
        {
            if (!IsOwner) return;
            EventBus<WinRoundEvent>.Raise(new WinRoundEvent
            {
                WinCount = winCount
            });
        }

        [Server]
        public void WinMatch()
        {
            _winCount.Value++;
            WinMatchObserversRpc(_winCount.Value);
        }
        
        [ObserversRpc]
        private void WinMatchObserversRpc(int winCount)
        {
            if (IsOwner)
            {
                EventBus<WinMatchEvent>.Raise(new WinMatchEvent
                {
                    WinCount = winCount
                });
                return;
            }
            
            EventBus<LoseMatchEvent>.Raise(new LoseMatchEvent());
        }
    }
}