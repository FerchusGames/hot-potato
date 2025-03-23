using FishNet.Object;
using FishNet.Object.Synchronizing;
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
            
            _roundEndedEventBinding = new EventBinding<RoundEndedEvent>(StopTimerObserversRpc);
            EventBus<RoundEndedEvent>.Register(_roundEndedEventBinding);
            
            _matchEndedEventBinding = new EventBinding<MatchEndedEvent>(StopTimerObserversRpc);
            EventBus<MatchEndedEvent>.Register(_matchEndedEventBinding);
        }

        private void DeregisterServerEvents()
        {
            EventBus<TurnOwnerChangedEvent>.Deregister(_turnChangedEventBinding);
            EventBus<RoundEndedEvent>.Deregister(_roundEndedEventBinding);
            EventBus<MatchEndedEvent>.Deregister(_matchEndedEventBinding);
        }

        private void Update()
        {
            if (!_isRunning.Value) 
            {
                _text.text = "END";
                return;
            }

            _timer.Update();
    
            if (IsClientStarted)
            {
                _text.text = _timer.Remaining > 0 ? _timer.Remaining.ToString("F2") : "BOOM!";
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
        public void StopTimerObserversRpc()
        {
            StopTimer();
            StopTimerClientRpc();
        }

        [Server]
        private void StopTimer()
        {
            _isRunning.Value = false;
            _timer.StopTimer();
        }

        [ObserversRpc]
        private void StopTimerClientRpc()
        {
            _isRunning.Value = false;
        }
    }
}