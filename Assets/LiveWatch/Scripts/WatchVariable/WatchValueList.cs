using System;
using System.Collections.Generic;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public class WatchValueList
    {
        public WatchValueType Type = WatchValueType.NotSet;
        public RepetitiveValueList<WatchValue<float>> FloatList = new ();
        public RepetitiveValueList<WatchValue<double>> DoubleList = new ();
        public RepetitiveValueList<WatchValue<int>> IntList = new ();
        public RepetitiveValueList<WatchValue<bool>> BoolList = new ();
        public RepetitiveValueList<WatchValue<string>> StringList = new ();

        public List<int> OriginalKeys
        {
            get
            {
                return Type switch
                {
                    WatchValueType.Float => FloatList.OriginalKeys,
                    WatchValueType.Double => DoubleList.OriginalKeys,
                    WatchValueType.Int => IntList.OriginalKeys,
                    WatchValueType.Bool => BoolList.OriginalKeys,
                    WatchValueType.String or WatchValueType.NotSet => StringList.OriginalKeys,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
        
        public int Count
        {
            get
            {
                return Type switch
                {
                    WatchValueType.Float => FloatList?.Count ?? 0,
                    WatchValueType.Double => DoubleList?.Count ?? 0,
                    WatchValueType.Int => IntList?.Count ?? 0,
                    WatchValueType.Bool => BoolList?.Count ?? 0,
                    WatchValueType.String or WatchValueType.NotSet => StringList?.Count ?? 0,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

        public void Clear()
        {
            if (Type is WatchValueType.NotSet or WatchValueType.Float)
                FloatList.Clear();
            
            if (Type is WatchValueType.NotSet or WatchValueType.Double)
                DoubleList.Clear();
            
            if (Type is WatchValueType.NotSet or WatchValueType.Int)
                IntList.Clear();
            
            if (Type is WatchValueType.NotSet or WatchValueType.Bool)
                BoolList.Clear();
            
            if (Type is WatchValueType.NotSet or WatchValueType.String)
                StringList.Clear();
        }

        public bool IsOriginalAt(int index)
        {
            return Type switch
            {
                WatchValueType.Float => FloatList.IsOriginalAt(index),
                WatchValueType.Double => DoubleList.IsOriginalAt(index),
                WatchValueType.Int => IntList.IsOriginalAt(index),
                WatchValueType.Bool => BoolList.IsOriginalAt(index),
                WatchValueType.String or WatchValueType.NotSet => StringList.IsOriginalAt(index),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public int GetOriginalKey(int index)
        {
            return Type switch
            {
                WatchValueType.Float => FloatList.GetOriginalIndex(index),
                WatchValueType.Double => DoubleList.GetOriginalIndex(index),
                WatchValueType.Int => IntList.GetOriginalIndex(index),
                WatchValueType.Bool => BoolList.GetOriginalIndex(index),
                WatchValueType.String or WatchValueType.NotSet => StringList.GetOriginalIndex(index),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public bool IsEmptyAt(int index)
        {
            return Type switch
            {
                
                WatchValueType.Float => !FloatList.AnyAt(index) || FloatList[index].IsEmpty,
                WatchValueType.Double => !DoubleList.AnyAt(index) || DoubleList[index].IsEmpty,
                WatchValueType.Int => !IntList.AnyAt(index) || IntList[index].IsEmpty,
                WatchValueType.Bool => !BoolList.AnyAt(index) || BoolList[index].IsEmpty,
                WatchValueType.String or WatchValueType.NotSet => !StringList.AnyAt(index) || StringList[index].IsEmpty,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        public bool AnyAt(int index)
        {
            return Type switch
            {
                WatchValueType.Float => FloatList.AnyAt(index),
                WatchValueType.Double => DoubleList.AnyAt(index),
                WatchValueType.Int => IntList.AnyAt(index),
                WatchValueType.Bool => BoolList.AnyAt(index),
                WatchValueType.String or WatchValueType.NotSet => StringList.AnyAt(index),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void Expand(int count)
        {
            if (Type is WatchValueType.Float or WatchValueType.NotSet)
                FloatList.Expand(count);
            
            if (Type is WatchValueType.Double or WatchValueType.NotSet)
                DoubleList.Expand(count);
            
            if (Type is WatchValueType.Int or WatchValueType.NotSet)
                IntList.Expand(count);
            
            if (Type is WatchValueType.Bool or WatchValueType.NotSet)
                BoolList.Expand(count);
            
            if (Type is WatchValueType.String or WatchValueType.NotSet)
                StringList.Expand(count);
        }

        public void PushEmpty()
        {
            if (Type is WatchValueType.String or WatchValueType.NotSet)
                StringList.Add(WatchValue<string>.Empty());
            
            if (Type is WatchValueType.Float or WatchValueType.NotSet)
                FloatList.Add(WatchValue<float>.Empty());
            
            if (Type is WatchValueType.Double or WatchValueType.NotSet)
                DoubleList.Add(WatchValue<double>.Empty());
            
            if (Type is WatchValueType.Int or WatchValueType.NotSet)
                IntList.Add(WatchValue<int>.Empty());
            
            if (Type is WatchValueType.Bool or WatchValueType.NotSet)
                BoolList.Add(WatchValue<bool>.Empty());
        }
    }
}