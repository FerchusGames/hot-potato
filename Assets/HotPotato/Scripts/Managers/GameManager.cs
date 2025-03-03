using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.Player;

namespace HotPotato.Managers
{
    public class GameManager : NetworkBehaviour, IGameManager
    {
        // private readonly SyncList<NetworkBehaviour> _players = new();
        // private readonly SyncVar<int> _currentPlayerIndex = new();
        //
        // public override void OnStartServer()
        // {
        //     base.OnStartServer();
        //     _currentPlayerIndex.Value = 0;
        // }
        //
        // [ServerRpc]
        // public void AddPlayer(NetworkBehaviour player)
        // {
        //     if (!IsServerStarted) return;
        //     _players.Add(player);
        // }
        //
        // [ServerRpc]
        // public void ConfirmCurrentTurn()
        // {
        //     if (!IsServerStarted || _players.Count == 0) return;
        //
        //     _currentPlayerIndex.Value = (_currentPlayerIndex.Value + 1) % _players.Count;
        //     UpdatePlayerTurns();
        // }
        //
        // [ObserversRpc(ExcludeOwner = false)]
        // private void UpdatePlayerTurns()
        // {
        //     for (int i = 0; i < _players.Count; i++)
        //     {
        //         bool isCurrentPlayer = i == _currentPlayerIndex.Value;
        //         if (_players[i] is IPlayer player && isCurrentPlayer)
        //         {
        //             player.ConfirmTurn();
        //         }
        //     }
        // }
        //
        // public bool IsPlayerTurn(NetworkBehaviour player)
        // {
        //     return _players.IndexOf(player) == _currentPlayerIndex.Value;
        // }
        public void AddPlayer(NetworkBehaviour player)
        {
            throw new System.NotImplementedException();
        }

        public void ConfirmCurrentTurn()
        {
            throw new System.NotImplementedException();
        }

        public bool IsPlayerTurn(NetworkBehaviour player)
        {
            throw new System.NotImplementedException();
        }
    }
}