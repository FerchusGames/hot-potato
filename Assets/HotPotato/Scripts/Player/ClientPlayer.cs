using FishNet.Object;
using HotPotato.Managers;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace HotPotato.Player
{
    public class ClientPlayer : NetworkBehaviour, IPlayer
    {
        private bool _isMyTurn;
        
        private IGameManager _gameManager;
        
        public void Construct(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }
        
        public override void OnStartClient()
        {
            base.OnStartClient();

            if (IsOwner)
            {
                _gameManager.AddPlayer(this);
            }
        }
        
        private void Update()
        {
            if (!IsOwner || !_isMyTurn) return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _gameManager.ConfirmCurrentTurn();
                _isMyTurn = false;
            }
        }

        public void ConfirmTurn()
        {
            if (!IsOwner) return;

            _isMyTurn = true;
            Debug.Log("It's your turn! Press SPACE to end turn.");
        }
    }
}