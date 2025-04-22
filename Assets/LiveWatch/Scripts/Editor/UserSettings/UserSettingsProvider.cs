using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public static class UserSettingsProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateUserSettingsProvider()
        {
            var provider = new SettingsProvider("Preferences/Live Watch", SettingsScope.User)
            {
                label = "Live Watch",
                guiHandler = (searchContext) =>
                {
                    SettingsGUI();
                },

                keywords = new HashSet<string>(new[] { "Live", "Watch", "LiveWatch" })
            };

            return provider;
        }

        private static void SettingsGUI()
        {
            EditorGUILayout.Space();
            DrawShortcuts();
        }

        private static void DrawShortcuts()
        {
            EditorGUILayout.Space(5);
            GUILayout.Label("Shortcuts", Styles.SettingsSubTitleText);
            EditorGUILayout.Space(5);

            ModifierField("Cell Width Zoom In/Out", UserSettings.WidthZoomKeys);
            ModifierField("Cell Height Zoom In/Out", UserSettings.HeightZoomKeys);
            ModifierField("Values Scroll", UserSettings.ScrollValuesKeys);
            GUILayout.Space(5);
            KeyField("Flip Selection", UserSettings.FlipSelectionKey);
            KeyField("Expand selected variable", UserSettings.ExpandVariableKey);
            KeyField("Previous variable", UserSettings.NextVariableKey);
            KeyField("Next variable", UserSettings.PreviousVariableKey);
            KeyField("Previous value", UserSettings.PreviousValueKey);
            KeyField("Next value", UserSettings.NextValueKey);
            GUILayout.Space(5);

            if (GUILayout.Button("Restore default shortcuts", GUILayout.Width(200)))
            {
                UserSettings.RestoreDefaultValues();
            }
            
            void ModifierField(string labelText, EditorProperty<EventModifiers> property)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(labelText);
                property.PropertyDrawer.Draw(GUILayout.Width(100));
                GUILayout.Space(20);
                GUILayout.Label("+", EditorStyles.boldLabel, GUILayout.Width(30));
                GUILayout.Label("Mouse Scroll Up/Down", EditorStyles.label, GUILayout.Width(150));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
            
            void KeyField(string labelText, EditorProperty<KeyCode> property)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(labelText);
                property.PropertyDrawer.Draw(GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}