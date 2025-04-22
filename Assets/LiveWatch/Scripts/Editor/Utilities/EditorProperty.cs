using System;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public abstract class EditorProperty<T>
    {
        public string Key { get; }
        public T Value
        {
            get => value;
            set
            {
                if (IsEqual(this.value, value))
                    return;

                this.value = value;
                Save(value);
            }
        }

        public EditorPropertyDrawer<T> PropertyDrawer
        {
            get
            {
                if (propertyDrawer != null)
                    return propertyDrawer;

                propertyDrawer = GetDrawer();
                return propertyDrawer;
            }
        }
        
        private T value;
        private readonly T defaultValue;
        private EditorPropertyDrawer<T> propertyDrawer;
        
        public EditorProperty(string key, T defaultValue)
        {
            this.defaultValue = defaultValue;
            
            Key = key;
            value = EditorPrefs.HasKey(key) ? Load() : defaultValue;
        }

        public void SetToDefault()
        {
            Value = defaultValue;
        }
        
        protected abstract bool IsEqual(T left, T right);
        protected abstract T Load();
        protected abstract void Save(T value);
        protected abstract EditorPropertyDrawer<T> GetDrawer();

        public static implicit operator T(EditorProperty<T> p) => p.Value;
    }
    
    public abstract class EditorPropertyDrawer<T>
    {
        protected EditorProperty<T> Property { get; }
        
        public EditorPropertyDrawer(EditorProperty<T> property)
        {
            Property = property;
        }
        
        public abstract void Draw(params GUILayoutOption[] options);
    }
}