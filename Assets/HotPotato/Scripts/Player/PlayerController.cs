using FishNet.Object;
using HotPotato.Managers;
using UnityEngine;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour
    {
        private bool _isMyTurn = false; // Local turn flag

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (IsServer)
            {
                GameManager.Instance.RegisterPlayer(this);
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
            GameManager.Instance.EndTurnServerRpc();
        }

        /// <summary>
        /// Called on all clients to notify the current player.
        /// </summary>
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