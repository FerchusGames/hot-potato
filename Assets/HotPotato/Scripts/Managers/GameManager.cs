using FishNet.Object;
using HotPotato.Clues;

namespace HotPotato.Managers
{
    public class GameManager : NetworkBehaviour
    {
        private EventBinding<ModulesSpawnedEvent> _modulesSpawnedEventBinding;
        private EventBinding<ModuleClickedEvent> _moduleClickedEventBinding;

        public override void OnStartNetwork()
        {
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            RegisterServerEvents();
        }

        public override void OnStopServer()
        {
            DeregisterServerEvents();
        }

        public override void OnStartClient()
        {
            RegisterClientEvents();
        }

        public override void OnStopClient()
        {
            DeregisterClientEvents();
        }

        private void RegisterServerEvents()
        {
            _modulesSpawnedEventBinding = new EventBinding<ModulesSpawnedEvent>(SetCurrentRoundModuleSettings);
            EventBus<ModulesSpawnedEvent>.Register(_modulesSpawnedEventBinding);
        }

        private void DeregisterServerEvents()
        {
            EventBus<ModulesSpawnedEvent>.Deregister(_modulesSpawnedEventBinding);
        }

        private void RegisterClientEvents()
        {
            _moduleClickedEventBinding = new EventBinding<ModuleClickedEvent>(InteractWithModuleServerRpc);
            EventBus<ModuleClickedEvent>.Register(_moduleClickedEventBinding);
        }

        private void DeregisterClientEvents()
        {
            EventBus<ModuleClickedEvent>.Deregister(_moduleClickedEventBinding);
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractWithModuleServerRpc(ModuleClickedEvent moduleClickedEvent)
        {
            if (!IsServerStarted) return;

            var module = moduleClickedEvent.Module;
            
            var moduleInteractedEvent = new ModuleInteractedEvent
            {
                Settings = module.GetSettings()
            };

            EventBus<ModuleInteractedEvent>.Raise(moduleInteractedEvent);
            
            module.Despawn();
        }

        [Server]
        private void SetCurrentRoundModuleSettings(ModulesSpawnedEvent modulesSpawnedEvent)
        {
            var clueData = new ClueData(modulesSpawnedEvent.SettingsList, true);
            
            EventBus<GeneratedClueDataEvent>.Raise(new GeneratedClueDataEvent
            {
                ClueData = clueData
            });
        }
    }
}