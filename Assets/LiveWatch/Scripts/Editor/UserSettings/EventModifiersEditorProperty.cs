using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public class EventModifiersEditorProperty : EditorProperty<EventModifiers>
    {
        public EventModifiersEditorProperty(string key, EventModifiers defaultValue) : base(key, defaultValue)
        {
            
        }
        protected override bool IsEqual(EventModifiers left, EventModifiers right)
        {
            return left == right;
        }

        protected override EventModifiers Load()
        {
            return (EventModifiers)EditorPrefs.GetInt(Key);
        }

        protected override void Save(EventModifiers value)
        {
            EditorPrefs.SetInt(Key, (int)value);
        }

        protected override EditorPropertyDrawer<EventModifiers> GetDrawer()
        {
            return new EventModifiersEditorPropertyDrawer(this);
        }

    }
    
    public class EventModifiersEditorPropertyDrawer : EditorPropertyDrawer<EventModifiers>
    {
        public EventModifiersEditorPropertyDrawer(EditorProperty<EventModifiers> property) : base(property)
        {
            
        }
        
        public override void Draw(params GUILayoutOption[] options)
        {
            Property.Value = (EventModifiers)EditorGUILayout.EnumFlagsField(Property, options);
        }
    }
}