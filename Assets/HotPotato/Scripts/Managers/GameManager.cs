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
        [SerializeField] private BombTimer _bombTimer;
        [SerializeField] private int _roundsToWin = 3;
        
        private readonly SyncVar<int> _currentPlayerIndex = new();

        private EventBinding<ModulesSpawnedEvent> _modulesSpawnedEventBinding;
        private EventBinding<TimerExpiredEvent> _timerExpiredEventBinding;
        
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
            
            _modulesSpawnedEventBinding = new EventBinding<ModulesSpawnedEvent>(SetCurrentRoundModuleSettings);
            EventBus<ModulesSpawnedEvent>.Register(_modulesSpawnedEventBinding);
            
            _timerExpiredEventBinding = new EventBinding<TimerExpiredEvent>(TimerExpiredEvent);
            EventBus<TimerExpiredEvent>.Register(_timerExpiredEventBinding);
        }

        public override void OnStopServer()
        {
            EventBus<ModulesSpawnedEvent>.Deregister(_modulesSpawnedEventBinding);
            EventBus<TimerExpiredEvent>.Deregister(_timerExpiredEventBinding);
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
                    EventBus<RoundStartedEvent>.Raise(new RoundStartedEvent());
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
        private void SetCurrentRoundModuleSettings(ModulesSpawnedEvent modulesSpawnedEvent)
        {
            var settingsList = modulesSpawnedEvent.settingsList;
            
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
            EventBus<RoundEndedEvent>.Raise(new RoundEndedEvent());
            _bombTimer.StopTimerObserversRpc();
            _remainingPlayers[0].WinRound();
        }
        
        [Server]
        private void EndMatch()
        {
            EventBus<MatchEndedEvent>.Raise(new MatchEndedEvent());
            _bombTimer.StopTimerObserversRpc();
            _remainingPlayers[0].WinMatch();
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void StartNextRoundServerRpc()
        {
            ResetPlayers();
            EventBus<RoundStartedEvent>.Raise(new RoundStartedEvent());

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
            EventBus<RoundStartedEvent>.Raise(new RoundStartedEvent());

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
            PlayerController currentPlayer = _remainingPlayers[_currentPlayerIndex.Value];
            currentPlayer.StartTurnObserversRpc();
        }
    }
}