using UnityEngine;

namespace HotPotato.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour 
    {
        [SerializeField] private bool _isPersistent = true;
        
        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            if (_isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
            
            Instance = this as T;
        }
    }
}