using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace HotPotato.Visual
{
    public class DeadPostProcessingController : MonoBehaviour
    {
        [SerializeField, Required] private Volume _postProcessingVolume;
        [SerializeField, Required] private CanvasGroup _blindingImage;
        [SerializeField] private Ease _fadeInEase = Ease.InOutSine;
        [SerializeField] private Ease _fadeOutEase = Ease.InOutSine;
        [SerializeField] private float _fadeInDuration = 0.2f;
        [SerializeField] private float _holdDuration = 0.5f;
        [SerializeField] private float _fadeOutDuration = 1f;
        
        private EventBinding<LoseRoundEvent> _loseRoundEventBinding;
        private EventBinding<RoundStartedEvent> _roundStartedEventBinding;
        private EventBinding<LoseMatchEvent> _loseMatchEventBinding;

        private Sequence _blindingImageSequence;
        
        private bool _isFirstRound = true;
        
        private void Start()
        {
            _loseRoundEventBinding = new EventBinding<LoseRoundEvent>(EnableVolume);
            EventBus<LoseRoundEvent>.Register(_loseRoundEventBinding);
            
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(DisableVolume);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
            
            _loseMatchEventBinding = new EventBinding<LoseMatchEvent>(DisableVolume);
            EventBus<LoseMatchEvent>.Register(_loseMatchEventBinding);
        }

        private void EnableVolume()
        {
            _blindingImage.alpha = 1f;
            
            _blindingImageSequence?.Kill();
            _blindingImageSequence = DOTween.Sequence();
            _blindingImageSequence.AppendCallback(() => _blindingImage.alpha = 1f);
            _blindingImageSequence.AppendCallback(() => _postProcessingVolume.enabled = true);
            _blindingImageSequence.AppendInterval(_holdDuration);
            _blindingImageSequence.Append(_blindingImage.DOFade(0f, _fadeOutDuration).SetEase(_fadeOutEase));
        }

        private void DisableVolume()
        {
            if (_isFirstRound)
            {
                _isFirstRound = false;
                _blindingImage.alpha = 0f;
                return;
            }
            
            _blindingImageSequence?.Kill();
            _blindingImageSequence = DOTween.Sequence();
            _blindingImageSequence.Append(_blindingImage.DOFade(1f, _fadeInDuration).SetEase(_fadeInEase));
            _blindingImageSequence.AppendCallback(() => _postProcessingVolume.enabled = false);
            _blindingImageSequence.AppendInterval(_holdDuration);
            _blindingImageSequence.Append(_blindingImage.DOFade(0f, _fadeOutDuration).SetEase(_fadeOutEase));
        }
    }
}