using FishNet.Object;

namespace HotPotato.Managers
{
    public interface IGameManager
    {
        void AddPlayer(NetworkBehaviour player);
        void ConfirmCurrentTurn();
        bool IsPlayerTurn(NetworkBehaviour player);
    }
}