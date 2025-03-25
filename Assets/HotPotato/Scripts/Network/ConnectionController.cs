using Heathen.SteamworksIntegration;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Network
{
    public class ConnectionController : MonoBehaviour
    {
        [Required]
        [SerializeField] private FishySteamworks.FishySteamworks _fishySteamworks;

        public void StartHost()
        {
            _fishySteamworks.StartConnection(true);
            _fishySteamworks.StartConnection(false);
        }
        
        public void StartClient(LobbyData lobbyData)
        {
            var hostUserData = lobbyData.Owner.user;
            
            _fishySteamworks.SetClientAddress(hostUserData.id.ToString());
            _fishySteamworks.StartConnection(false);
        }
        
        public void Disconnect()
        {
            _fishySteamworks.StopConnection(true);
            _fishySteamworks.StopConnection(false);
        }
    }
}