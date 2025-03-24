using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;
using FishNet.Managing.Scened;
using Sirenix.OdinInspector;

namespace HotPotato.Player
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [Required]
        [SerializeField] private NetworkObject _playerPrefab;
        
        [SerializeField] private Transform[] _spawnPoints;
        
        private NetworkManager _networkManager;
        private int _nextSpawnIndex;

        public override void OnStartServer()
        {
            _networkManager = base.NetworkManager;
            _networkManager.SceneManager.OnClientPresenceChangeEnd += OnClientPresenceChangeEnd;
        }

        public override void OnStopServer()
        {
            _networkManager.SceneManager.OnClientPresenceChangeEnd -= OnClientPresenceChangeEnd;
        }

        private void OnClientPresenceChangeEnd(ClientPresenceChangeEventArgs args)
        {
            if (!args.Added) return;
            
            Vector3 position;
            Quaternion rotation;
            if (_spawnPoints != null && _spawnPoints.Length > 0)
            {
                Transform spawnPoint = _spawnPoints[_nextSpawnIndex];
                position = spawnPoint.position;
                rotation = spawnPoint.rotation;
                _nextSpawnIndex = (_nextSpawnIndex + 1) % _spawnPoints.Length;
            }
            else
            {
                position = Vector3.zero;
                rotation = Quaternion.identity;
            }
            
            NetworkObject playerInstance = _networkManager.GetPooledInstantiated(_playerPrefab, position, rotation, true);
            _networkManager.ServerManager.Spawn(playerInstance, args.Connection);
        }
    }
}