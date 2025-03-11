using Cysharp.Threading.Tasks;
using FishNet.Object;
using HotPotato.Managers;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour
    {
        private GameManager _gameManager;
        
        private bool _isCurrentPlayer = false;
        private bool _isMyTurn = false;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            GetInstancesFromNetworkManager().Forget();
            
            if (IsServerInitialized)
            {
                _gameManager.RegisterPlayer(this);
            }
        }
        
        private async UniTaskVoid GetInstancesFromNetworkManager() // TODO: Remove and initialize in joining scene
        {
            do
            {
                _gameManager = base.NetworkManager.GetInstance<GameManager>();
                await UniTask.Yield();
            } while (_gameManager == null);
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