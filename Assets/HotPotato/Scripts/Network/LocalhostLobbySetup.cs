using System;
using FishNet.Object;
using FishNet.Transporting.Tugboat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Network
{
    public class LocalhostLobbySetup : MonoBehaviour
    {
        [Required]
        [SerializeField] private Tugboat _tugboat;
        
        private void Awake()
        {
            if (ParrelSync.ClonesManager.IsClone())
            {
                ConnectAsClient();
                return;
            }
            ConnectAsHost();
        }
        
        private void ConnectAsClient()
        {
            _tugboat.StartConnection(false);
        }

        private void ConnectAsHost()
        {
            _tugboat.StartConnection(true);
            _tugboat.StartConnection(false);
        }
    }
}