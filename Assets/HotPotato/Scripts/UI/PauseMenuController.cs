using FishNet;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HotPotato.UI
{
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }

        public void TogglePauseMenu()
        {
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        }

        public void QuitMatch()
        {
            InstanceFinder.ServerManager.StopConnection(true);
            InstanceFinder.ClientManager.StopConnection();
            SceneManager.LoadScene(0);
        }
    }
}