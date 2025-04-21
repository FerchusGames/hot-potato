using System;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    public class WatchReferenceCreator
    {
        private static Type typeAny = typeof(Any);
        
        public WatchReference<T> Empty<T>()
        {
            return new WatchReference<T>();
        }

        public virtual WatchReference<T> GetOrAdd<T>(string path, bool directChild = true)
        {
            var variable = WatchServices.VariableCreator.GetOrAdd(Watch.Watches, path, directChild);
            var castType = typeof(T);

            if (castType != typeAny)
                variable.RuntimeMeta.ValueType ??= castType;
            
            return CreateWatchRef<T>(variable);
        }
        
        public virtual WatchReference<T> GetOrAdd<T, V>(WatchReference<V> parent, string path, bool directChild = true)
        {
            var variable = WatchServices.VariableCreator.GetOrAdd(parent.WatchVariable.Childs, path, directChild);
            
            if (directChild)
                variable.Parent = parent.WatchVariable;
            
            if (typeof(T) != typeof(Any))
                variable.RuntimeMeta.ValueType ??= typeof(T);
            
            return CreateWatchRef<T>(variable);
        }
        
        public virtual WatchReference<T> TrySetSortOrder<T>(WatchReference<T> watch, int sortOrder)
        {
            if (!watch.WatchVariable.RuntimeMeta.IsOrderSet)
                watch.WatchVariable.RuntimeMeta.SortOrder = sortOrder;

            return watch;
        }

        public void SetUpdateCall<T>(WatchReference<T> watch, Action updateCall)
        {
            watch.WatchVariable.RuntimeMeta.UpdateCall = updateCall;
        }

        public bool IsInvalidType<T>(WatchReference<T> watch)
        {
            var castType = typeof(T);
            
            if (castType == watch.WatchVariable.RuntimeMeta.ValueType || castType == typeAny)
                return false;
            
            watch.PushExtraText($"Type mismatch! Variable type is [{watch.WatchVariable.RuntimeMeta.ValueType}], not [{castType}]");
            return true;
        }

        public void MarkAsCollectionValue<T>(WatchReference<T> watch)
        {
            watch.WatchVariable.RuntimeMeta.IsCollectionValue = true;
        }
        
        public void MarkAsDictionaryKey<T>(WatchReference<T> watch)
        {
            watch.WatchVariable.RuntimeMeta.IsDictionaryKey = true;
        }
        
        public void MarkAsDictionaryValue<T>(WatchReference<T> watch)
        {
            watch.WatchVariable.RuntimeMeta.IsDictionaryValue = true;
        }
        
        public bool IsSetUp<T>(WatchReference<T> watch)
        {
            return watch.WatchVariable.RuntimeMeta.IsSetUp;
        }
        
        public bool IsDictionaryKey<T>(WatchReference<T> watch)
        {
            return watch.WatchVariable.RuntimeMeta.IsDictionaryKey;
        }
        
        public bool IsDictionaryValue<T>(WatchReference<T> watch)
        {
            return watch.WatchVariable.RuntimeMeta.IsDictionaryValue;
        }
        
        public bool IsCollectionValue<T>(WatchReference<T> watch)
        {
            return watch.WatchVariable.RuntimeMeta.IsCollectionValue;
        }

        public WatchReference<T> PushEmpty<T>(WatchReference<T> watch, bool withRoot = true, int maxRecursionDepth = 10)
        {
            WatchServices.VariableCreator.PushEmpty(watch.WatchVariable, withRoot, maxRecursionDepth);
            return watch;
        }

        public WatchReference<double> PushDouble(WatchReference<double> watch, double value)
        {
            watch.WatchVariable.Values.DoubleList.Add(new WatchValue<double>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<float> PushFloat(WatchReference<float> watch, float value)
        {
            watch.WatchVariable.Values.FloatList.Add(new WatchValue<float>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<int> PushInt(WatchReference<int> watch, int value)
        {
            watch.WatchVariable.Values.IntList.Add(new WatchValue<int>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<string> PushString(WatchReference<string> watch, string value)
        {
            watch.WatchVariable.Values.StringList.Add(new WatchValue<string>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<bool> PushBool(WatchReference<bool> watch, bool value)
        {
            watch.WatchVariable.Values.BoolList.Add(new WatchValue<bool>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<decimal> PushDecimal(WatchReference<decimal> watch, decimal value)
        {
            watch.WatchVariable.Values.DoubleList.Add(new WatchValue<double>((double)value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<long> PushLong(WatchReference<long> watch, long value)
        {
            watch.WatchVariable.Values.DoubleList.Add(new WatchValue<double>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<short> PushShort(WatchReference<short> watch, short value)
        {
            watch.WatchVariable.Values.IntList.Add(new WatchValue<int>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<byte> PushByte(WatchReference<byte> watch, byte value)
        {
            watch.WatchVariable.Values.IntList.Add(new WatchValue<int>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<ulong> PushULong(WatchReference<ulong> watch, ulong value)
        {
            watch.WatchVariable.Values.DoubleList.Add(new WatchValue<double>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<ushort> PushUShort(WatchReference<ushort> watch, ushort value)
        {
            watch.WatchVariable.Values.IntList.Add(new WatchValue<int>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<sbyte> PushSByte(WatchReference<sbyte> watch, sbyte value)
        {
            watch.WatchVariable.Values.IntList.Add(new WatchValue<int>(value));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<char> PushChar(WatchReference<char> watch, char value)
        {
            watch.WatchVariable.Values.StringList.Add(new WatchValue<string>(WatchServices.NameBuilder.GetString(value)));
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }
        
        public WatchReference<T> PushNull<T>(WatchReference<T> watch, int maxRecursionDepth = 10)
        {
            watch.WatchVariable.Values.StringList.Add(new WatchValue<string>("NULL"));
            WatchServices.VariableCreator.PushEmpty(watch.WatchVariable, false, maxRecursionDepth);
            AfterPush(watch);
            return watch;
        }

        public WatchReference<T> PushNonBasic<T>(WatchReference<T> watch, T value)
        {
            watch.WatchVariable.Values.PushEmpty();
            WatchServices.ValueFormatUpdater.TryUpdateLastValueFormat(watch, value);
            AfterPush(watch);
            return watch;
        }

        public WatchReference<T> TrySetupAs<T>(WatchReference<T> watch, WatchValueType type)
        {
            if (watch.WatchVariable.RuntimeMeta.IsSetUp)
                return watch;

            watch.WatchVariable.Values.Type = type;
            watch.WatchVariable.RuntimeMeta.ValueType = typeof(T);
            watch.WatchVariable.RuntimeMeta.IsSetUp = true;
   
            return watch;
        }

        private void AfterPush<T>(WatchReference<T> watch)
        {
            WatchServices.VariableUpdater.UpdateTotalValuesCount(watch.WatchVariable);
            WatchServices.VariableSortUpdater.TrySortByVariable(watch.WatchVariable);
            WatchServices.VariableUpdater.AnyPushSinceUpdate = true;
            
            if (watch.WatchVariable.RuntimeMeta.IsTraceable && !WatchServices.VariableUpdater.IsUpdatingNow)
                WatchServices.StackTraceUpdater.PushStackTrace(watch.WatchVariable, StackTraceUtility.ExtractStackTrace());
        }
        
        private WatchReference<T> CreateWatchRef<T>(WatchVariable variable)
        {
            return new WatchReference<T>(variable);
        }

        #region Obsolete
        
        public WatchReference<T> PushEmptyRoot<T>(WatchReference<T> watch)
        {
            watch.WatchVariable.Values.PushEmpty();
            WatchServices.VariableUpdater.UpdateTotalValuesCount(watch.WatchVariable);
            return watch;
        }
        
        public void SetValuesType<T>(WatchReference<T> watch, WatchValueType valueType)
        {
            watch.WatchVariable.Values.Type = valueType;
            watch.WatchVariable.RuntimeMeta.ValueType = typeof(T);
        }
        
        public void MarkAsSetUp<T>(WatchReference<T> watch)
        {
            watch.WatchVariable.RuntimeMeta.IsSetUp = true;
        }

        public void PushFloat<T>(WatchReference<T> watch, float value)
        {
            watch.WatchVariable.Values.FloatList.Add(new WatchValue<float>(value));
            WatchServices.VariableUpdater.UpdateTotalValuesCount(watch.WatchVariable);
        }
        
        public void PushDouble<T>(WatchReference<T> watch, double value)
        {
            watch.WatchVariable.Values.DoubleList.Add(new WatchValue<double>(value));
            WatchServices.VariableUpdater.UpdateTotalValuesCount(watch.WatchVariable);
        }
        
        public void PushInt<T>(WatchReference<T> watch, int value)
        {
            watch.WatchVariable.Values.IntList.Add(new WatchValue<int>(value));
            WatchServices.VariableUpdater.UpdateTotalValuesCount(watch.WatchVariable);
        }
        
        public void PushString<T>(WatchReference<T> watch, string value)
        {
            watch.WatchVariable.Values.StringList.Add(new WatchValue<string>(value));
            WatchServices.VariableUpdater.UpdateTotalValuesCount(watch.WatchVariable);
        }
        
        public void PushBool<T>(WatchReference<T> watch, bool value)
        {
            watch.WatchVariable.Values.BoolList.Add(new WatchValue<bool>(value));
            WatchServices.VariableUpdater.UpdateTotalValuesCount(watch.WatchVariable);
        }
        
        #endregion
    }
}