using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.Player;
using UnityEngine;

namespace HotPotato.Managers
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private int _roundsToWin = 3;
        
        private readonly SyncVar<int> _currentPlayerIndex = new();
        
        private List<IPlayerController> _matchPlayers = new();
        private List<IPlayerController> _remainingPlayers = new();
        
        private EventBinding<PlayerJoinedEvent> _playerJoinedEventBinding;
        private EventBinding<TimerExpiredEvent> _timerExpiredEventBinding;
        private EventBinding<ModuleExplodedEvent> _moduleExplodedEventBinding;
        private EventBinding<ModuleDefusedEvent> _moduleDefusedEventBinding;
        private EventBinding<StartNextRoundEvent> _startNextRoundEventBinding;
        private EventBinding<StartNextMatchEvent> _startNextMatchEventBinding;
        
        public override void OnStartServer()
        {
            _remainingPlayers.Clear();
            
            RegisterServerEvents();
        }
        
        public override void OnStopServer()
        {
            DeregisterServerEvents();
        }

        private void RegisterServerEvents()
        {
            _playerJoinedEventBinding = new EventBinding<PlayerJoinedEvent>(RegisterPlayer);
            EventBus<PlayerJoinedEvent>.Register(_playerJoinedEventBinding);
            
            _timerExpiredEventBinding = new EventBinding<TimerExpiredEvent>(HandleTimerExpiredEvent);
            EventBus<TimerExpiredEvent>.Register(_timerExpiredEventBinding);
            
            _moduleExplodedEventBinding = new EventBinding<ModuleExplodedEvent>(HandleModuleExplodedEvent);
            EventBus<ModuleExplodedEvent>.Register(_moduleExplodedEventBinding);
            
            _moduleDefusedEventBinding = new EventBinding<ModuleDefusedEvent>(HandelModuleDefusedEvent);
            EventBus<ModuleDefusedEvent>.Register(_moduleDefusedEventBinding);
            
            _startNextRoundEventBinding = new EventBinding<StartNextRoundEvent>(StartNextRoundServerRpc);
            EventBus<StartNextRoundEvent>.Register(_startNextRoundEventBinding);
            
            _startNextMatchEventBinding = new EventBinding<StartNextMatchEvent>(StartNextMatchServerRpc);
            EventBus<StartNextMatchEvent>.Register(_startNextMatchEventBinding);
        }
        
        private void DeregisterServerEvents()
        {
            EventBus<PlayerJoinedEvent>.Deregister(_playerJoinedEventBinding);
            EventBus<TimerExpiredEvent>.Deregister(_timerExpiredEventBinding);
            EventBus<ModuleExplodedEvent>.Deregister(_moduleExplodedEventBinding);
            EventBus<ModuleDefusedEvent>.Deregister(_moduleDefusedEventBinding);
            EventBus<StartNextRoundEvent>.Deregister(_startNextRoundEventBinding);
            EventBus<StartNextMatchEvent>.Deregister(_startNextMatchEventBinding);
        }
        
        private void RegisterPlayer(PlayerJoinedEvent playerJoinedEvent)
        {
            if (!IsServerStarted) return;

            var player = playerJoinedEvent.PlayerController;
            
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
        
        [Server]
        private void HandleTimerExpiredEvent()
        {
            HandleModuleExplodedEvent();
        }
        
        [Server]
        private void HandelModuleDefusedEvent()
        {
            _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _remainingPlayers.Count;
            CheckForNextTurn();
        }
        
        [Server]
        private void HandleModuleExplodedEvent()
        {
            if (_remainingPlayers.Count <= 1) return;
            _remainingPlayers[_currentPlayerIndex.Value].Lose();
            _remainingPlayers.RemoveAt(_currentPlayerIndex.Value);
            _currentPlayerIndex.Value %= _remainingPlayers.Count;
            CheckForNextTurn();
        }
        
        private void ResetPlayers()
        {
            _remainingPlayers.Clear();
            _remainingPlayers.AddRange(_matchPlayers);
            _currentPlayerIndex.Value = 0;
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
        private void StartNextTurn()
        {
            IPlayerController currentPlayer = _remainingPlayers[_currentPlayerIndex.Value];
            currentPlayer.StartTurn();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void StartNextRoundServerRpc()
        {
            ResetPlayers();

            foreach (var player in _remainingPlayers)
            {
                player.StartRound();
            }
            
            StartNextTurn();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void StartNextMatchServerRpc()
        {
            ResetPlayers();

            foreach (var player in _remainingPlayers)
            {
                player.ResetMatchStats();
                player.StartRound();
            }
            
            StartNextTurn();
        }
        
        [Server]
        private void EndRound()
        {
            EventBus<RoundEndedEvent>.Raise(new RoundEndedEvent());
            _remainingPlayers[0].WinRound();
        }
        
        [Server]
        private void EndMatch()
        {
            EventBus<MatchEndedEvent>.Raise(new MatchEndedEvent());
            _remainingPlayers[0].WinMatch();
        }
    }
}