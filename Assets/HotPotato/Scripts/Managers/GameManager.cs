using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using HotPotato.Bomb;
using HotPotato.Clues;
using HotPotato.Player;
using HotPotato.UI;
using UnityEngine;

namespace HotPotato.Managers
{
    public class GameManager : NetworkBehaviour
    {
        public event Action OnTurnChanged;
        public event Action OnRoundEnded;
        public event Action OnRoundStarted;
        public event Action OnMatchEnded;
        
        [SerializeField] private BombTimer _bombTimer;
        [SerializeField] private int _roundsToWin = 3;
        
        private readonly SyncVar<int> _currentPlayerIndex = new();

        private List<PlayerController> _matchPlayers = new();
        private List<PlayerController> _remainingPlayers = new();
        private List<BombModuleSettings> _bombModuleSettingsList = new();
        
        private ClueData _clueData;
        
        private UIManager UIManager => base.NetworkManager.GetInstance<UIManager>();
        
        public override void OnStartNetwork()
        {
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            _remainingPlayers.Clear();
            _bombTimer.OnTimerExpired += TimerExpiredEvent;
        }

        public override void OnStopServer()
        {
            _bombTimer.OnTimerExpired -= TimerExpiredEvent;
        }

        public void RegisterPlayer(PlayerController player)
        {
            if (!IsServerStarted) return;

            if (!_matchPlayers.Contains(player))
            {
                _matchPlayers.Add(player);
                _remainingPlayers.Add(player);
                if (_remainingPlayers.Count == 1)
                {
                    OnRoundStarted?.Invoke();
                    StartNextTurn();
                }
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void InteractWithModuleServerRpc(BombModule module)
        {
            if (!IsServerStarted) return;
            
            if (module.IsTrap)
            {
                module.ExplodeObserversRpc();
                
                ExplodeBomb();
            }
            else
            {
                _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _remainingPlayers.Count;
            }
            
            module.Despawn();
            CheckForNextTurn();
        }

        [Server]
        private void ExplodeBomb()
        {
            _remainingPlayers[_currentPlayerIndex.Value].LoseObserversRpc();
            _remainingPlayers.RemoveAt(_currentPlayerIndex.Value);
            _currentPlayerIndex.Value %= _remainingPlayers.Count;
        }
        
        [Server]
        private void TimerExpiredEvent()
        {
            ExplodeBomb();
            CheckForNextTurn();
        }

        [Server]
        public void SetCurrentRoundModuleSettings(List<BombModuleSettings> settingsList)
        {
            _bombModuleSettingsList = settingsList;
            _clueData = new ClueData(settingsList, false);
            UIManager.SetClueData(_clueData);
        }
   
        [Server]
        private void CheckForNextTurn()
        {
            if (!IsServerStarted) return;

            if (_remainingPlayers.Count > 1)
            {
                StartNextTurn();
            }
            else if (_remainingPlayers[0].WinCount + 1 >= _roundsToWin)
            {
                EndMatch();
            }
            else
            {
                EndRound();
            }
        }

        [Server]
        private void EndRound()
        {
            OnRoundEnded?.Invoke();
            _bombTimer.StopTimerObserversRpc();
            _remainingPlayers[0].WinRound();
        }
        
        [Server]
        private void EndMatch()
        {
            OnMatchEnded?.Invoke();
            _bombTimer.StopTimerObserversRpc();
            _remainingPlayers[0].WinMatch();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void StartNextRoundServerRpc()
        {
            ResetPlayers();
            OnRoundStarted?.Invoke();

            foreach (var player in _remainingPlayers)
            {
                player.StartRoundObserversRpc();
            }
            
            StartNextTurn();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void StartNextMatchServerRpc()
        {
            ResetPlayers();
            OnRoundStarted?.Invoke();

            foreach (var player in _remainingPlayers)
            {
                player.ResetMatchStats();
                player.StartRoundObserversRpc();
            }
            
            StartNextTurn();
        }

        private void ResetPlayers()
        {
            _remainingPlayers.Clear();
            _remainingPlayers.AddRange(_matchPlayers);
            _currentPlayerIndex.Value = 0;
        }
        
        [Server]
        private void StartNextTurn()
        {
            OnTurnChanged?.Invoke();
            PlayerController currentPlayer = _remainingPlayers[_currentPlayerIndex.Value];
            currentPlayer.StartTurnObserversRpc();
        }
    }
}