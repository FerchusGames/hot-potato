using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using HotPotato.Player;
using HotPotato.Utils;

namespace HotPotato.Managers
{
    public class GameManager : NetworkSingleton<GameManager>
    {
        public static event Action OnTurnChanged;
        
        private readonly SyncVar<int> _currentPlayerIndex = new(); // Tracks the active player

        private List<PlayerController> _players = new();

        public override void OnStartServer()
        {
            base.OnStartServer();
            _players.Clear();
        }

        /// <summary>
        /// Registers a player when they join.
        /// </summary>
        public void RegisterPlayer(PlayerController player)
        {
            if (!IsServerStarted) return;

            if (!_players.Contains(player))
            {
                _players.Add(player);
                if (_players.Count == 1)
                {
                    StartTurn(); // Start turn when first player joins
                }
            }
        }

        /// <summary>
        /// Called when a player ends their turn (Server Only).
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public void EndTurnServerRpc()
        {
            if (!IsServerStarted) return;

            _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
            StartTurn();
        }

        /// <summary>
        /// Starts the turn for the current player.
        /// </summary>
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