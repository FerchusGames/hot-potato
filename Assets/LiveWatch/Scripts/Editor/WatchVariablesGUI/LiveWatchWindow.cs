using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Profiling;

namespace Ingvar.LiveWatch.Editor
{
    public class LiveWatchWindow : EditorWindow
    {
        public static bool IsRepaintRequested { get; set; }
        public static int SelectedColumnIndex { get; set; } = -1;
        public static float MaxPlayRepaintDelay { get; set; } = 1f;
        public static float MinSoftRepaintDelay { get; set; } = 0.3f;
        
        private Vector2 _watchesScrollPosition;

        [SerializeField] private ToolbarGUI _toolbarGUI = new ToolbarGUI();
        [SerializeField] private SearchGUI _searchGUI = new SearchGUI();
        [SerializeField] private VariablesListGUI _variablesListGUI = new VariablesListGUI();
        [SerializeField] private InfoGUI _infoGUI = new InfoGUI();
        
        [SerializeField] private ResizerGUI _horizontalResizerSearchToList = new ResizerGUI(true, 8, 1, Color.black);
        [SerializeField] private ResizerGUI _horizontalResizerListToInfo = new ResizerGUI(true, 8, 1, Color.black, 1);
        [SerializeField] private ResizerGUI _verticalResizerNameToValues = new ResizerGUI(false, 8, 1, Color.black);

        private float previousRepaintTime;

        [MenuItem("Window/LiveWatch")]
        static void Init()
        {
            LiveWatchWindow window = (LiveWatchWindow)EditorWindow.GetWindow(typeof(LiveWatchWindow));
            window.titleContent = new GUIContent("LiveWatch");
            window.Show();
        }

        private void OnEnable()
        {
            IsRepaintRequested = false;
            _variablesListGUI.OnEnable();
            EditorApplication.update += OnEditorUpdate;
            Watch.OnClearedAll += OnWatchClearedAll;
            Watch.OnDestroyedAll += OnWatchDestroyedAll;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
            Watch.OnClearedAll -= OnWatchClearedAll;
            Watch.OnDestroyedAll -= OnWatchDestroyedAll;
        }

        private void OnEditorUpdate()
        {
            if (EditorApplication.isPlaying)
            {
                if (Time.realtimeSinceStartup < previousRepaintTime + MaxPlayRepaintDelay) 
                    return;
            
                Repaint();
            }
            else if (IsRepaintRequested)
            {
                if (Time.realtimeSinceStartup < previousRepaintTime + MinSoftRepaintDelay)
                    return;
                
                IsRepaintRequested = false;
                Repaint();
            }
        }
        
        private void OnGUI()
        {
            if (Event.current.type == EventType.Repaint)
                previousRepaintTime = Time.realtimeSinceStartup;
            
            Profiler.BeginSample("Watch GUI");
            
            HandleResizers();

            DrawVariableList();
            
            DrawResizers();
            
            DrawToolbar();
            DrawSearch();
            DrawInfo();

            Profiler.EndSample();
        }

        private void OnWatchClearedAll()
        {
            _variablesListGUI.Clear();
        }

        private void OnWatchDestroyedAll()
        {
            _variablesListGUI.Clear();
        }
        
        private void HandleResizers()
        {
            if (_toolbarGUI.Search)
            {
                _horizontalResizerSearchToList.LocalArea = new Rect(0, 0, position.width, position.height)
                    .CropFromStartToPosition(CropEdge.TopLocal, _horizontalResizerListToInfo.Position)
                    .CropFromPositionToEnd(CropEdge.TopLocal, Constants.ToolbarAreaHeight + Constants.SearchAreaMinHeight)
                    .CropFromPositionToEnd(CropEdge.BottomLocal, Constants.VariablesAreaMinHeight);
                
                _horizontalResizerSearchToList.ProcessHandle();
            }
            
            _verticalResizerNameToValues.LocalArea = new Rect(0, 0, position.width, position.height)
                .CropFromStartToPosition(CropEdge.TopLocal, _horizontalResizerListToInfo.Position)
                .CropFromPositionToEnd(CropEdge.TopLocal, _toolbarGUI.Search ? _horizontalResizerSearchToList.Position : Constants.ToolbarAreaHeight)
                .CropFromPositionToEnd(CropEdge.LeftLocal, Constants.VariableLabelMinWidth)
                .CropFromPositionToEnd(CropEdge.RightLocal, Constants.VariableValuesMinWidth);
            
            _horizontalResizerListToInfo.LocalArea = new Rect(0, 0, position.width, position.height)
                .CropFromPositionToEnd(CropEdge.TopLocal, Constants.ToolbarAreaHeight + Constants.SearchAreaMinHeight + Constants.VariablesAreaMinHeight)
                .CropFromPositionToEnd(CropEdge.BottomLocal, Constants.InfoAreaMinHeight);
            
            _verticalResizerNameToValues.ProcessHandle();
            _horizontalResizerListToInfo.ProcessHandle();
        }

        private void DrawResizers()
        {
            if (_toolbarGUI.Search)
            {
                _horizontalResizerSearchToList.DrawLine();
            }

            _verticalResizerNameToValues.DrawLine();
            _horizontalResizerListToInfo.DrawLine();
        }
        
        private void DrawToolbar()
        {
            var toolbarRect = new Rect(0, 0, position.width + 1, Constants.ToolbarAreaHeight);

            _toolbarGUI.WindowRect = position;
            _toolbarGUI.OnGUI(toolbarRect);
        }

        private void DrawSearch()
        {
            if (!_toolbarGUI.Search)
            {
                return;
            }
            
            var searchRect = new Rect(0, 0, position.width + 1, position.height)
                .CropFromPositionToPosition(CropEdge.TopLocal, Constants.ToolbarAreaHeight + 1, _horizontalResizerSearchToList.Position);
            
            _searchGUI.OnGUI(searchRect);
        }

        private void DrawVariableList()
        {
            var contentRect = new Rect(0, 0, position.width + 1, position.height)
                .CropFromPositionToPosition(
                    CropEdge.TopLocal, 
                    _toolbarGUI.Search ? _horizontalResizerSearchToList.Position : Constants.ToolbarAreaHeight, 
                    _horizontalResizerListToInfo.Position);

            _variablesListGUI.Search = _toolbarGUI.Search;
            _variablesListGUI.NameColumnWidth = _verticalResizerNameToValues.Position;
            _variablesListGUI.OnGUI(contentRect);
        }
        
        private void DrawInfo()
        {
            var infoRect = new Rect(0, _horizontalResizerListToInfo.Position + 1, position.width + 1, position.height - _horizontalResizerListToInfo.Position - 1);
            
            _infoGUI.SelectedVariable = _variablesListGUI.SelectedVariable;
            _infoGUI.IsTitleSelected = _variablesListGUI.IsTitleSelected;
            _infoGUI.SelectedColumn = _variablesListGUI.SelectedColumnIndex;
            
            _infoGUI.OnGUI(infoRect);
        }
    }
}