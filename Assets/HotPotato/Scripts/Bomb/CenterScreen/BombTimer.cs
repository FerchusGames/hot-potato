using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.GameFlow.TurnStateMachine;
using TMPro;
using UnityEngine;

namespace HotPotato.Bomb.CenterScreen
{
    public class BombTimer : NetworkBehaviour
    {
        [SerializeField] private int _initialTime = 20;

        [SerializeField] private TextMeshProUGUI _text;
        
        private readonly SyncTimer _timer = new();
        private readonly SyncVar<bool> _isRunning = new(true);
        
        private EventBinding<BombTickingUpdateStateEvent> _updateStateEventBinding;
        private EventBinding<BombTickingEnterStateEvent> _enterStateEventBinding;
        private EventBinding<BombTickingExitStateEvent> _exitStateEventBinding;
        
        private bool _timerExpired = false;
    
        public override void OnStartServer()
        {
            RegisterServerEvents();
        }
        
        public override void OnStopServer()
        {
            DeregisterServerEvents();
        }

        private void RegisterServerEvents()
        {
            _updateStateEventBinding = new EventBinding<BombTickingUpdateStateEvent>(UpdateTimer);
            EventBus<BombTickingUpdateStateEvent>.Register(_updateStateEventBinding);
            
            _enterStateEventBinding = new EventBinding<BombTickingEnterStateEvent>(ResetTimer);
            EventBus<BombTickingEnterStateEvent>.Register(_enterStateEventBinding);
            
            _exitStateEventBinding = new EventBinding<BombTickingExitStateEvent>(StopTimerServerRpc);
            EventBus<BombTickingExitStateEvent>.Register(_exitStateEventBinding);
        }

        private void DeregisterServerEvents()
        {
            EventBus<BombTickingUpdateStateEvent>.Deregister(_updateStateEventBinding);
            EventBus<BombTickingEnterStateEvent>.Deregister(_enterStateEventBinding);
            EventBus<BombTickingExitStateEvent>.Deregister(_exitStateEventBinding);
        }
        
        [ObserversRpc]
        private void UpdateTimer()
        {
            _timer.Update();
    
            if (IsClientStarted)
            {
                var shownTime = Mathf.CeilToInt(_timer.Remaining).ToString();
                _text.text = shownTime;
            }
        
            CheckTimer();
        }

        [Server]
        private void CheckTimer()
        {
            if (_timer.Remaining <= 0 && !_timerExpired)
            {
                _timerExpired = true;
                EventBus<TimerExpiredEvent>.Raise(new TimerExpiredEvent());
            }
        }
    
        [Server]
        private void ResetTimer()
        {
            _isRunning.Value = true;
            _timerExpired = false;
            _timer.StartTimer(_initialTime);
            SetVisibilityObserversRpc(true);
        }
    
        [Server]
        private void StopTimerServerRpc()
        {
            SetVisibilityObserversRpc(false);
            _isRunning.Value = false;
            _timer.StopTimer();
            StopTimerClientRpc();
        }

        [ObserversRpc]
        private void StopTimerClientRpc()
        {
            _isRunning.Value = false;
        }

        [ObserversRpc]
        private void SetVisibilityObserversRpc(bool isVisible)
        {
            if (isVisible)
            {
                _text.alpha = 1;
                return;
            }
            
            _text.alpha = 0;
        }
    }
}