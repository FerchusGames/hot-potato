using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    public static partial class Watch
    {
        public static char PathSeparator { get; set; } = '/';

        internal static int MaxRecursionDepth = 100;
        
        public static bool IsLive
        {
            get => WatchStorageSO.instance.IsLive;
            set => WatchStorageSO.instance.IsLive = value;
        }

        internal static WatchStorage Watches
        {
            get => WatchStorageSO.instance.Watches;
            set
            {
                DestroyAll();
                WatchStorageSO.instance.Watches = value;
            }
        }
    }
}