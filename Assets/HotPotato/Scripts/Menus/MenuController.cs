using System.Collections.Generic;
using Heathen.SteamworksIntegration;
using HotPotato.UI.Lobby;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using API = Heathen.SteamworksIntegration.API;

namespace HotPotato.Menus
{
    public class MenuController : MonoBehaviour
    {
        //TODO: Separate into LobbyController and MenuController
        
        [Required]
        [SerializeField] private GameObject _mainMenu;
        
        [BoxGroup("Lobby"), Required]
        [SerializeField] private GameObject _lobbyMenu;

        [BoxGroup("Lobby"), Required] 
        [SerializeField] private TextMeshProUGUI _lobbyTitle;
        
        [BoxGroup("Lobby"), Required]
        [SerializeField] private LobbyManager _lobbyManager;
        
        [BoxGroup("Lobby"), Required, AssetsOnly]
        [SerializeField] private LobbyUserPanel _lobbyUserPanelPrefab;
        
        [BoxGroup("Lobby"), Required]
        [SerializeField] private Transform _lobbyUserPanelHolder;

        [BoxGroup("Lobby"), Required]
        [SerializeField] private GameObject _startGameButton;
        
        [BoxGroup("Lobby"), Required]
        [SerializeField] private TextMeshProUGUI _playerCountText;
        
        private Dictionary<UserData, LobbyUserPanel> _lobbyUserPanels = new();
        
        private void Awake()
        {
            OpenMainMenu();
            API.Overlay.Client.EventGameLobbyJoinRequested.AddListener(OverlayJoinButton);
        }

        public void OnLobbyCreated(LobbyData lobbyData)
        {
            ClearUserPanels();
            
            UpdatePlayerCount();
            UpdateStartGameButtonVisibility();
            lobbyData.Name = UserData.Me.Name + "'s Lobby";
            _lobbyTitle.text = lobbyData.Name;
            OpenLobbyMenu();
            
            SetupUserPanel(UserData.Me);
        }
        
        public void OnLobbyJoined(LobbyData lobbyData)
        {
            ClearUserPanels();
            
            UpdatePlayerCount();
            _lobbyTitle.text = lobbyData.Name;
            OpenLobbyMenu();

            foreach (var member in lobbyData.Members)
            {
                SetupUserPanel(member.user);
            }
        }
        
        public void OnUserJoined(UserData userData)
        {
            UpdatePlayerCount();
            UpdateStartGameButtonVisibility();
            SetupUserPanel(userData);
        }

        public void OnUserLeft(UserLobbyLeaveData userLobbyLeaveData)
        {
            UpdatePlayerCount();
            UpdateStartGameButtonVisibility();
            DestroyUserPanel(userLobbyLeaveData);
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

        private void SetupUserPanel(UserData userData)
        {
            var userPanel = Instantiate(_lobbyUserPanelPrefab, _lobbyUserPanelHolder);
            userPanel.Initialize(userData); 
            _lobbyUserPanels.TryAdd(userData, userPanel);
        }
        
        private void DestroyUserPanel(UserLobbyLeaveData userLobbyLeaveData)
        {
            if (!_lobbyUserPanels.TryGetValue(userLobbyLeaveData.user, out var userPanel))
            {
                Debug.Log("User panel not found for user: " + userLobbyLeaveData.user.Name);
                return;
            }
            
            Destroy(userPanel.gameObject);
            _lobbyUserPanels.Remove(userLobbyLeaveData.user);
        }
        
        private void ClearUserPanels()
        {
            foreach (Transform child in _lobbyUserPanelHolder)
            {
               Destroy(child.gameObject);
            }
            
            _lobbyUserPanels.Clear();
        }
        
        private void UpdateStartGameButtonVisibility()
        {
            bool isHost = _lobbyManager.HasLobby && _lobbyManager.IsPlayerOwner;
            bool hasEnoughPlayers = _lobbyManager.MemberCount >= 2;
    
            _startGameButton.SetActive(isHost && hasEnoughPlayers);
        }

        private void UpdatePlayerCount()
        {
            _playerCountText.text = _lobbyManager.MemberCount + "/" + _lobbyManager.MaxMembers;
        }
    }
}