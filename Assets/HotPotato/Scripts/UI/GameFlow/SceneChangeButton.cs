using FishNet.Managing.Scened;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI.GameFlow
{
    public class SceneChangeButton : NetworkBehaviour
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
            var sceneLoadData = new SceneLoadData(_sceneToLoadName)
            {
                ReplaceScenes = ReplaceOption.All,
            };
            
            base.SceneManager.LoadGlobalScenes(sceneLoadData);
        }
    }
}