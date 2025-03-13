using FishNet.Object;
using HotPotato.Managers;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour
    {
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

        [ObserversRpc]
        public void WinObserversRpc()
        {
            if (IsOwner)
            {
                OwnedPlayerManager.Instance.Win();
            }
        }
    }
}