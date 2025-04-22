using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch
{
    public class WatchStorageSO : 
#if UNITY_EDITOR
        ScriptableSingleton<WatchStorageSO>
#else
        Singleton<WatchStorageSO>
#endif
    
    {
        [SerializeField] private bool _isLive = true;
        [SerializeField] private bool _collapse = true;
        [SerializeField] private bool _isLeftSelection = false;
        [SerializeField] private float _columnWidth = 30;
        [SerializeField] private float _rowHeight = 30;
        
        [SerializeField] private WatchStorage _watches = new WatchStorage();

        public bool IsLive
        {
            get => _isLive;
            set => _isLive = value;
        }

        public bool Collapse
        {
            get => _collapse;
            set => _collapse = value;
        }
        
        public bool IsLeftSelection
        {
            get => _isLeftSelection;
            set => _isLeftSelection = value;
        }
        
        public float ColumnWidth
        {
            get => _columnWidth;
            set => _columnWidth = value;
        }
        
        public float RowHeight
        {
            get => _rowHeight;
            set => _rowHeight = value;
        }

        public WatchStorage Watches
        {
            get => _watches;
            set => _watches = value;
        }
    }
}