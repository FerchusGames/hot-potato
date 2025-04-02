using System.Linq;
using FishNet.Object;
using HotPotato.GameFlow.TurnStateMachine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HotPotato.Bomb.CenterScreen
{
    public class InteractedModuleDisplay : NetworkBehaviour
    {
        [Required]
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [Required]
        [SerializeField] private TextMeshProUGUI _text;

        EventBinding<ModuleInteractedEnterStateEvent> _enterStateEventBinding;
        EventBinding<ModuleInteractedExitStateEvent> _exitStateEventBinding;
        
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
            _enterStateEventBinding = new EventBinding<ModuleInteractedEnterStateEvent>(DisplayModule);
            EventBus<ModuleInteractedEnterStateEvent>.Register(_enterStateEventBinding);
            
            _exitStateEventBinding = new EventBinding<ModuleInteractedExitStateEvent>(HideModuleObserversRpc);
            EventBus<ModuleInteractedExitStateEvent>.Register(_exitStateEventBinding);
        }
        
        private void DeregisterServerEvents()
        {
            EventBus<ModuleInteractedEnterStateEvent>.Deregister(_enterStateEventBinding);
            EventBus<ModuleInteractedExitStateEvent>.Deregister(_exitStateEventBinding);
        }
        
        [Server]
        private void DisplayModule(ModuleInteractedEnterStateEvent enterStateEvent)
        {
            var textToShow = string.Join("\n", enterStateEvent.Settings.GetType()
                .GetFields()
                .Select(field => $"{field.Name}: {field.GetValue(enterStateEvent.Settings)}"));
            
            DisplayModuleObserversRpc(textToShow);
        }
        
        [ObserversRpc]
        private void DisplayModuleObserversRpc(string textToShow)
        {
            _text.text = textToShow;
            _canvasGroup.alpha = 1f;
        }

        [ObserversRpc]
        private void HideModuleObserversRpc()
        {
            _canvasGroup.alpha = 0f;
        }
    }
}