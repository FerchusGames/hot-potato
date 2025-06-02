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
        private EventBinding<ModuleInteractedEvent> _enterStateEventBinding;
        private EventBinding<ModuleInteractedExitStateEvent> _exitStateEventBinding;
        
        private BombModule _interactedModule;
        
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
            _enterStateEventBinding = new EventBinding<ModuleInteractedEvent>(DisplayModule);
            EventBus<ModuleInteractedEvent>.Register(_enterStateEventBinding);
            
            _exitStateEventBinding = new EventBinding<ModuleInteractedExitStateEvent>(HideModule);
            EventBus<ModuleInteractedExitStateEvent>.Register(_exitStateEventBinding);
        }
        
        private void DeregisterServerEvents()
        {
            EventBus<ModuleInteractedEvent>.Deregister(_enterStateEventBinding);
            EventBus<ModuleInteractedExitStateEvent>.Deregister(_exitStateEventBinding);
        }
        
        [Server]
        private void DisplayModule(ModuleInteractedEvent enterStateEvent)
        {
            _interactedModule = enterStateEvent.Module;
            
            _interactedModule.transform.position = transform.position;
            _interactedModule.transform.rotation = transform.rotation;
            _interactedModule.transform.localScale = transform.localScale;
        }
        

        [Server]
        private void HideModule()
        {
            _interactedModule.Despawn();
        }
    }
}