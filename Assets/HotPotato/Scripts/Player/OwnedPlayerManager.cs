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
        public bool IsMyTurn { get; private set; } = false;
        
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
            IsMyTurn = turnOwner;
            OnIsMyTurnUpdate?.Invoke(turnOwner);
            SetModuleInteractivity(IsMyTurn);
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