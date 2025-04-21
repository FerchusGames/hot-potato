using UnityEngine;

namespace Ingvar.LiveWatch
{
    public static class WatchColors
    {
        public static readonly Color Green;
        public static readonly Color Yellow;
        public static readonly Color Red;
        public static readonly Color Magenta;
        public static readonly Color Blue;
        public static readonly Color Cyan;
        
        static WatchColors()
        {
            Green = new Color32(64, 132, 5, 255);
            Yellow = new Color32(173, 127, 2, 255);
            Red = new Color32(192, 86, 72, 255);
            Magenta = new Color32(174, 81, 188, 255);
            Blue = new Color32(67, 86, 204, 255);
            Cyan = new Color32(44, 147, 154, 255);
        }
    }
}