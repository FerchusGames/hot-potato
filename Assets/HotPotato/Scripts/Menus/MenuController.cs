using Heathen.SteamworksIntegration;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using API = Heathen.SteamworksIntegration.API;

namespace HotPotato.Menus
{
    public class MenuController : MonoBehaviour
    {
        [Required]
        [SerializeField] private GameObject _mainMenu;
        
        [Required]
        [SerializeField] private GameObject _lobbyMenu;

        [Required] 
        [SerializeField] private TextMeshProUGUI _lobbyTitle;
        
        [Required]
        [SerializeField] private LobbyManager _lobbyManager;
        
        private void Awake()
        {
            OpenMainMenu();
            API.Overlay.Client.EventGameLobbyJoinRequested.AddListener(OverlayJoinButton);
        }

        public void OnLobbyCreated(LobbyData lobbyData)
        {
            lobbyData.Name = UserData.Me.Name + "'s Lobby";
            _lobbyTitle.text = lobbyData.Name;
            OpenLobbyMenu();
        }
        
        public void OnLobbyJoined(LobbyData lobbyData)
        {
            _lobbyTitle.text = lobbyData.Name;
            OpenLobbyMenu();
        }
        
        private void OverlayJoinButton(LobbyData lobbyData, UserData userData)
        {
            _lobbyManager.Join(lobbyData);
        }
        
        public void OpenMainMenu()
        {
            CloseScreens();
            _mainMenu.SetActive(true);
        }
        
        private void OpenLobbyMenu()
        {
            CloseScreens();
            _lobbyMenu.SetActive(true);
        }

        private void CloseScreens()
        {
            _lobbyMenu.SetActive(false);
            _mainMenu.SetActive(false);
        }
    }
}