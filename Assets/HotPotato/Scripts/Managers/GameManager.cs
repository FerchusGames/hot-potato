using FishNet.Object;
using System.Collections.Generic;
using HotPotato.Bomb;
using HotPotato.Clues;
using HotPotato.UI;

namespace HotPotato.Managers
{
    public class GameManager : NetworkBehaviour
    {
        private EventBinding<ModulesSpawnedEvent> _modulesSpawnedEventBinding;
        private EventBinding<ModuleClickedEvent> _moduleClickedEventBinding;

        private List<BombModuleSettings> _bombModuleSettingsList = new();

        private ClueData _clueData;

        private UIManager UIManager => base.NetworkManager.GetInstance<UIManager>();

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

            if (module.IsTrap)
            {
                module.ExplodeObserversRpc();
                EventBus<ModuleExplodedEvent>.Raise(new ModuleExplodedEvent());
            }
            else
            {
                EventBus<ModuleDefusedEvent>.Raise(new ModuleDefusedEvent());
            }

            module.Despawn();
        }

        [Server]
        private void SetCurrentRoundModuleSettings(ModulesSpawnedEvent modulesSpawnedEvent)
        {
            var settingsList = modulesSpawnedEvent.SettingsList;

            _bombModuleSettingsList = settingsList;
            _clueData = new ClueData(settingsList, false);
            UIManager.SetClueData(_clueData);
        }
    }
}