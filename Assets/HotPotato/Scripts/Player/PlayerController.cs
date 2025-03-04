using Cysharp.Threading.Tasks;
using FishNet.Object;
using HotPotato.Managers;
using UnityEngine;

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
        
        private async UniTaskVoid GetInstancesFromNetworkManager()
        {
            do
            {
                _gameManager = base.NetworkManager.GetInstance<GameManager>();
                await UniTask.Yield();
            } while (_gameManager == null);
        }

        private void Update()
        {
            if (!_isMyTurn) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndTurn();
            }
        }

        private void EndTurn()
        {
            if (!IsOwner) return;
            _isMyTurn = false;
            _isCurrentPlayer = false;
            _gameManager.EndTurnServerRpc();
        }
        
        [ObserversRpc]
        public void StartTurnObserversRpc()
        {
            _isCurrentPlayer = true;
            _isMyTurn = IsOwner;
            UIManager.Instance.TriggerIsMyOwnerChanged(_isMyTurn);
        }
    }
}