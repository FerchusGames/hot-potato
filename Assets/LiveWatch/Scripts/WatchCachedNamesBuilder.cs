using System;
using System.Collections.Generic;

namespace Ingvar.LiveWatch
{
    public class WatchCachedNamesBuilder
    {
        public string ArrayItemFormat = "Item[{0}]";
        public string DictionaryKeyFormat = "Key {0}";
        public string DictionaryValueFormat = "Value {0}";
        
        private Dictionary<int, string> _arrayItemNames = new();
        private Dictionary<int, string> _dicitonaryKeyNames = new();
        private Dictionary<int, string> _dictionaryValueNames = new();
        private Dictionary<Type, Dictionary<int, string>> _enumToStrings = new();
        
        private Dictionary<int, string> _intToStrings = new();
        private Dictionary<long, string> _longToStrings = new();
        private Dictionary<char, string> _charToStrings = new();
        
        private string _tempStr;
        
        public string GetCollectionItemName(int index)
        {
            if (_arrayItemNames.TryGetValue(index, out _tempStr))
            {
                return _tempStr;
            }

            _tempStr = string.Format(ArrayItemFormat, index);
            _arrayItemNames.Add(index, _tempStr);

            return _tempStr;
        }
        
        public string GetDictionaryKeyName(int hashCode)
        {
            if (_dicitonaryKeyNames.TryGetValue(hashCode, out _tempStr))
            {
                return _tempStr;
            }

            _tempStr = string.Format(DictionaryKeyFormat, hashCode);
            _dicitonaryKeyNames.Add(hashCode, _tempStr);

            return _tempStr;
        }
        
        public string GetDictionaryValueName(int hashCode)
        {
            if (_dictionaryValueNames.TryGetValue(hashCode, out _tempStr))
            {
                return _tempStr;
            }

            _tempStr = string.Format(DictionaryValueFormat, hashCode);
            _dictionaryValueNames.Add(hashCode, _tempStr);

            return _tempStr;
        }
        
        public string GetStringFromEnum<T>(int value) where T : Enum
        {
            var intToStrDict = GetIntToStrDict();

            if (intToStrDict.TryGetValue(value, out _tempStr))
            {
                return _tempStr;
            }

            _tempStr = Enum.GetName(typeof(T), value);
            intToStrDict.Add(value, _tempStr);

            return _tempStr;
            
            Dictionary<int, string> GetIntToStrDict()
            {
                if (_enumToStrings.TryGetValue(typeof(T), out var dict))
                    return dict;

                dict = new Dictionary<int, string>();
                _enumToStrings.Add(typeof(T), dict);
                return dict;
            }
        }
        
        public string GetString(int value)
        {
            if (_intToStrings.TryGetValue(value, out _tempStr))
            {
                return _tempStr;
            }

            _tempStr = value.ToString();
            _intToStrings.Add(value, _tempStr);

            return _tempStr;
        }
        
        public string GetString(long value)
        {
            if (_longToStrings.TryGetValue(value, out _tempStr))
            {
                return _tempStr;
            }

            _tempStr = value.ToString();
            _longToStrings.Add(value, _tempStr);

            return _tempStr;
        }

        public string GetString(char value)
        {
            if (_charToStrings.TryGetValue(value, out _tempStr))
            {
                return _tempStr;
            }

            _tempStr = value.ToString();
            _charToStrings.Add(value, _tempStr);

            return _tempStr;
        }
    }
}