using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    public class MainThreadDispatcher : MonoBehaviour
    {
        private static MainThreadDispatcher _instance;
        private static Queue<Action> _dispatchedActions = new();

        public static void Dispatch(Action action)
        {
            _dispatchedActions.Enqueue(action);
        }

        public static void ClearAll()
        {
            _dispatchedActions.Clear();
        }
        
        public static void RefreshInstance()
        {
            if (Application.isEditor || _instance != null)
                return;
            
            var go = new GameObject(nameof(MainThreadDispatcher));
            _instance = go.AddComponent<MainThreadDispatcher>();
            
            DontDestroyOnLoad(go);
        }

        private void Update()
        {
            if (Application.isEditor)
                return;
            
            InvokeAll();
        }

        private static void InvokeAll()
        {
            while (_dispatchedActions.Count > 0)
            {
                var action = _dispatchedActions.Dequeue();

                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error in dispatched action! Message: {e.Message}");
                }
            }
        }
        
        #if UNITY_EDITOR
        
        [UnityEditor.InitializeOnLoadMethod]
        static void EditorInitialize()
        {
            UnityEditor.EditorApplication.update += EditorUpdate;
        }
        
        private static void EditorUpdate()
        {
            InvokeAll();
        }
        
        #endif
    }
}