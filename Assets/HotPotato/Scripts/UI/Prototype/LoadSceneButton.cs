using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HotPotato.UI.Prototype
{
    public class LoadSceneButton : MonoBehaviour
    {
        [SerializeField, Required] private Button _button;
        [SerializeField] private string _sceneName = "SteamLobby";
        
        private void Awake()
        {
            _button.onClick.AddListener(LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}