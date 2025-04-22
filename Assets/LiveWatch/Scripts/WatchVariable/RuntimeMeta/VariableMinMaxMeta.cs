using System;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public struct VariableMinMaxMeta
    {
        public WatchMinMaxMode Mode;
        public double CustomMinValue;
        public double CustomMaxValue;

        public static VariableMinMaxMeta Global()
        {
            return new VariableMinMaxMeta()
            {
                Mode = WatchMinMaxMode.Global
            };
        }
        
        public static VariableMinMaxMeta Custom(double minValue, double maxValue)
        {
            return new VariableMinMaxMeta()
            {
                Mode = WatchMinMaxMode.Custom,
                CustomMinValue = minValue,
                CustomMaxValue = maxValue
            };
        }
    }
}