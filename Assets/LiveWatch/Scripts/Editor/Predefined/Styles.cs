using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public static class Styles
    {
        public static readonly GUIStyle VariableLabel;
        public static readonly GUIStyle VariableFoldoutLabel;
        public static readonly GUIStyle VariableGraphMaxLabel;
        public static readonly GUIStyle VariableGraphMinLabel;
        
        public static readonly GUIStyle VariableValue;
        public static readonly GUIStyle VariableValueSelected;
        public static readonly GUIStyle VariableValueLeft;
        public static readonly GUIStyle VariableValueSelectedLeft;
        
        public static readonly GUIStyle InfoValueText;
        public static readonly GUIStyle InfoMetaText;
        public static readonly GUIStyle InfoHeaderFoldout;
        public static readonly GUIStyle InfoCopyButton;
        public static readonly GUIStyle InfoCustomButton;
        
        public static readonly GUIStyle SettingsSubTitleText;
        
        public static readonly GUIStyle ElementBackground = (GUIStyle) "RL Element";
        
        static Styles()
        {
            VariableLabel = new GUIStyle(EditorStyles.label);
            VariableFoldoutLabel = new GUIStyle(EditorStyles.foldout);
            
            VariableGraphMaxLabel = new GUIStyle(new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.UpperRight,
                fontSize = Constants.VariableGraphMinMaxFontSize
            });
            
            VariableGraphMinLabel = new GUIStyle(new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.LowerRight,
                fontSize = Constants.VariableGraphMinMaxFontSize
            });
            
            VariableValue = new GUIStyle(new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Constants.VariableValueFontSize
            });

            VariableValueLeft = new GUIStyle(new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = Constants.VariableValueFontSize
            });

            VariableValueSelected = new GUIStyle(new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Constants.VariableSelectedValueFontSize
            });
            
            VariableValueSelectedLeft = new GUIStyle(new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = Constants.VariableSelectedValueFontSize
            });

            InfoValueText = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.UpperLeft,
                wordWrap = true
            };
            InfoMetaText = "CN Message";

            InfoHeaderFoldout = new GUIStyle(EditorStyles.foldout)
            {
                fontSize = 10,
                fontStyle = FontStyle.Italic,
            };

            InfoCopyButton = new GUIStyle(EditorStyles.toolbarButton)
            {
                fixedHeight = 14,
                fontSize = 10
            };

            InfoCustomButton = new GUIStyle(GUI.skin.button);
            
            SettingsSubTitleText = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 14,
            };
        }
    }
}