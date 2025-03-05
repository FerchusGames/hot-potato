using FishNet.Object;
using HotPotato.Managers;
using HotPotato.Player;
using UnityEngine.EventSystems;

namespace HotPotato.Bomb
{
    public class BombModuleType : NetworkBehaviour, IPointerClickHandler
    {
        private GameManager _gameManager;
        
        public override void OnStartClient()
        {
            _gameManager = base.NetworkManager.GetInstance<GameManager>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _gameManager.EndTurnServerRpc();
            OwnedPlayerManager.Instance.DisableModuleInteractivity();
        }
    }
}