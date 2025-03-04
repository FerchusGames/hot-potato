using FishNet.Object;
using HotPotato.Managers;
using UnityEngine;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour
    {
        private GameManager _gameManager;
        
        private bool _isMyTurn = false;

        public override void OnStartNetwork()
        {
            _gameManager = base.NetworkManager.GetInstance<GameManager>();
        }
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (IsServer)
            {
                _gameManager.RegisterPlayer(this);
            }
        }

        private void Update()
        {
            if (!_isMyTurn || !IsOwner) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndTurn();
            }
        }

        private void EndTurn()
        {
            if (!IsOwner) return;
            _isMyTurn = false;
            _gameManager.EndTurnServerRpc();
        }
        
        [ObserversRpc]
        public void StartTurnObserversRpc()
        {
            _isMyTurn = IsOwner;
            if (_isMyTurn)
            {
                Debug.Log("It's my turn!");
                return;
            }
            Debug.Log("It's not my turn.");
        }
    }
}