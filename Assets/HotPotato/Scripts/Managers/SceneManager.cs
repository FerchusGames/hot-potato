using FishNet.Managing.Scened;
using FishNet.Object;

namespace HotPotato.Managers
{
    public class SceneManager : NetworkBehaviour
    {
        private EventBinding<ChangeSceneEvent> _loadSceneEventBinding;
        
        public override void OnStartServer()
        {
            _loadSceneEventBinding = new EventBinding<ChangeSceneEvent>(ChangeScene);
            EventBus<ChangeSceneEvent>.Register(_loadSceneEventBinding);
        }

        public override void OnStopServer()
        {
            EventBus<ChangeSceneEvent>.Deregister(_loadSceneEventBinding);
        }

        [Server]
        private void ChangeScene(ChangeSceneEvent changeSceneEvent)
        {
            var sceneLoadData = new SceneLoadData(changeSceneEvent.SceneToLoadName)
            {
                ReplaceScenes = ReplaceOption.All,
            };
            
            base.SceneManager.LoadGlobalScenes(sceneLoadData);
        }
    }
}