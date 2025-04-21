using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public class WatchStorage : ISerializationCallbackReceiver
    {
        public int Count => _dictionary.Count;
        public Dictionary<string, WatchVariable> Items => _dictionary;
        public List<string> SortedNames => _sortedNames;

        private readonly Dictionary<string, WatchVariable> _dictionary = new();
        private readonly List<string> _sortedNames = new();

        private object _lockObject = new ();
        [SerializeField] private List<string> _keys = new();
        [SerializeField] private List<WatchVariable> _values = new();

        public void Add(WatchVariable variable)
        {
            lock (_lockObject)
            {
                _dictionary.TryAdd(variable.Name, variable);
                _sortedNames.Add(variable.Name);
            }
        }

        public WatchVariable Get(string variableName)
        {
            return _dictionary[variableName];
        }

        public bool TryGet(string variableName, out WatchVariable watchVariable)
        {
            return _dictionary.TryGetValue(variableName, out watchVariable);
        }
        
        public void Remove(string variableName)
        {
            lock (_lockObject)
            {
                _dictionary.Remove(variableName, out _);
                _sortedNames.Add(variableName);
            }
        }
        
        public bool Contains(string variableName)
        {
            return _dictionary.ContainsKey(variableName);
        }

        public WatchVariable GetRelative(string fullVariableName)
        {
            var innerNames = fullVariableName.Split(Watch.PathSeparator);
            var currentStorage = this;
            WatchVariable variable = default;

            foreach (var name in innerNames)
            {
                variable = currentStorage.Get(name);
                currentStorage = variable.Childs;
            }

            return variable;
        }
        
        public bool TryGetRelative(string fullVariableName, out WatchVariable watchVariable)
        {
            watchVariable = default;
            
            var innerNames = fullVariableName.Split(Watch.PathSeparator);
            var currentStorage = this;

            foreach (var name in innerNames)
            {
                var hasVariable = currentStorage.TryGet(name, out watchVariable);

                if (!hasVariable)
                {
                    return false;
                }
                
                currentStorage = watchVariable.Childs;
            }

            return true;
        }

        public void Clear()
        {
            _dictionary.Clear();
            _sortedNames.Clear();
        }
        
        #region Serialization

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
    
            foreach (var pair in _dictionary)
            {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _dictionary.Clear();
            _sortedNames.Clear();

            var count = Math.Min(_keys.Count, _values.Count);
            
            if (count == 0)
                return;
            
            for (var i = 0; i < count; i++)
            {
                _dictionary.TryAdd(_keys[i], _values[i]);
                _sortedNames.Add(_keys[i]);
            }
            
            WatchServices.VariableSortUpdater.SortWatches(this);
        }

        #endregion
    }
}