using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour, IPlayerController
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

        public void StartRound()
        {
            StartRoundObserversRpc();
        }
        
        [ObserversRpc]
        private void StartRoundObserversRpc()
        {
            if (!IsOwner) return;
            EventBus<RoundStartedEvent>.Raise(new RoundStartedEvent());
        }

        public void StartTurn()
        {
            StartTurnObserversRpc();
        }
        
        [ObserversRpc]
        private void StartTurnObserversRpc()
        {
            EventBus<TurnOwnerChangedEvent>.Raise(new TurnOwnerChangedEvent
            {
                IsMyTurn = IsOwner
            });
        }
        
        public void Lose()
        {
            LoseObserversRpc();
        }

        [ObserversRpc]
        private void LoseObserversRpc()
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