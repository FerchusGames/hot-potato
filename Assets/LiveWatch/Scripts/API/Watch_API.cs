using System;
using System.Diagnostics;

namespace Ingvar.LiveWatch
{
    public static partial class Watch
    {
        public static event Action OnClearedAll;
        public static event Action OnDestroyedAll;
        
        public static WatchReference<T> GetOrAdd<T>(string path)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            return WatchServices.ReferenceCreator.GetOrAdd<T>(path);
#else    
            return WatchServices.ReferenceCreator.Empty<T>();
#endif
        }

        public static WatchReference<Any> PushEmpty(string path)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            return WatchServices.ReferenceCreator.GetOrAdd<Any>(path).PushEmptyValue();
#else
            return WatchServices.ReferenceCreator.Empty<Any>();
#endif
        }
        
        public static WatchReference<Any> PushFormat(string path, WatchValueFormat format)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            return WatchServices.ReferenceCreator.GetOrAdd<Any>(path).PushValueFormat(format);
#else
            return WatchServices.ReferenceCreator.Empty<Any>();
#endif
        }
        
        public static WatchReference<Any> PushExtraText(string path, string extraText)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            return WatchServices.ReferenceCreator.GetOrAdd<Any>(path).PushExtraText(extraText);
#else
            return WatchServices.ReferenceCreator.Empty<Any>();
#endif
        }
        
        public static WatchReference<Any> PushStackTrace(string path)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            return WatchServices.ReferenceCreator.GetOrAdd<Any>(path).PushStackTrace();
#else
            return WatchServices.ReferenceCreator.Empty<Any>();
#endif
        }
        
        public static void UpdateAll()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchServices.VariableUpdater.UpdateAll();
#endif
        }

        public static void ClearAll()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchServices.VariableUpdater.ClearAll();
            
            OnClearedAll?.Invoke();
#endif     
        }

        public static void DestroyAll()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchServices.VariableUpdater.ClearAll();
            Watches.Clear();
            
            OnDestroyedAll?.Invoke();
#endif
        }
    }
}