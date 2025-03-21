using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.Managers;
using UnityEngine;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour
    {
        public int WinCount => _winCount.Value;

        private readonly SyncVar<int> _winCount = new();

        private bool _isCurrentPlayer = false;
        private bool _isMyTurn = false;

        private GameManager GameManager => base.NetworkManager.GetInstance<GameManager>();

        public override void OnStartClient()
        {
            if (!IsServerInitialized) return;
            GameManager.RegisterPlayer(this);
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
                isMyTurn = IsOwner
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
                winCount = winCount
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
                    winCount = winCount
                });
                return;
            }
            
            EventBus<LoseMatchEvent>.Raise(new LoseMatchEvent());
        }
    }
}