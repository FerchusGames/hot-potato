using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Managing.Server;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HotPotato.Network
{
    public class ServerDisconnectHandler : NetworkBehaviour
    {
        [SerializeField] private string _disconnectSceneName = "DisconnectScene";

        private void Start()
        {
            var clientManager = InstanceFinder.ClientManager;
            var serverManager = InstanceFinder.ServerManager;

            if (clientManager != null)
                clientManager.OnClientConnectionState += OnClientConnectionState;

            if (serverManager != null)
                serverManager.OnRemoteConnectionState += OnRemoteClientConnectionState;
        }

        private void OnDestroy()
        {
            var clientManager = InstanceFinder.ClientManager;
            var serverManager = InstanceFinder.ServerManager;

            if (clientManager != null)
                clientManager.OnClientConnectionState -= OnClientConnectionState;

            if (serverManager != null)
                serverManager.OnRemoteConnectionState -= OnRemoteClientConnectionState;
        }

        // Called locally when *this* client disconnects from server
        private void OnClientConnectionState(ClientConnectionStateArgs args)
        {
            if (args.ConnectionState == LocalConnectionState.Stopped)
            {
                Debug.Log("Local client disconnected. Loading disconnect scene.");
                UnityEngine.SceneManagement.SceneManager.LoadScene(_disconnectSceneName);
            }
        }

        // Called on the server when a remote client disconnects
        private void OnRemoteClientConnectionState(NetworkConnection conn, RemoteConnectionStateArgs args)
        {
            if (args.ConnectionState == RemoteConnectionState.Stopped)
            {
                Debug.Log($"Client {conn.ClientId} disconnected. Forcing all clients to disconnect scene.");
                SendClientsToDisconnectSceneServerRpc();
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void SendClientsToDisconnectSceneServerRpc()
        {
            Debug.Log("Sending disconnect scene RPC to all clients.");
            SendClientsToDisconnectScene();
        }

        [ObserversRpc(BufferLast = false)]
        private void SendClientsToDisconnectScene()
        {
            Debug.Log("Received disconnect scene RPC. Loading...");
            UnityEngine.SceneManagement.SceneManager.LoadScene(_disconnectSceneName);
        }
    }
}
