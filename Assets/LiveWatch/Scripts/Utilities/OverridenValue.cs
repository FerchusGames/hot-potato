using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public struct OverridenValue<T> : IEquatable<OverridenValue<T>> where T : IEquatable<T>
    {
        [SerializeField] private bool isSet;
        [SerializeField] private T value;

        public bool IsSet => isSet;
        public T Value => value;

        public OverridenValue(T initialValue)
        {
            isSet = true;
            value = initialValue;
        }
        
        public void SetValue(T setValue)
        {
            isSet = true;
            value = setValue;
        }

        public bool Equals(OverridenValue<T> other)
        {
            return isSet && other.isSet && value.Equals(other.value)
                || !isSet && !other.isSet;
        }

        public override bool Equals(object obj)
        {
            return obj is OverridenValue<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(isSet, value);
        }
    }
}