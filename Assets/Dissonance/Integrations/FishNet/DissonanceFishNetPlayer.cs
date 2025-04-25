using System;
using System.Collections;
using Dissonance.Integrations.FishNet.Utils;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace Dissonance.Integrations.FishNet
{
    // A Player object wrapper for Dissonance Voice
    public sealed class DissonanceFishNetPlayer : NetworkBehaviour, IDissonancePlayer
    {
        [Tooltip("This transform will be used in positional voice processing. If unset, then GameObject's transform will be used.")]
        [SerializeField] private Transform trackingTransform;
        
        // SyncVar ensures that all observers know player ID, even late joiners
        private readonly SyncVar<string> _syncedPlayerName = new (settings: new SyncTypeSettings(WritePermission.ServerOnly, ReadPermission.Observers));

        // Captured DissonanceComms instance
        public DissonanceComms Comms { get; private set; }
        
        Coroutine getPlayerTrackingBuffer; // Need a reference to the coroutine to stop it if needed
        
        public string PlayerId => _syncedPlayerName.Value;
        public Vector3 Position => trackingTransform.position;
        public Quaternion Rotation => trackingTransform.rotation;
        public NetworkPlayerType Type => IsOwner ? NetworkPlayerType.Local : NetworkPlayerType.Remote;

        public bool IsTracking { get; private set; }


        private void Awake()
        {
            if (trackingTransform == null) trackingTransform = transform;
        }

        private void OnEnable()
        {
            Debug.Log($"DissonanceFishNetPlayer: {PlayerId}");
            ManageTrackingState(true);
        }
        
        private void OnDisable()
        { 
            ManageTrackingState(false);
        }

        // Called by FishNet when object is spawned on client with authority
        public override void OnOwnershipClient(NetworkConnection prevOwner)
        {
            base.OnOwnershipClient(prevOwner);

            if (prevOwner == null || !IsOwner) return;
            
            DissonanceFishNetComms fishNetComms = DissonanceFishNetComms.Instance;
            if (fishNetComms == null)
            {
                LoggingHelper.Logger.Error("Could not find any DissonanceFishNetComms instance! This DissonancePlayer instance will not work!");
                return;
            }

            // Configure Player name
            fishNetComms.Comms.LocalPlayerNameChanged += SetPlayerName;
            if (fishNetComms.Comms.LocalPlayerName == null)
            {
                string randomGuid = Guid.NewGuid().ToString();
                fishNetComms.Comms.LocalPlayerName = randomGuid;
            }
            else
            {
                SetPlayerName(fishNetComms.Comms.LocalPlayerName);
            }
        }

        private void SetPlayerName(string playerName)
        {
            // Disable tracking before name change
            if (IsTracking) ManageTrackingState(false);
            
            // Update name & re-enable tracking
            _syncedPlayerName.Value = playerName;
            ManageTrackingState(true);
            
            // And if owner, sync name over network
            if(IsOwner) ServerRpcSetPlayerName(playerName);
        }
        
        [ServerRpc(RequireOwnership = true)]
        private void ServerRpcSetPlayerName(string playerName)
        {
            _syncedPlayerName.Value = playerName;
        }

        private void OnSyncedPlayerNameUpdated(string _, string updatedName, bool __)
        {
            if(!IsOwner) SetPlayerName(updatedName);
        }
        
        private void ManageTrackingState(bool track)
        {
            if (getPlayerTrackingBuffer != null) // If the coroutine is currently running, stop it. It can't work anyway because if it's running then the value the code needs is null, so it has no purpose.
                StopCoroutine(getPlayerTrackingBuffer);
            getPlayerTrackingBuffer = StartCoroutine(ManageTrackingStateCoroutine(track)); // start the coroutine with the latest boolean we want to send it.
        }

        private IEnumerator ManageTrackingStateCoroutine(bool track)
        {
            while (string.IsNullOrEmpty(PlayerId)) // The code will sit here with the latest boolean sent to it and only continue once the value it needs is null.
                yield return null;
            
            // Check if you should change tracking state
            if (IsTracking == track) yield break;
            if (DissonanceFishNetComms.Instance == null) yield break;
            if (track && !DissonanceFishNetComms.Instance.IsInitialized) yield break;

            // And update it
            DissonanceComms comms = DissonanceFishNetComms.Instance.Comms;
            if (track) comms.TrackPlayerPosition(this);
            else comms.StopTracking(this);
            IsTracking = track;

            Debug.Log("<color=green>Successfully assigned player tracking</color>");
            getPlayerTrackingBuffer = null;
        }
    }
}