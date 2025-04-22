using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public static class UserSettings
    {
        private const string PREFIX = "LivwWatch.";

        public static EditorProperty<EventModifiers> WidthZoomKeys = new EventModifiersEditorProperty($"{PREFIX}WidthZoomKeys", EventModifiers.Control);
        public static EditorProperty<EventModifiers> HeightZoomKeys = new EventModifiersEditorProperty($"{PREFIX}HeightZoomKeys", EventModifiers.Shift);
        public static EditorProperty<EventModifiers> ScrollValuesKeys = new EventModifiersEditorProperty($"{PREFIX}ScrollValuesKeys", EventModifiers.Alt);
        
        public static EditorProperty<KeyCode> FlipSelectionKey = new KeyCodeEditorProperty($"{PREFIX}FlipSelectionKey", KeyCode.F);
        public static EditorProperty<KeyCode> ExpandVariableKey = new KeyCodeEditorProperty($"{PREFIX}ExpandVariableKey", KeyCode.Space);
        public static EditorProperty<KeyCode> PreviousValueKey = new KeyCodeEditorProperty($"{PREFIX}PreviousValueKey", KeyCode.LeftArrow);
        public static EditorProperty<KeyCode> NextValueKey = new KeyCodeEditorProperty($"{PREFIX}NextValueKey", KeyCode.RightArrow);
        public static EditorProperty<KeyCode> PreviousVariableKey = new KeyCodeEditorProperty($"{PREFIX}PreviousVariableKey", KeyCode.UpArrow);
        public static EditorProperty<KeyCode> NextVariableKey = new KeyCodeEditorProperty($"{PREFIX}NextVariableKey", KeyCode.DownArrow);
        
        public static void RestoreDefaultValues()
        {
            WidthZoomKeys.SetToDefault();
            HeightZoomKeys.SetToDefault();
            ScrollValuesKeys.SetToDefault();
            
            FlipSelectionKey.SetToDefault();
            ExpandVariableKey.SetToDefault();
            PreviousValueKey.SetToDefault();
            NextValueKey.SetToDefault();
            PreviousVariableKey.SetToDefault();
            NextVariableKey.SetToDefault();
        }
    }
}