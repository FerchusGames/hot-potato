using System.IO;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    public abstract class Singleton<T> where T : new()
    {
        public static T instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = new T();
                return _instance;
            }
        }

        private static T _instance;
    }
}