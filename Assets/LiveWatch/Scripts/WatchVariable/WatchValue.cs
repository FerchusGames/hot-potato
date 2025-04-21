using System;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public struct WatchValue<T> : IEquatable<WatchValue<T>> where T : IEquatable<T>
    {
        public T Value;
        public bool IsEmpty;
        public string ValueText { get; set; }
        
        public WatchValue(T value)
        {
            Value = value;
            IsEmpty = false;
            ValueText = null;
        }
        
        public static WatchValue<T> Empty()
        {
            return new WatchValue<T>()
            {
                IsEmpty = true,
            };
        }
        
        public bool Equals(WatchValue<T> other)
        {
            return this == other;
        }
        
        public static bool operator ==(WatchValue<T> left, WatchValue<T> right)
        {
            if (!left.IsEmpty && !right.IsEmpty)
                return left.Value.Equals(right.Value);

            return left.IsEmpty == right.IsEmpty;
        }

        public static bool operator !=(WatchValue<T> left, WatchValue<T> right)
        {
            return !(left == right);
        }
        
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}