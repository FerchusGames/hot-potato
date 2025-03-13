using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.Managers;
using UnityEngine;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour
    {
        private readonly SyncVar<int> _winCount = new();
        
        private bool _isCurrentPlayer = false;
        private bool _isMyTurn = false;
        
        private GameManager GameManager => base.NetworkManager.GetInstance<GameManager>();
        
        public override void OnStartClient()
        { 
            if (IsServerInitialized)
            {
                GameManager.RegisterPlayer(this);
            }
        }
        
        [ObserversRpc]
        public void StartRoundObserversRpc()
        {
            OwnedPlayerManager.Instance.StartRound();
        }
        
        [ObserversRpc]
        public void StartTurnObserversRpc()
        {
            OwnedPlayerManager.Instance.UpdateIsMyTurn(IsOwner);
        }
        
        [ObserversRpc]
        public void LoseObserversRpc()
        {
            if (IsOwner)
            {
                OwnedPlayerManager.Instance.Lose();
            }
        }
        
        [Server]
        public void Win()
        {
            _winCount.Value++;
            WinObserversRpc(_winCount.Value);
        }
        
        [ObserversRpc]
        private void WinObserversRpc(int winCount)
        {
            if (IsOwner)
            {
                OwnedPlayerManager.Instance.Win(winCount);
            }
        }
    }
}