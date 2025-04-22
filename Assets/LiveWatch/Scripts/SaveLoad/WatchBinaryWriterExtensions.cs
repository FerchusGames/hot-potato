using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    public static class WatchBinaryWriterExtensions
    {
        public static void Write(this BinaryWriter bw, WatchStorage watchStorage, int maxValuesCount, CancellationToken token = default, ProcessProgress progress = null)
        {
            var count = watchStorage.Count;
            var counter = 0;
            
            bw.Write(count);
            
            foreach (var watchNameVariablePair in watchStorage.Items)
            {
                if (token.IsCancellationRequested)
                    return;
                
                if (counter++ > count)
                    break;
                
                bw.Write(watchNameVariablePair.Value, maxValuesCount, token, progress);
            }
        }
        
        public static void Write(this BinaryWriter bw, WatchVariable variable, int maxValuesCount, CancellationToken token = default, ProcessProgress progress = null)
        {
            bw.Write(variable.Name);
            bw.Write(variable.Values, maxValuesCount, token, progress);
            bw.Write(variable.Childs, maxValuesCount, token, progress);
            bw.Write(variable.RuntimeMeta, maxValuesCount, token);
        }

        public static void Write(this BinaryWriter bw, WatchValueList valueList, int maxValuesCount, CancellationToken token = default, ProcessProgress progress = null)
        {
            bw.Write((byte)valueList.Type);

            switch (valueList.Type)
            {
                case WatchValueType.NotSet:
                    break;
                case WatchValueType.Float:
                    bw.Write(valueList.FloatList, bw.Write, maxValuesCount, token, progress);
                    break;
                case WatchValueType.Double:
                    bw.Write(valueList.DoubleList, bw.Write, maxValuesCount, token, progress);
                    break;
                case WatchValueType.Int:
                    bw.Write(valueList.IntList, bw.Write, maxValuesCount, token, progress);
                    break;
                case WatchValueType.Bool:
                    bw.Write(valueList.BoolList, bw.Write, maxValuesCount, token, progress);
                    break;
                case WatchValueType.String:
                    bw.Write(valueList.StringList, bw.Write, maxValuesCount, token, progress);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void Write(this BinaryWriter bw, VariableRuntimeMeta runtimeMeta, int maxValuesCount, CancellationToken token = default)
        {
            bw.Write(runtimeMeta.AlwaysShrinkable);
            bw.Write(runtimeMeta.DecimalPlaces);
            bw.Write(runtimeMeta.IsOrderSet);
            bw.Write(runtimeMeta.SortOrder);
            bw.Write(runtimeMeta.CreationStackTrace);
            bw.Write(runtimeMeta.MinMax);
            bw.Write(runtimeMeta.Formatting, maxValuesCount, token);
            bw.Write(runtimeMeta.ExtraTexts, bw.Write, maxValuesCount, token);
            bw.Write(runtimeMeta.StackTraces, bw.Write, maxValuesCount, token);
        }

        public static void Write(this BinaryWriter bw, VariableFormatMeta formatMeta, int maxValuesCount, CancellationToken token = default)
        {
            bw.Write(formatMeta.TitleFormat);
            bw.Write(formatMeta.DefaultValueFormat);
            bw.Write(formatMeta.ValueFormats, bw.Write, maxValuesCount, token);
        }
        
        public static void Write(this BinaryWriter bw, WatchTitleFormat titleFormat)
        {
            bw.Write(titleFormat.BackColor);
        }

        public static void Write(this BinaryWriter bw, WatchValueFormat valueFormat)
        {
            bw.Write(valueFormat.FillColor);
            bw.Write(valueFormat.GraphLineColor);
        }

        public static void Write<T>(this BinaryWriter bw, 
            RepetitiveValueList<WatchValue<T>> list, 
            Action<T> writeValue, 
            int maxValuesCount,
            CancellationToken token = default,
            ProcessProgress progress = null) where T : IEquatable<T>
        {
            var maxCount = list?.Count ?? 0;
            maxCount = Mathf.Min(maxCount, maxValuesCount);
            
            bw.Write(maxCount);
            
            if (maxCount == 0)
                return;

            var keysCount = Mathf.Min(list.OriginalKeys.Count, maxValuesCount);
            bw.Write(keysCount);

            for (var i = 0; i < keysCount; i++)
            {
                if (token.IsCancellationRequested)
                    return;

                bw.Write(list.OriginalKeys[i]);
                
                var value = list.OriginalValues[i];
                bw.Write(value.IsEmpty);

                if (!value.IsEmpty)
                    writeValue(value.Value);

                if (progress != null)
                    progress.CurrentCount++;
            }
        }
        
        public static void Write<T>(this BinaryWriter bw, RepetitiveValueList<T> list, Action<T> writeValue, int maxValuesCount, CancellationToken token = default) where T : IEquatable<T>
        {
            var maxCount = list?.Count ?? 0;
            maxCount = Mathf.Min(maxCount, maxValuesCount);
            
            bw.Write(maxCount);
            
            if (maxCount == 0)
                return;

            var keysCount = Mathf.Min(list.OriginalKeys.Count, maxValuesCount);
            bw.Write(keysCount);

            for (var i = 0; i < keysCount; i++)
            {
                if (token.IsCancellationRequested)
                    return;
                
                bw.Write(list.OriginalKeys[i]);
                writeValue(list.OriginalValues[i]);
            }
        }

        public static void Write(this BinaryWriter bw, OverridenValue<Color> color)
        {
            bw.Write(color.IsSet);
            
            if (color.IsSet)
                bw.Write(color.Value);
        }
        
        public static void Write(this BinaryWriter bw, Color color)
        {
            bw.Write(color.r);
            bw.Write(color.g);
            bw.Write(color.b);
            bw.Write(color.a);
        }
        
        public static void Write(this BinaryWriter bw, VariableMinMaxMeta minMax)
        {
            bw.Write(minMax.Mode);

            if (minMax.Mode == WatchMinMaxMode.Custom)
            {
                bw.Write(minMax.CustomMinValue);
                bw.Write(minMax.CustomMaxValue);
            }
        }
        
        public static void Write(this BinaryWriter bw, WatchMinMaxMode mode)
        {
            bw.Write((byte)mode);
        }
    }
}