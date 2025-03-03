using FishNet.Object;
using UnityEngine;

namespace HotPotato.Utils
{
    public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T> 
    {
        public static T Instance { get; private set; }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            
            if (Instance != null)
            {
                Debug.LogWarning($"Multiple instances of {typeof(T)} found. Destroying this instance.");
                Destroy(gameObject);
                return;
            }
            
            Instance = (T) this;
        }
        
        public override void OnStopNetwork()
        {
            base.OnStopNetwork();
            
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}