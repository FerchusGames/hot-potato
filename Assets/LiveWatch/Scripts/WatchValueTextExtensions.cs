using System;
using System.Globalization;

namespace Ingvar.LiveWatch
{
    public static class WatchValueTextExtensions
    {
        public static string TrueString = "True";
        public static string FalseString = "False";

        public static string GetValueText(this WatchVariable variable, int index)
        {
            if (variable.Values.IsEmptyAt(index))
                return string.Empty;

            return variable.Values.Type switch
            {
                WatchValueType.String or WatchValueType.NotSet => GetStringText(variable.Values.StringList, index),
                WatchValueType.Float => GetFloatText(variable.Values.FloatList, index, variable.RuntimeMeta.DecimalPlaces),
                WatchValueType.Double => GetDoubleText(variable.Values.DoubleList, index, variable.RuntimeMeta.DecimalPlaces),
                WatchValueType.Int => GetIntText(variable.Values.IntList, index),
                WatchValueType.Bool => GetBoolText(variable.Values.BoolList, index),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private static string GetFloatText(RepetitiveValueList<WatchValue<float>> valueList, int index, int decimalPlaces)
        {
            var watchValue = valueList[index];
            
            if (!string.IsNullOrEmpty(watchValue.ValueText))
                return watchValue.ValueText;

            watchValue.ValueText = Math.Round(watchValue.Value, decimalPlaces).ToString(NumberFormatInfo.CurrentInfo);
            valueList[index] = watchValue;
            return watchValue.ValueText;
        }
        
        private static string GetDoubleText(RepetitiveValueList<WatchValue<double>> valueList, int index, int decimalPlaces)
        {
            var watchValue = valueList[index];
            
            if (!string.IsNullOrEmpty(watchValue.ValueText))
                return watchValue.ValueText;

            watchValue.ValueText = Math.Round(watchValue.Value, decimalPlaces).ToString(NumberFormatInfo.CurrentInfo);
            valueList[index] = watchValue;
            return watchValue.ValueText;
        }
        
        private static string GetIntText(RepetitiveValueList<WatchValue<int>> valueList, int index)
        {
            var watchValue = valueList[index];
            
            if (!string.IsNullOrEmpty(watchValue.ValueText))
                return watchValue.ValueText;

            watchValue.ValueText = watchValue.Value.ToString();
            valueList[index] = watchValue;
            return watchValue.ValueText;
        }
        
        private static string GetStringText(RepetitiveValueList<WatchValue<string>> valueList, int index)
        {
            var watchValue = valueList[index];
            return watchValue.Value;
        }
        
        private static string GetBoolText(RepetitiveValueList<WatchValue<bool>> valueList, int index)
        {
            var watchValue = valueList[index];
            return watchValue.Value ? TrueString : FalseString;
        }
    }
}