using FishNet.Managing.Scened;
using FishNet.Object;

namespace HotPotato.Managers
{
    public class SceneManager : NetworkBehaviour
    {
        private EventBinding<ChangeSceneRequestEvent> _changeSceneRequestEventBinding;
        
        public override void OnStartServer()
        {
            _changeSceneRequestEventBinding = new EventBinding<ChangeSceneRequestEvent>(ChangeScene);
            EventBus<ChangeSceneRequestEvent>.Register(_changeSceneRequestEventBinding);
        }

        public override void OnStopServer()
        {
            EventBus<ChangeSceneRequestEvent>.Deregister(_changeSceneRequestEventBinding);
        }

        [Server]
        private void ChangeScene(ChangeSceneRequestEvent changeSceneRequestEvent)
        {
            var sceneLoadData = new SceneLoadData(changeSceneRequestEvent.SceneToLoadName)
            {
                ReplaceScenes = ReplaceOption.All,
            };

            EventBus<TransportingClientsToSceneEvent>.Raise(new TransportingClientsToSceneEvent
            {
                PlayerCount = base.NetworkManager.ClientManager.Clients.Count
            });

            base.SceneManager.LoadGlobalScenes(sceneLoadData);
        }
    }
}