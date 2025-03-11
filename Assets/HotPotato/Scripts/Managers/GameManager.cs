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
        
        [SerializeField] private BombTimer _bombTimer;
        
        private readonly SyncVar<int> _currentPlayerIndex = new();

        private List<PlayerController> _players = new();
        private List<BombModuleSettings> _bombModuleSettingsList = new();
        
        private ClueData _clueData;
        
        public override void OnStartNetwork()
        {
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            _players.Clear();
            _bombTimer.OnTimerExpired += TimerExpiredEvent;
        }

        public override void OnStopServer()
        {
            _bombTimer.OnTimerExpired -= TimerExpiredEvent;
        }

        public void RegisterPlayer(PlayerController player)
        {
            if (!IsServerStarted) return;

            if (!_players.Contains(player))
            {
                _players.Add(player);
                if (_players.Count == 1)
                {
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
                _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
            }
            
            module.Despawn();
            CheckForNextTurn();
        }

        [Server]
        private void ExplodeBomb()
        {
            _players[_currentPlayerIndex.Value].LoseObserversRpc();
            _players.RemoveAt(_currentPlayerIndex.Value);
            _currentPlayerIndex.Value %= _players.Count;
        }
        
        [ServerRpc(RequireOwnership = false)]
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
   
        private void CheckForNextTurn()
        {
            if (!IsServerStarted) return;

            if (_players.Count > 1)
            {
                StartNextTurn();
            }
            else
            {
                EndRound();
            }
        }

        private void EndRound()
        {
            OnRoundEnded?.Invoke();
            _bombTimer.StopTimerObserversRpc();
            _players[0].WinObserversRpc();
        }

        private void StartNextTurn()
        {
            OnTurnChanged?.Invoke();
            PlayerController currentPlayer = _players[_currentPlayerIndex.Value];
            currentPlayer.StartTurnObserversRpc();
        }
    }
}