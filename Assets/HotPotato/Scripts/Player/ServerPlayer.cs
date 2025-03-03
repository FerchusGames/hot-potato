using FishNet.Object;
using HotPotato.Managers;
using VContainer;

namespace HotPotato.Player
{
    public class ServerPlayer : NetworkBehaviour, IPlayer
    {
        [Inject] private IGameManager _gameManager;
        
        public void ConfirmTurn()
        {
            // Server-side turn confirmation if needed
            // Usually empty as the server manages turns through GameManager
        }
    }
}