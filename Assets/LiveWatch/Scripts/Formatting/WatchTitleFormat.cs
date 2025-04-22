using System;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public struct WatchTitleFormat
    {
        public static WatchTitleFormat Empty { get; } = new();
        public static WatchTitleFormat Red  { get; } = new(WatchColors.Red);
        public static WatchTitleFormat Blue  { get; } = new(WatchColors.Blue);
        public static WatchTitleFormat Green  { get; } = new(WatchColors.Green);
        public static WatchTitleFormat Cyan  { get; } = new(WatchColors.Cyan);
        public static WatchTitleFormat Magenta  { get; } = new(WatchColors.Magenta);
        public static WatchTitleFormat Yellow  { get; } = new(WatchColors.Yellow);
        
        public OverridenValue<Color> BackColor;
        
        public WatchTitleFormat(Color color)
        {
            BackColor = new OverridenValue<Color>(color);
        }
    }
}