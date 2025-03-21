using HotPotato.Player;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HotPotato.UI
{
    public class WinCounterUI : MonoBehaviour
    {
        [Required]
        [SerializeField] private TextMeshProUGUI _text;
    
        private EventBinding<WinRoundEvent> _winRoundEventBinding;
        private EventBinding<WinMatchEvent> _winMatchEventBinding;
        private EventBinding<MatchResetEvent> _resetMatchStatsEventBinding;
        
        private void Start()
        {
            RegisterEvents();
        }

        private void OnDestroy()
        {
            DeregisterEvents();
        }

        private void UpdateWinRoundCount(WinRoundEvent winRoundEvent)
        {
            _text.text = winRoundEvent.winCount.ToString();
        }
        
        private void UpdateWinRoundCount(WinMatchEvent winMatchEvent)
        {
            _text.text = winMatchEvent.winCount.ToString();
        }
        
        private void ResetWinRoundCount()
        {
            _text.text = "0";
        }
        
        private void RegisterEvents()
        {
            _winRoundEventBinding = new EventBinding<WinRoundEvent>(UpdateWinRoundCount);
            EventBus<WinRoundEvent>.Register(_winRoundEventBinding);
            
            _winMatchEventBinding = new EventBinding<WinMatchEvent>(UpdateWinRoundCount);
            EventBus<WinMatchEvent>.Register(_winMatchEventBinding);
            
            _resetMatchStatsEventBinding = new EventBinding<MatchResetEvent>(ResetWinRoundCount);
            EventBus<MatchResetEvent>.Register(_resetMatchStatsEventBinding);
        }

        private void DeregisterEvents()
        {
            EventBus<WinRoundEvent>.Deregister(_winRoundEventBinding);
            EventBus<WinMatchEvent>.Deregister(_winMatchEventBinding);
            EventBus<MatchResetEvent>.Deregister(_resetMatchStatsEventBinding);
        }
    }
}