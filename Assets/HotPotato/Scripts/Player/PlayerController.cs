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
            
            WaitForGameManager().Forget();
            
            if (IsServerInitialized)
            {
                _gameManager.RegisterPlayer(this);
            }
        }
        
        private async UniTaskVoid WaitForGameManager()
        {
            _gameManager = base.NetworkManager.GetInstance<GameManager>();
            
            while (_gameManager == null)
            {
                await UniTask.Yield();
                _gameManager = base.NetworkManager.GetInstance<GameManager>();
            }
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