using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI.GameFlow
{
    public class ChangeSceneButton : MonoBehaviour
    {
        [Required]
        [SerializeField] private string _sceneToLoadName;
        
        [Required]
        [SerializeField] private Button _button;

        private void Awake()
        {
            _button.onClick.AddListener(ChangeScene);
        }
        
        [Server]
        private void ChangeScene()
        {
            EventBus<ChangeSceneRequestEvent>.Raise(new ChangeSceneRequestEvent
            {
                SceneToLoadName = _sceneToLoadName
            });
        }
    }
}