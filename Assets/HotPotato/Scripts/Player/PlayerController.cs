using System;
using DG.Tweening;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.AbilitySystem;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HotPotato.Player
{
    public class PlayerController : NetworkBehaviour, IPlayerController
    {
        public int WinCount => _winCount.Value;

        private readonly SyncVar<int> _winCount = new();
        
        [Required]
        [SerializeField] private AbilityController _abilityControllerObject;
        
        [SerializeField] private int _rendererLayer;
        [SerializeField] private GameObject _rendererParent;
        
        [SerializeField] private float _swaySpeed;
        [SerializeField] private float _swayAmount;
        private float _timeOffset;
        
        private IAbilityController AbilityController => _abilityControllerObject as IAbilityController;
        
        public override void OnStartServer()
        {
            EventBus<PlayerJoinedEvent>.Raise(new PlayerJoinedEvent 
            {
                PlayerController = this
            });
        }

        public override void OnStartClient()
        {
            StartSwaying();
            
            if (!IsOwner) return;
            
            EventBus<OwnedPlayerSpawnedEvent>.Raise(new OwnedPlayerSpawnedEvent
            {
                PlayerObject = gameObject,
            });
            
            SetLayerRecursively(_rendererParent, _rendererLayer);
        }

        private void SetLayerRecursively(GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }
        
        [Server]
        public void RequestToMoveBomb()
        {
            EventBus<MoveBombToPlayerEvent>.Raise(new MoveBombToPlayerEvent
            {
                PlayerPosition = transform.position,
            });
        }
        
        [Server]
        public void ResetMatchStats()
        {
            _winCount.Value = 0;
            ResetMatchStatsObserversRpc();
        }
        
        [ObserversRpc]
        private void ResetMatchStatsObserversRpc()
        {
            if (!IsOwner) return;
            EventBus<MatchResetEvent>.Raise(new MatchResetEvent());
        }

        [Server]
        public void StartRound()
        {
            StartRoundObserversRpc();
        }
        
        [ObserversRpc]
        private void StartRoundObserversRpc()
        {
            if (!IsOwner) return;
            EventBus<RoundStartedEvent>.Raise(new RoundStartedEvent());
        }

        [Server]
        public void StartTurn()
        {
            AbilityController.EnableAbility();
            StartTurnObserversRpc();
        }
        
        [ObserversRpc]
        private void StartTurnObserversRpc()
        {
            EventBus<TurnOwnerChangedEvent>.Raise(new TurnOwnerChangedEvent
            {
                IsMyTurn = IsOwner, 
            });
        }
        
        public void Lose()
        {
            LoseObserversRpc();
        }

        [ObserversRpc]
        private void LoseObserversRpc()
        {
            if (!IsOwner) return;
            EventBus<LoseRoundEvent>.Raise(new LoseRoundEvent());
        }

        [Server]
        public void WinRound()
        {
            _winCount.Value++;
            WinRoundObserversRpc(_winCount.Value);
        }

        [ObserversRpc]
        private void WinRoundObserversRpc(int winCount)
        {
            if (!IsOwner) return;
            EventBus<WinRoundEvent>.Raise(new WinRoundEvent
            {
                WinCount = winCount
            });
        }

        [Server]
        public void WinMatch()
        {
            _winCount.Value++;
            WinMatchObserversRpc(_winCount.Value);
        }
        
        [ObserversRpc]
        private void WinMatchObserversRpc(int winCount)
        {
            if (IsOwner)
            {
                EventBus<WinMatchEvent>.Raise(new WinMatchEvent
                {
                    WinCount = winCount
                });
                return;
            }
            
            EventBus<LoseMatchEvent>.Raise(new LoseMatchEvent());
        }

        private void StartSwaying()
        {
            _timeOffset = Random.Range(0f, 2f * Mathf.PI);
        }

        private void Update()
        {
            float sway = Mathf.Sin((Time.time * _swaySpeed) + _timeOffset) * _swayAmount;
            Vector3 newPosition = _rendererParent.transform.localPosition;
            newPosition.x += sway - (sway * Time.deltaTime);
            _rendererParent.transform.localPosition = newPosition;
        }
    }
}