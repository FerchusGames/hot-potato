using System;
using HotPotato.Utilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotPotato.Player
{
    public class OwnedPlayerManager : Singleton<OwnedPlayerManager>
    {
        [SerializeField] private LayerMask _notOnTurnEventMask;
        
        public event Action<bool> OnIsMyTurnUpdate;
        public event Action OnLose;
        public event Action OnWin;
        public event Action OnRoundStarted;
        
        private bool _isMyTurn = false;
        private bool _isStillPlaying = true;
        
        private PhysicsRaycaster _physicsRaycaster = null;
        private static readonly LayerMask EverythingMask = ~0;
            
        protected override void Awake()
        {   
            base.Awake();
            
            _physicsRaycaster = Camera.main.GetComponent<PhysicsRaycaster>();
            _physicsRaycaster.eventMask = _notOnTurnEventMask;
        }

        public void UpdateIsMyTurn(bool turnOwner)
        {
            if (!_isStillPlaying) return;
            
            _isMyTurn = turnOwner;
            OnIsMyTurnUpdate?.Invoke(turnOwner);
            SetModuleInteractivity(_isMyTurn);
        }

        public void StartRound()
        {
            _isStillPlaying = true;
            OnRoundStarted?.Invoke();
        }
        
        public void Lose()
        {
            _isStillPlaying = false;
            OnLose?.Invoke();
            SetModuleInteractivity(false);
        }

        public void Win()
        {
            _isStillPlaying = false;
            OnWin?.Invoke();
            SetModuleInteractivity(false);
        }
        
        public void DisableModuleInteractivity()
        {
            SetModuleInteractivity(false);
        }
        
        private void SetModuleInteractivity(bool interactive)
        {
            if (interactive)
            {
                _physicsRaycaster.eventMask = EverythingMask; 
                return;
            }

            _physicsRaycaster.eventMask = _notOnTurnEventMask;
        }
    }
}