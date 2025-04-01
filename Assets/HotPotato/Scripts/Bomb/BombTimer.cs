using FishNet.Object;
using FishNet.Object.Synchronizing;
using HotPotato.GameFlow.StateMachine.ConcreteStates;
using TMPro;
using UnityEngine;

namespace HotPotato.Bomb
{
    public class BombTimer : NetworkBehaviour
    {
        [SerializeField] private int _initialTime = 20;

        [SerializeField] private TextMeshProUGUI _text;
    
        private readonly SyncTimer _timer = new();
        private readonly SyncVar<bool> _isRunning = new(true);
    
        private EventBinding<TurnOwnerChangedEvent> _turnChangedEventBinding;
        private EventBinding<RoundEndedEvent> _roundEndedEventBinding;
        private EventBinding<MatchEndedEvent> _matchEndedEventBinding;
        
        private EventBinding<BombTickingState.UpdateStateEvent> _updateStateEventBinding;
        private EventBinding<BombTickingState.EnterStateEvent> _enterStateEventBinding;
        private EventBinding<BombTickingState.ExitStateEvent> _exitStateEventBinding;
    
        private float _remainingTime;
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
            _turnChangedEventBinding = new EventBinding<TurnOwnerChangedEvent>(ResetTimer);
            EventBus<TurnOwnerChangedEvent>.Register(_turnChangedEventBinding);
            
            _roundEndedEventBinding = new EventBinding<RoundEndedEvent>(StopTimerServerRpc);
            EventBus<RoundEndedEvent>.Register(_roundEndedEventBinding);
            
            _matchEndedEventBinding = new EventBinding<MatchEndedEvent>(StopTimerServerRpc);
            EventBus<MatchEndedEvent>.Register(_matchEndedEventBinding);
            
            _updateStateEventBinding = new EventBinding<BombTickingState.UpdateStateEvent>(UpdateTimer);
            EventBus<BombTickingState.UpdateStateEvent>.Register(_updateStateEventBinding);
            
            _enterStateEventBinding = new EventBinding<BombTickingState.EnterStateEvent>(ReturnToTimer);
            EventBus<BombTickingState.EnterStateEvent>.Register(_enterStateEventBinding);
            
            _exitStateEventBinding = new EventBinding<BombTickingState.ExitStateEvent>(StopTimerServerRpc);
            EventBus<BombTickingState.ExitStateEvent>.Register(_exitStateEventBinding);
        }

        private void DeregisterServerEvents()
        {
            EventBus<TurnOwnerChangedEvent>.Deregister(_turnChangedEventBinding);
            EventBus<RoundEndedEvent>.Deregister(_roundEndedEventBinding);
            EventBus<MatchEndedEvent>.Deregister(_matchEndedEventBinding);
            EventBus<BombTickingState.UpdateStateEvent>.Deregister(_updateStateEventBinding);
            EventBus<BombTickingState.EnterStateEvent>.Deregister(_enterStateEventBinding);
            EventBus<BombTickingState.ExitStateEvent>.Deregister(_exitStateEventBinding);
        }

        private void ReturnToTimer()
        {
            _isRunning.Value = true;
            _timerExpired = false;
            _timer.StartTimer(Mathf.Ceil(_remainingTime));
        }
        
        [ObserversRpc]
        private void UpdateTimer()
        {
            if (!_isRunning.Value)
            {
                _text.text = "END";
                return;
            }

            _timer.Update();
    
            if (IsClientStarted)
            {
                var shownTime = Mathf.CeilToInt(_timer.Remaining).ToString();
                _text.text = _timer.Remaining > 0 ? shownTime : "BOOM!";
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
        }
    
        [ServerRpc(RequireOwnership = false)]
        private void StopTimerServerRpc()
        {
            StopTimer();
            StopTimerClientRpc();
        }

        [Server]
        private void StopTimer()
        {
            _isRunning.Value = false;
            _remainingTime = _timer.Remaining;
            _timer.StopTimer();
        }

        [ObserversRpc]
        private void StopTimerClientRpc()
        {
            _isRunning.Value = false;
        }
    }
}