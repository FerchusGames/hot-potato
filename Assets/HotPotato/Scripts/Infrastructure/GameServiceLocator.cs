using HotPotato.ApplicationLifecycle;
using UnityEngine;

namespace HotPotato.Infrastructure
{
    public static class GameServiceLocator
    {
        private static ApplicationManager _applicationManager;

        public static void RegisterApplicationManager(ApplicationManager manager)
        {
            _applicationManager = manager;
        }

        public static ApplicationManager GetApplicationManager()
        {
            if (_applicationManager == null)
            {
                Debug.LogError("ApplicationManager has not been registered in GameServiceLocator!");
            }
            return _applicationManager;
        }
    }
}