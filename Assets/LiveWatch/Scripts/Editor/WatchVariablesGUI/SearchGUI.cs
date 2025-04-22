using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class SearchGUI
    {
        private SearchToolbarGUI toolbarGUI = new ();
        private List<SearchQueryGUI> _searchGUIs = new()
        {
            new SearchQueryGUI()
        };

        private Rect _areaRect;
        private Rect _listArea;
        private Vector2 _verticalScroll;
        private bool _hasSearchUpdated;
        private float _listBottomY;
        
        public void OnGUI(Rect areaRect)
        {
            EditorGUI.DrawRect(areaRect.Extrude(ExtrudeFlags.Bottom | ExtrudeFlags.Top, -1), Colors.Background);
            
            _areaRect = areaRect;

            DrawTopPanel();
            DrawList();
            DrawScrollPlaceholder();
            InvokeSearchIfNeeded();
        }
        
        private void DrawTopPanel()
        {
            var topPanelRect = _areaRect.CropFromStartToPosition(CropEdge.TopLocal, Constants.SearchToolbarHeight)
                .Extrude(ExtrudeFlags.All, -1);

            toolbarGUI.OnGUI(topPanelRect);

            if (toolbarGUI.IsAddRequested)
            {
                toolbarGUI.IsAddRequested = false;
                
                _searchGUIs.Add(new SearchQueryGUI());
                _verticalScroll.y = Mathf.Infinity;
                
                _hasSearchUpdated = true;
            }

            if (toolbarGUI.IsRunRequested)
            {
                toolbarGUI.IsRunRequested = false;
                RunSearch();
            }
        }

        private void DrawReorderableList()
        {
            _listArea = _areaRect.CropFromPositionToEnd(CropEdge.TopLocal, Constants.SearchToolbarHeight + 1)
                .Extrude(ExtrudeFlags.Top, -1)
                .Extrude(ExtrudeFlags.Right, -15);
            
            var list = new ReorderableList(_searchGUIs, typeof(SearchQueryGUI), true, false, true, true)
            {
                drawElementCallback = (rect, index, active, focused) =>
                {
                    _searchGUIs[index].Index = index;
                    _searchGUIs[index].OnGUI(rect);
                },
                elementHeight = Constants.SearchLineHeight,
                elementHeightCallback = _ => Constants.SearchLineHeight,
            };
            list.DoList(_listArea);
        }

        private void DrawList()
        {
            _listArea = _areaRect.CropFromPositionToEnd(CropEdge.TopLocal, Constants.SearchToolbarHeight + 1)
                .Extrude(ExtrudeFlags.Top, -1)
                .Extrude(ExtrudeFlags.Right, -2);
            
            GUILayout.BeginArea(_listArea);
            _verticalScroll = GUILayout.BeginScrollView(_verticalScroll, false, false);

            for (int i = 0; i < _searchGUIs.Count; i++)
            {
                GUILayout.Label(string.Empty, GUILayout.Height(Constants.SearchLineHeight));
                var rect = GUILayoutUtility.GetLastRect().SetWidth(_listArea.width - 20);
                _listBottomY = rect.yMax;
                rect = rect.Extrude(ExtrudeFlags.Left, 4f).Extrude(ExtrudeFlags.Right, 6f);
                
                _searchGUIs[i].Index = i;
                _searchGUIs[i].OnGUI(rect);
                
                if (Event.current.type == EventType.Repaint)
                    _hasSearchUpdated |= _searchGUIs[i].IsQueryUpdated || _searchGUIs[i].IsDeleteRequested;
            }
            
            if (Event.current.type == EventType.Repaint)
                _searchGUIs.RemoveAll(s => s.IsDeleteRequested);
            
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        
        private void DrawScrollPlaceholder()
        {
            if (_listBottomY > _listArea.height)
                return;
            
            if (Event.current.type == EventType.Repaint)
            {
                var placeholderRect = _areaRect
                    .CropFromStartToPosition(CropEdge.RightLocal, Constants.VerticalScrollWidth)
                    .CropFromPositionToEnd(CropEdge.TopLocal, Constants.SearchToolbarHeight+1);
                
                EditorGUI.DrawRect(placeholderRect, Colors.Background);
                
                var style = new GUIStyle(GUI.skin.verticalScrollbar);
                style.Draw(placeholderRect, false, false, false, false);
            }
        }

        private void InvokeSearchIfNeeded()
        {
            if (Event.current.type != EventType.Repaint
                || !_hasSearchUpdated
                || !toolbarGUI.AutoRun)
            {
                return;
            }

            RunSearch();
        }

        private void RunSearch()
        {
            _hasSearchUpdated = false;
            var queries = _searchGUIs.Select(s => s.CurrentSearchQuery).ToList();
            WatchEditorServices.SearchEngine.StartSearch(WatchStorageSO.instance.Watches, queries);
        }
    }
}