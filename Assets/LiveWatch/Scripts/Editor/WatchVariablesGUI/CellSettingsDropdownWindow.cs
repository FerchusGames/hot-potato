using System;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public class CellSettingsDropdownWindow : EditorWindow
    {
        protected float ColumnWidth
        {
            get => WatchStorageSO.instance.ColumnWidth;
            set => WatchStorageSO.instance.ColumnWidth = value;
        }

        protected float RowHeight
        {
            get => WatchStorageSO.instance.RowHeight;
            set => WatchStorageSO.instance.RowHeight = value; 
        }
        
        protected bool IsLeftSelection
        {
            get => WatchStorageSO.instance.IsLeftSelection;
            set => WatchStorageSO.instance.IsLeftSelection = value; 
        }
        
        private int selectionTypeIndex;
        private static string[] selectionTypes = new[] {"Left", "Right"};
        private static Vector2 windowSize = new Vector2(150, 90);

        public static void Create(Rect buttonRect)
        {
            var window = CreateInstance<CellSettingsDropdownWindow>();
            window.ShowAsDropDown(buttonRect, windowSize);
            window.Focus();
        }

        private void OnGUI()
        {
            var prevWidth = ColumnWidth;
            var prevHeight = RowHeight;

            DrawBackground();
            DrawWidthSlider();
            DrawHeightSlider();
            DrawSelectionPopup();
            DrawRestoreDefaultsButton();
                
            if (Math.Abs(prevWidth - ColumnWidth) > 0.001f
                || Math.Abs(prevHeight - RowHeight) > 0.001f)
            {
                var window = GetWindow<LiveWatchWindow>();
                
                EditorUtility.SetDirty(window);
                Focus();
            }
        }

        private void DrawBackground()
        {
            if (Event.current.type != EventType.Repaint)
                return;
            
            var rect = new Rect(0, 0, position.width, position.height);
            
            EditorGUI.DrawRect(rect, Color.black);
            EditorGUI.DrawRect(rect.Extrude(ExtrudeFlags.All, -1), Colors.Background);
        }

        private void DrawWidthSlider()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Width", "Width of watch cells"), GUILayout.Width(60));
            ColumnWidth = GUILayout.HorizontalSlider(ColumnWidth, Constants.VariableValueColumnWidthMin, Constants.VariableValueColumnWidthMax);
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawHeightSlider()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Height", "Height of watch cells"), GUILayout.Width(60));
            RowHeight = GUILayout.HorizontalSlider(RowHeight, Constants.VariableRowHeighMin, Constants.VariableRowHeightMax);
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSelectionPopup()
        {
            selectionTypeIndex = IsLeftSelection ? 0 : 1;
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(new GUIContent("Selection", "The direction of cell's selection"), GUILayout.Width(60));
            selectionTypeIndex = EditorGUILayout.Popup(selectionTypeIndex, selectionTypes);
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
            
            IsLeftSelection = selectionTypeIndex == 0;
        }

        private void DrawRestoreDefaultsButton()
        {
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button(new GUIContent("Restore defaults", "Reset all view preferences to default values")))
            {
                ColumnWidth = 30;
                RowHeight = 30;
                IsLeftSelection = false;
            }
        }
    }
}