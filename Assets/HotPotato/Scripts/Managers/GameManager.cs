using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using HotPotato.Bomb;
using HotPotato.Player;

namespace HotPotato.Managers
{
    public class GameManager : NetworkBehaviour
    {
        public event Action OnTurnChanged;
        
        private readonly SyncVar<int> _currentPlayerIndex = new();

        private List<PlayerController> _players = new();
        
        public override void OnStartNetwork()
        {
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            _players.Clear();
        }
        
        public void RegisterPlayer(PlayerController player)
        {
            if (!IsServerStarted) return;

            if (!_players.Contains(player))
            {
                _players.Add(player);
                if (_players.Count == 1)
                {
                    StartTurn();
                }
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void InteractWithModuleServerRpc(BombModule module)
        {
            if (!IsServerStarted) return;
        
            module.Despawn();
            _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
            StartTurn();
        }
        
        private void StartTurn()
        {
            if (!IsServerStarted) return;

            if (_players.Count > 0)
            {
                PlayerController currentPlayer = _players[_currentPlayerIndex.Value];
                currentPlayer.StartTurnObserversRpc();
            }
            
            OnTurnChanged?.Invoke();
        }
    }
}