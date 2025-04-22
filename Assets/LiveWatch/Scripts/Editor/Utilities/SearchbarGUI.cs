using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class SearchbarGUI
    {
        public Rect Rect;
        public string QueryText = string.Empty;

        private SearchField _searchField;
        
        public void Draw()
        {
            _searchField ??= new SearchField();
            QueryText = _searchField.OnGUI(Rect, QueryText);

            var style = EditorStyles.toolbarSearchField;
            
            if (Event.current.type == EventType.MouseDown && !Rect.Contains(Event.current.mousePosition))
            {
                GUI.FocusControl(null);
            }
        }
    }
}