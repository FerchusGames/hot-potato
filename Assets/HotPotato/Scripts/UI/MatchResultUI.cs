using HotPotato.Player;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI
{
    public class MatchResultUI : MonoBehaviour
    {
        [Required]
        [SerializeField] private Image _background;
        
        [Required]
        [SerializeField] private TextMeshProUGUI _resultText;

        private EventBinding<WinMatchEvent> _winMatchEventBinding;
        private EventBinding<LoseMatchEvent> _loseMatchEventBinding;
        private EventBinding<RoundStartedEvent> _roundStartedEventBinding;
        
        private void Start()
        {
            RegisterEvents();

            HideResult();
        }

        private void OnDestroy()
        {
            DeregisterEvents();
        }

        private void ShowWinResult(WinMatchEvent winMatchEvent)
        {
            gameObject.SetActive(true);
            _background.color = Color.yellow;
            _resultText.text = "You've won the match!";
        }
        
        private void ShowLoseResult()
        {
            gameObject.SetActive(true);
            _background.color = Color.red;
            _resultText.text = "You've lost the match!";
        }

        private void HideResult()
        {
            gameObject.SetActive(false);
        }
        
        private void RegisterEvents()
        {
            _winMatchEventBinding = new EventBinding<WinMatchEvent>(ShowWinResult);
            EventBus<WinMatchEvent>.Register(_winMatchEventBinding);
            
            _loseMatchEventBinding = new EventBinding<LoseMatchEvent>(ShowLoseResult);
            EventBus<LoseMatchEvent>.Register(_loseMatchEventBinding);
            
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(HideResult);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
        }
        
        private void DeregisterEvents()
        {
            EventBus<WinMatchEvent>.Deregister(_winMatchEventBinding);
            EventBus<LoseMatchEvent>.Deregister(_loseMatchEventBinding);
            EventBus<RoundStartedEvent>.Deregister(_roundStartedEventBinding);
        }
    }
}
