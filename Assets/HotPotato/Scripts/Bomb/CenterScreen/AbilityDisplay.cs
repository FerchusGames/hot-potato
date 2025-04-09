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
        
        EventBinding<AbilityPlayingEvent> _abilityPlayingEventBinding;
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
            _abilityPlayingEventBinding = new EventBinding<AbilityPlayingEvent>(DisplayAbility);
            EventBus<AbilityPlayingEvent>.Register(_abilityPlayingEventBinding);
            
            _abilityFinishedEventBinding = new EventBinding<AbilityFinishedEvent>(HideAbility);
            EventBus<AbilityFinishedEvent>.Register(_abilityFinishedEventBinding);
        }
        
        private void DeregisterServerEvents()
        {
            EventBus<AbilityPlayingEvent>.Deregister(_abilityPlayingEventBinding);
            EventBus<AbilityFinishedEvent>.Deregister(_abilityFinishedEventBinding);
        }

        [Server]
        private void DisplayAbility(AbilityPlayingEvent abilityPlayingEvent)
        {
            string abilityMessage = abilityPlayingEvent.Ability.Message;
            
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