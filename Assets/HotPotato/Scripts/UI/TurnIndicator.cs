using HotPotato.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI
{
    public class TurnIndicator : MonoBehaviour
    {
        [SerializeField, Required] private Image _image;

        private EventBinding<RoundStartedEvent> _roundStartedEventBinding;
        private EventBinding<TurnOwnerChangedEvent> _turnOwnerChangedEventBinding;
        private EventBinding<LoseRoundEvent> _loseRoundEventBinding;
        private EventBinding<WinRoundEvent> _winRoundEventBinding;

        private bool _hasLost = false;
        
        private void RegisterEvents()
        {
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(HandleRoundStartedEvent);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
            
            _turnOwnerChangedEventBinding = new EventBinding<TurnOwnerChangedEvent>(SetTurnOwner);
            EventBus<TurnOwnerChangedEvent>.Register(_turnOwnerChangedEventBinding);
            
            _loseRoundEventBinding = new EventBinding<LoseRoundEvent>(LoseRound);
            EventBus<LoseRoundEvent>.Register(_loseRoundEventBinding);
            
            _winRoundEventBinding = new EventBinding<WinRoundEvent>(WinRound);
            EventBus<WinRoundEvent>.Register(_winRoundEventBinding);
        }
        
        private void DeregisterEvents()
        {
            EventBus<RoundStartedEvent>.Deregister(_roundStartedEventBinding);
            EventBus<TurnOwnerChangedEvent>.Deregister(_turnOwnerChangedEventBinding);
            EventBus<LoseRoundEvent>.Deregister(_loseRoundEventBinding);
            EventBus<WinRoundEvent>.Deregister(_winRoundEventBinding);
        }
        
        private void Start()
        {
            RegisterEvents();
        }

        private void OnDestroy()
        {
            DeregisterEvents();
        }

        private void SetTurnOwner(TurnOwnerChangedEvent turnOwnerChangedEvent)
        {
            if (_hasLost) return;
            
            _image.color = turnOwnerChangedEvent.IsMyTurn ? Color.green : Color.red;
        }

        private void HandleRoundStartedEvent()
        {
            _hasLost = false;
        }
        
        private void LoseRound()
        {
            _hasLost = true;
            _image.color = Color.black;
        }

        private void WinRound()
        {
            _image.color = Color.yellow;
        }
    }
}