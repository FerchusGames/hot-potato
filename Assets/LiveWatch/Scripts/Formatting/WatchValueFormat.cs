using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public struct WatchValueFormat : IEquatable<WatchValueFormat>
    {
        public static WatchValueFormat Empty { get; } = new();
        public static WatchValueFormat Red  { get; } = new(WatchColors.Red);
        public static WatchValueFormat Blue  { get; } = new(WatchColors.Blue);
        public static WatchValueFormat Green  { get; } = new(WatchColors.Green);
        public static WatchValueFormat Cyan  { get; } = new(WatchColors.Cyan);
        public static WatchValueFormat Magenta  { get; } = new(WatchColors.Magenta);
        public static WatchValueFormat Yellow  { get; } = new(WatchColors.Yellow);
        
        public OverridenValue<Color> FillColor;
        public OverridenValue<Color> GraphLineColor;

        public bool FillAndGraphColorOverriden => FillColor.IsSet || GraphLineColor.IsSet;

        public WatchValueFormat(Color fillColor)
        {
            FillColor = new OverridenValue<Color>(fillColor);
            GraphLineColor = new OverridenValue<Color>();
        }
        
        public WatchValueFormat(Color fillColor, Color graphLineColor)
        {
            FillColor = new OverridenValue<Color>(fillColor);
            GraphLineColor = new OverridenValue<Color>(graphLineColor);
        }
        
        public bool Equals(WatchValueFormat other)
        {
            return FillColor.Equals(other.FillColor) && GraphLineColor.Equals(other.GraphLineColor);
        }

        public override bool Equals(object obj)
        {
            return obj is WatchValueFormat other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FillColor, GraphLineColor);
        }
    }
}