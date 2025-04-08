using FishNet.Object;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace HotPotato.Bomb.CenterScreen
{
    public class AbilityDisplay : NetworkBehaviour
    {
        [Required]
        [SerializeField] private CanvasGroup _canvasGroup;

        [Required]
        [SerializeField] private TextMeshProUGUI _text;
        
        EventBinding<AbilityStartedEvent> _abilityStartedEventBinding;
        EventBinding<AbilityFinishedEvent> _abilityFinishedEventBinding;

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
            _abilityStartedEventBinding = new EventBinding<AbilityStartedEvent>(DisplayAbility);
            EventBus<AbilityStartedEvent>.Register(_abilityStartedEventBinding);
            
            _abilityFinishedEventBinding = new EventBinding<AbilityFinishedEvent>(HideAbility);
            EventBus<AbilityFinishedEvent>.Register(_abilityFinishedEventBinding);
        }
        
        private void DeregisterServerEvents()
        {
            EventBus<AbilityStartedEvent>.Deregister(_abilityStartedEventBinding);
            EventBus<AbilityFinishedEvent>.Deregister(_abilityFinishedEventBinding);
        }

        [Server]
        private void DisplayAbility(AbilityStartedEvent abilityStartedEvent)
        {
            string abilityMessage = abilityStartedEvent.Ability.Message;
            
            DisplayAbilityObserversRpc(abilityMessage);
        }
        
        [ObserversRpc]
        private void DisplayAbilityObserversRpc(string abilityMessage)
        {
            _text.text = abilityMessage;
            _canvasGroup.alpha = 1f;
        }
        
        [Server]
        private void HideAbility()
        {
            HideAbilityObserversRpc();
        }
        
        [ObserversRpc]
        private void HideAbilityObserversRpc()
        {
            _canvasGroup.alpha = 0f;
        }
    }
}