using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public class KeyCodeEditorProperty : EditorProperty<KeyCode>
    {
        public KeyCodeEditorProperty(string key, KeyCode defaultValue) : base(key, defaultValue)
        {
            
        }
        protected override bool IsEqual(KeyCode left, KeyCode right)
        {
            return left == right;
        }

        protected override KeyCode Load()
        {
            return (KeyCode)EditorPrefs.GetInt(Key);
        }

        protected override void Save(KeyCode value)
        {
            EditorPrefs.SetInt(Key, (int)value);
        }

        protected override EditorPropertyDrawer<KeyCode> GetDrawer()
        {
            return new KeyEditorPropertyDrawer(this);
        }

    }
    
    public class KeyEditorPropertyDrawer : EditorPropertyDrawer<KeyCode>
    {
        public KeyEditorPropertyDrawer(EditorProperty<KeyCode> property) : base(property)
        {
            
        }
        
        public override void Draw(params GUILayoutOption[] options)
        {
            Property.Value = (KeyCode)EditorGUILayout.EnumPopup(Property, options);
        }
    }
}