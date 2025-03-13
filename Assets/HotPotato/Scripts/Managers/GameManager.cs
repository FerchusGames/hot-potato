using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        
        [SerializeField] private BombTimer _bombTimer;
        
        private readonly SyncVar<int> _currentPlayerIndex = new();

        private List<PlayerController> _matchPlayers = new();
        private List<PlayerController> _remainingPlayers = new();
        private List<BombModuleSettings> _bombModuleSettingsList = new();
        
        private ClueData _clueData;
        
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

            if (!_remainingPlayers.Contains(player))
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
            SetClueDataAsync(_clueData).Forget();
        }

        [Server]
        private async UniTaskVoid SetClueDataAsync(ClueData clueData)
        {
            while (base.NetworkManager.GetInstance<UIManager>() == null)
            {
                await UniTask.Yield();
            }

            base.NetworkManager.GetInstance<UIManager>().SetClueData(_clueData);
        }
   
        [Server]
        private void CheckForNextTurn()
        {
            if (!IsServerStarted) return;

            if (_remainingPlayers.Count > 1)
            {
                StartNextTurn();
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
            _remainingPlayers[0].WinObserversRpc();
            StartNextRound();
        }

        private void StartNextRound()
        {
            OnRoundStarted?.Invoke();
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