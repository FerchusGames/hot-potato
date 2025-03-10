using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HotPotato.Bomb;
using HotPotato.Clues;
using HotPotato.Player;
using HotPotato.UI;

namespace HotPotato.Managers
{
    public class GameManager : NetworkBehaviour
    {
        public event Action OnTurnChanged;
        
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
            
            if (module.IsTrap)
            {
                module.ExplodeObserversRpc();
                
                _players[_currentPlayerIndex.Value].LoseObserversRpc();
                _players.RemoveAt(_currentPlayerIndex.Value);
                _currentPlayerIndex.Value %= _players.Count;
            }
            else
            {
                _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
            }
            
            module.Despawn();
            StartTurn();
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