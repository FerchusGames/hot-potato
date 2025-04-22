using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public class RepetitiveValueList<T> where T : IEquatable<T>
    {
        public int Count
        {
            get => _count;
            set => _count = value;
        }
        public List<int> OriginalKeys => _originalKeys;
        public List<T> OriginalValues => _originalValues;
        
        [SerializeField] private int _count = 0;
        [SerializeField] private List<int> _originalKeys = new(1);
        [SerializeField] private List<T> _originalValues = new(1);
        
        private object _lockObject = new ();
        private int _previousIndexOfOriginalKey;
        private List<int> _indicesToOriginalKeysMap = new(1);

        public bool AnyAt(int index)
        {
            return index >= 0 && index < _count;
        }

        public bool IsOriginalAt(int index)
        {
            if (index == 0) 
                return true;
            
            var originalValueIndex = GetOriginalValueIndex(index);
            var previousOriginalValueIndex = GetOriginalValueIndex(index-1);
            
            return previousOriginalValueIndex != originalValueIndex;
        }

        public int GetOriginalIndex(int index)
        {
            var indexOfOriginalValue = GetOriginalValueIndex(index);
            return _originalKeys[indexOfOriginalValue];
        }
        
        public void Add(T value)
        {
            if (_count == 0 || !_originalValues[^1].Equals(value))
            {
                _originalKeys.Add(_count);
                _originalValues.Add(value);
            }

            _count++;
        }

        public void Expand(int expandCount)
        {
            if (_count == 0)
                return;
            
            _count += expandCount;
        }
        
        public T this[int index]
        {
            get
            {
                var indexOfOriginalValue = GetOriginalValueIndex(index);
                return _originalValues[indexOfOriginalValue];
            }
            set
            {
                var indexOfOriginalValue = GetOriginalValueIndex(index);
                _originalValues[indexOfOriginalValue] = value;
            }
        }

        public void Clear()
        {
            _count = 0;
            _originalKeys.Clear();
            _originalValues.Clear();

            _previousIndexOfOriginalKey = 0;
            _indicesToOriginalKeysMap.Clear();
        }
        
        public void UpdateIndicesMap(int desiredIndex)
        {
            var startIndex = _indicesToOriginalKeysMap.Count;
            var finishIndex = Mathf.Min(_originalKeys[^1] - 1, desiredIndex);
            var currentLastIndex = _indicesToOriginalKeysMap.Count - 1;
            
            if (_count == 0 || currentLastIndex >= finishIndex)
                return;
            
            for (var index = startIndex; index <= finishIndex; index++)
            {
                if (index == 0)
                {
                    _indicesToOriginalKeysMap.Add(0);
                    continue;
                }

                if (_previousIndexOfOriginalKey == _originalKeys.Count - 1)
                {
                    _indicesToOriginalKeysMap.Add(_indicesToOriginalKeysMap[^1]);
                    continue;
                }

                var nextOriginalKey = _originalKeys[_previousIndexOfOriginalKey + 1];
                
                if (index < nextOriginalKey)
                {
                    _indicesToOriginalKeysMap.Add(_indicesToOriginalKeysMap[^1]);
                }
                else
                {
                    _indicesToOriginalKeysMap.Add(_indicesToOriginalKeysMap[^1] + 1);
                    _previousIndexOfOriginalKey++;
                }
            }
        }

        private int GetOriginalValueIndex(int index)
        {
            if (index >= _originalKeys[^1])
                return _originalValues.Count - 1;

            lock (_lockObject)
            {
                UpdateIndicesMap(index);
            }

            return _indicesToOriginalKeysMap[index];
        }
    }
}