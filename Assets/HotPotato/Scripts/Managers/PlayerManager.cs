using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.GameFlow.TurnStateMachine;
using HotPotato.Player;
using UnityEngine;

namespace HotPotato.Managers
{
    public class PlayerManager : NetworkBehaviour
    {
        [SerializeField] private int _roundsToWin = 3;
        
        private readonly SyncVar<int> _currentPlayerIndex = new();

        private int _initialPlayerCount;
        private int _currentPlayerCount;
        
        private List<IPlayerController> _matchPlayers = new();
        private List<IPlayerController> _remainingPlayers = new();

        private IPlayerController CurrentPlayer => _remainingPlayers[_currentPlayerIndex.Value];
        
        private EventBinding<TransportingClientsToSceneEvent> _transportingClientsToSceneEventBinding;
        private EventBinding<PlayerJoinedEvent> _playerJoinedEventBinding;
        private EventBinding<StartNextRoundEvent> _startNextRoundEventBinding;
        private EventBinding<StartNextMatchEvent> _startNextMatchEventBinding;
        
        private EventBinding<MovingBombEnterStateEvent> _movingBombEnterStateEventBinding;
        private EventBinding<TurnStartEnterStateEvent> _turnStartEnterStateEventBinding;
        private EventBinding<ModuleExplodedExitStateEvent> _moduleExplodedExitStateEventBinding;
        private EventBinding<ModuleDefusedExitStateEvent> _moduleDefusedExitStateEventBinding;
        
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
            _transportingClientsToSceneEventBinding = new EventBinding<TransportingClientsToSceneEvent>(SetPlayerCount);
            EventBus<TransportingClientsToSceneEvent>.Register(_transportingClientsToSceneEventBinding);
            
            _playerJoinedEventBinding = new EventBinding<PlayerJoinedEvent>(RegisterPlayer);
            EventBus<PlayerJoinedEvent>.Register(_playerJoinedEventBinding);
            
            _startNextRoundEventBinding = new EventBinding<StartNextRoundEvent>(StartNextRoundServerRpc);
            EventBus<StartNextRoundEvent>.Register(_startNextRoundEventBinding);
            
            _startNextMatchEventBinding = new EventBinding<StartNextMatchEvent>(StartNextMatchServerRpc);
            EventBus<StartNextMatchEvent>.Register(_startNextMatchEventBinding);
            
            _movingBombEnterStateEventBinding = new EventBinding<MovingBombEnterStateEvent>(StartMovingBomb);
            EventBus<MovingBombEnterStateEvent>.Register(_movingBombEnterStateEventBinding);
            
            _turnStartEnterStateEventBinding = new EventBinding<TurnStartEnterStateEvent>(OnTurnStart);
            EventBus<TurnStartEnterStateEvent>.Register(_turnStartEnterStateEventBinding);
            
            _moduleExplodedExitStateEventBinding = new EventBinding<ModuleExplodedExitStateEvent>(HandleModuleExplodedExitStateEvent);
            EventBus<ModuleExplodedExitStateEvent>.Register(_moduleExplodedExitStateEventBinding);
            
            _moduleDefusedExitStateEventBinding = new EventBinding<ModuleDefusedExitStateEvent>(HandleModuleDefusedExitStateEvent);
            EventBus<ModuleDefusedExitStateEvent>.Register(_moduleDefusedExitStateEventBinding);
        }
        
        private void DeregisterServerEvents()
        {
            EventBus<TransportingClientsToSceneEvent>.Deregister(_transportingClientsToSceneEventBinding);
            EventBus<PlayerJoinedEvent>.Deregister(_playerJoinedEventBinding);
            EventBus<StartNextRoundEvent>.Deregister(_startNextRoundEventBinding);
            EventBus<StartNextMatchEvent>.Deregister(_startNextMatchEventBinding);
            
            EventBus<MovingBombEnterStateEvent>.Deregister(_movingBombEnterStateEventBinding);
            EventBus<TurnStartEnterStateEvent>.Deregister(_turnStartEnterStateEventBinding);
            EventBus<ModuleExplodedExitStateEvent>.Deregister(_moduleExplodedExitStateEventBinding);
            EventBus<ModuleDefusedExitStateEvent>.Deregister(_moduleDefusedExitStateEventBinding);
        }

        private void SetPlayerCount(TransportingClientsToSceneEvent transportingClientsToSceneEvent)
        {
            _initialPlayerCount = transportingClientsToSceneEvent.PlayerCount;
            _currentPlayerCount = 0;
        }
        
        private void RegisterPlayer(PlayerJoinedEvent playerJoinedEvent)
        {
            if (!IsServerStarted) return;

            var player = playerJoinedEvent.PlayerController;
            
            if (_matchPlayers.Count == 0)
            {
                _matchPlayers = new List<IPlayerController>(new IPlayerController[10]);
                _remainingPlayers = new List<IPlayerController>(new IPlayerController[10]);
            }
            
            if (!_matchPlayers.Contains(player))
            {
                AddPlayer(player);
                
                if (_currentPlayerCount != _initialPlayerCount) return;
                
                RemoveEmptyPlayersFromLists();
                StartNextRoundServerRpc();
            }
        }

        private void AddPlayer(IPlayerController playerController)
        {
            int orderIndex = 0;

            switch (_currentPlayerCount)
            {
                case 0:
                    orderIndex = 0;
                    break;
                case 1:
                    orderIndex = 2;
                    break;
                case 2:
                    orderIndex = 1;
                    break;
                case 3:
                    orderIndex = 3;
                    break;
            }
            
            _matchPlayers[orderIndex] = playerController;
            _remainingPlayers[orderIndex] = playerController;
            
            _currentPlayerCount++;
        }

        private void RemoveEmptyPlayersFromLists()
        {
            _matchPlayers.RemoveAll(item => item == null);
            _remainingPlayers.RemoveAll(item => item == null);
        }
        
        private void OnTurnStart()
        {
            CheckForNextTurn();
        }
        
        private void OnDefuseBomb()
        {
            _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _remainingPlayers.Count;
        }

        private void OnExplodeBomb()
        {
            if (_remainingPlayers.Count <= 1) return;
            _remainingPlayers[_currentPlayerIndex.Value].Lose();
            _remainingPlayers.RemoveAt(_currentPlayerIndex.Value);
            _currentPlayerIndex.Value %= _remainingPlayers.Count;
        }
        
        [Server]
        private void HandleModuleExplodedExitStateEvent(ModuleExplodedExitStateEvent moduleExplodedExitStateEvent)
        {
            OnExplodeBomb();
        }
        
        [Server]
        private void HandleModuleDefusedExitStateEvent(ModuleDefusedExitStateEvent moduleDefusedExitStateEvent)
        {
            OnDefuseBomb();
        }
        
        private void ResetPlayers()
        {
            IPlayerController standingPlayer = _remainingPlayers[0];
            
            _remainingPlayers.Clear();
            _remainingPlayers.AddRange(_matchPlayers);
            
            var standingIndex = _remainingPlayers.IndexOf(standingPlayer);
            var reorderedList = 
                _remainingPlayers.Skip(standingIndex).Concat(_remainingPlayers.Take(standingIndex)).ToList();
            
            _remainingPlayers = reorderedList;
            
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
        private void StartMovingBomb()
        {
            CurrentPlayer.RequestToMoveBomb();
        }
        
        [Server]
        private void StartNextTurn()
        {
            CurrentPlayer.StartTurn();
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