using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    public static class WatchValueNumberExtensions
    {
        public static bool IsValidNumberValue(this WatchVariable variable, int index, out double value)
        {
            if (variable.Values.IsEmptyAt(index))
            {
                value = 0;
                return false;
            }

            switch (variable.Values.Type)
            {
                case WatchValueType.NotSet:
                    value = 0;
                    return false;

                case WatchValueType.Float:
                    var rawValueFloat = variable.Values.FloatList[index].Value;
                    value = rawValueFloat;
                    return float.IsFinite(rawValueFloat);

                case WatchValueType.Double:
                    var rawValueDouble = variable.Values.DoubleList[index].Value;
                    value = rawValueDouble;
                    return double.IsFinite(rawValueDouble);

                case WatchValueType.Int:
                    value = variable.Values.IntList[index].Value;
                    return true;

                case WatchValueType.Bool:
                    value = variable.Values.BoolList[index].Value ? 1 : 0;
                    return true;

                case WatchValueType.String when variable.RuntimeMeta.ValueType is { IsEnum: true }:
                    value = GetEnumNumber(
                        ref variable.EditorMeta.StringToNumberValues,
                        variable.Values.StringList[index].Value,
                        variable.RuntimeMeta.ValueType);
                    return true;

                case WatchValueType.String:
                    value = GetStringNumber(
                        ref variable.EditorMeta.StringToNumberValues,
                        variable.Values.StringList[index].Value,
                        ref variable.EditorMeta.LastStringToNumberValue);
                    return true;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public static bool IsValidNumberValueByIndexOfKey(this WatchVariable variable, int indexOfKey, out double value)
        {
            var isEmpty = variable.Values.Type switch
            {
                WatchValueType.Float => variable.Values.FloatList.OriginalValues[indexOfKey].IsEmpty,
                WatchValueType.Double => variable.Values.DoubleList.OriginalValues[indexOfKey].IsEmpty,
                WatchValueType.Int => variable.Values.IntList.OriginalValues[indexOfKey].IsEmpty,
                WatchValueType.Bool => variable.Values.BoolList.OriginalValues[indexOfKey].IsEmpty,
                WatchValueType.String or WatchValueType.NotSet => variable.Values.StringList.OriginalValues[indexOfKey].IsEmpty,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            if (isEmpty)
            {
                value = 0;
                return false;
            }

            switch (variable.Values.Type)
            {
                case WatchValueType.NotSet:
                    value = 0;
                    return false;

                case WatchValueType.Float:
                    var rawValueFloat = variable.Values.FloatList.OriginalValues[indexOfKey].Value;
                    value = rawValueFloat;
                    return float.IsFinite(rawValueFloat);

                case WatchValueType.Double:
                    var rawValueDouble = variable.Values.DoubleList.OriginalValues[indexOfKey].Value;
                    value = rawValueDouble;
                    return double.IsFinite(rawValueDouble);

                case WatchValueType.Int:
                    value = variable.Values.IntList.OriginalValues[indexOfKey].Value;
                    return true;

                case WatchValueType.Bool:
                    value = variable.Values.BoolList.OriginalValues[indexOfKey].Value ? 1 : 0;
                    return true;

                case WatchValueType.String when variable.RuntimeMeta.ValueType is { IsEnum: true }:
                    value = GetEnumNumber(
                        ref variable.EditorMeta.StringToNumberValues,
                        variable.Values.StringList.OriginalValues[indexOfKey].Value,
                        variable.RuntimeMeta.ValueType);
                    return true;

                case WatchValueType.String:
                    value = GetStringNumber(
                        ref variable.EditorMeta.StringToNumberValues,
                        variable.Values.StringList.OriginalValues[indexOfKey].Value,
                        ref variable.EditorMeta.LastStringToNumberValue);
                    return true;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public static double GetValueNumber(this WatchVariable variable, int index)
        {
            if (variable.Values.IsEmptyAt(index))
                return 0;
            
            return variable.Values.Type switch
            {
                WatchValueType.NotSet => 0,
                WatchValueType.Float => variable.Values.FloatList[index].Value,
                WatchValueType.Double => variable.Values.DoubleList[index].Value,
                WatchValueType.Int => variable.Values.IntList[index].Value,
                WatchValueType.Bool => variable.Values.BoolList[index].Value ? 1 : 0,
                WatchValueType.String when variable.RuntimeMeta.ValueType is { IsEnum: true } => GetEnumNumber(
                    ref variable.EditorMeta.StringToNumberValues, 
                    variable.Values.StringList[index].Value,
                    variable.RuntimeMeta.ValueType),
                WatchValueType.String => GetStringNumber(
                    ref variable.EditorMeta.StringToNumberValues,
                    variable.Values.StringList[index].Value,
                    ref variable.EditorMeta.LastStringToNumberValue),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        
        private static int GetStringNumber(ref Dictionary<string, int> stringToNumbers, string stringValue, ref int lastStringToNumValue)
        {
            stringToNumbers ??= new Dictionary<string, int>();

            if (stringToNumbers.TryGetValue(stringValue, out var number))
                return number;
            
            stringToNumbers.Add(stringValue, ++lastStringToNumValue);
            return lastStringToNumValue;
        }

        private static int GetEnumNumber(ref Dictionary<string, int> stringToNumbers, string stringValue, Type enumType)
        {
            if (stringToNumbers == null)
            {
                stringToNumbers = new Dictionary<string, int>();

                var names = Enum.GetNames(enumType);

                for (var i = 0; i < names.Length; i++)
                    stringToNumbers.Add(names[i], i);
            }

            return stringToNumbers.TryGetValue(stringValue, out var number) 
                    ? number
                    : 0;
        }
    }
}