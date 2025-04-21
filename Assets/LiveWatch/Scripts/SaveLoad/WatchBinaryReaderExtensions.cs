using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    public static class WatchBinaryReaderExtensions
    {
        public static WatchStorage ReadWatchStorage(this BinaryReader br, CancellationToken token = default, ProcessProgress progress = null)
        {
            var watches = new WatchStorage();
            var count = br.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                if (token.IsCancellationRequested)
                    return watches;
                
                var variable = br.ReadWatchVariable(token, progress);
                watches.Items.Add(variable.Name, variable);
                watches.SortedNames.Add(variable.Name);
            }
            
            WatchServices.VariableSortUpdater.SortWatches(watches);
            return watches;
        }

        public static WatchVariable ReadWatchVariable(this BinaryReader br, CancellationToken token = default, ProcessProgress progress = null)
        {
            var watchVariable = new WatchVariable
            {
                Name = br.ReadString(),
                Values = br.ReadWatchValueList(token, progress),
                Childs = br.ReadWatchStorage(token, progress),
                RuntimeMeta = br.ReadVariableRuntimeMeta()
            };

            foreach (var child in watchVariable.Childs.Items.Values)
                child.Parent = watchVariable;
            
            return watchVariable;
        }

        public static WatchValueList ReadWatchValueList(this BinaryReader br, CancellationToken token = default, ProcessProgress progress = null)
        {
            var watchValueList = new WatchValueList()
            {
                Type = (WatchValueType)br.ReadByte()
            };

            switch (watchValueList.Type)
            {
                case WatchValueType.NotSet:
                    break;
                case WatchValueType.Float:
                    watchValueList.FloatList = br.ReadWatchValueList(br.ReadSingle, token, progress);
                    break;
                case WatchValueType.Double:
                    watchValueList.DoubleList = br.ReadWatchValueList(br.ReadDouble, token, progress);
                    break;
                case WatchValueType.Int:
                    watchValueList.IntList = br.ReadWatchValueList(br.ReadInt32, token, progress);
                    break;
                case WatchValueType.Bool:
                    watchValueList.BoolList = br.ReadWatchValueList(br.ReadBoolean, token, progress);
                    break;
                case WatchValueType.String:
                    watchValueList.StringList = br.ReadWatchValueList(br.ReadString, token, progress);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return watchValueList;
        }

        public static VariableRuntimeMeta ReadVariableRuntimeMeta(this BinaryReader br)
        {
            return new VariableRuntimeMeta()
            {
                AlwaysShrinkable = br.ReadBoolean(),
                DecimalPlaces = br.ReadInt32(),
                IsOrderSet = br.ReadBoolean(),
                SortOrder = br.ReadSingle(),
                CreationStackTrace = br.ReadString(),
                MinMax = br.ReadMinMaxMeta(),
                Formatting = br.ReadVariableFormatMeta(),
                ExtraTexts = br.ReadValueList(br.ReadString),
                StackTraces = br.ReadValueList(br.ReadString),
            };
        }

        public static VariableFormatMeta ReadVariableFormatMeta(this BinaryReader br)
        {
            return new VariableFormatMeta()
            {
                TitleFormat = br.ReadWatchTitleFormat(),
                DefaultValueFormat = br.ReadWatchValueFormat(),
                ValueFormats = br.ReadValueList(br.ReadWatchValueFormat)
            };
        }

        public static WatchTitleFormat ReadWatchTitleFormat(this BinaryReader br)
        {
            return new WatchTitleFormat()
            {
               BackColor = br.ReadOverridenColor()
            };
        }
        
        public static WatchValueFormat ReadWatchValueFormat(this BinaryReader br)
        {
            return new WatchValueFormat()
            {
                FillColor = br.ReadOverridenColor(),
                GraphLineColor = br.ReadOverridenColor()
            };
        }
        
        public static RepetitiveValueList<WatchValue<T>> ReadWatchValueList<T>(
            this BinaryReader br, 
            Func<T> readValue,
            CancellationToken token = default,
            ProcessProgress progress = null) where T : IEquatable<T>
        {
            var list = new RepetitiveValueList<WatchValue<T>>()
            {
                Count = br.ReadInt32()
            };

            if (list.Count == 0)
                return list;
            
            var originalsCount = br.ReadInt32();

            for (var i = 0; i < originalsCount; i++)
            {
                if (token.IsCancellationRequested)
                    return list;
                
                list.OriginalKeys.Add(br.ReadInt32());
                
                var isEmpty = br.ReadBoolean();

                list.OriginalValues.Add(isEmpty 
                    ? WatchValue<T>.Empty() 
                    : new WatchValue<T>(readValue()));
                
                if (progress != null)
                    progress.CurrentCount++;
            }

            return list;
        }
        
        public static RepetitiveValueList<T> ReadValueList<T>(this BinaryReader br, Func<T> readValue) where T : IEquatable<T>
        {
            var list = new RepetitiveValueList<T>()
            {
                Count = br.ReadInt32()
            };

            if (list.Count == 0)
                return list;
            
            var originalsCount = br.ReadInt32();

            for (var i = 0; i < originalsCount; i++)
            {
                list.OriginalKeys.Add(br.ReadInt32());
                list.OriginalValues.Add(readValue());
            }

            return list;
        }

        public static OverridenValue<Color> ReadOverridenColor(this BinaryReader br)
        {
            var overridenColor = new OverridenValue<Color>();
            
            var isSet = br.ReadBoolean();

            if (!isSet)
                return overridenColor;

            overridenColor.SetValue(br.ReadColor());
            return overridenColor;
        }

        public static Color ReadColor(this BinaryReader br)
        {
            return new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
        }

        public static VariableMinMaxMeta ReadMinMaxMeta(this BinaryReader br)
        {
            var mode = br.ReadMinMaxMode();

            return mode switch
            {
                WatchMinMaxMode.Local => new VariableMinMaxMeta(),
                WatchMinMaxMode.Global => VariableMinMaxMeta.Global(),
                WatchMinMaxMode.Custom => VariableMinMaxMeta.Custom(br.ReadDouble(), br.ReadDouble()),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public static WatchMinMaxMode ReadMinMaxMode(this BinaryReader br)
        {
            return (WatchMinMaxMode)br.ReadByte();
        }
    }
}