using System;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class SearchToolbarGUI
    {
        public bool AutoRun = true;
        public bool IsAddRequested;
        public bool IsRunRequested;
        
        private Rect _areaRect;
        private float _currentX;
        
        public void OnGUI(Rect areaRect)
        {
            _areaRect = areaRect;
            _currentX = -1;

            GUI.Label(areaRect, string.Empty, EditorStyles.toolbar);

            DrawAddButton();
            DrawProgressBar();
            DrawPrevButton();
            DrawNextButton();
            DrawSpace();
            DrawRunButton();
            DrawAutoRunToggle();
        }

        private void DrawAddButton()
        {
            var addButtonRect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchToolbarAddWidth);
            
            if (GUI.Button(addButtonRect, "+Line", EditorStyles.toolbarButton))
            {
                IsAddRequested = true;
            }
            
            _currentX += Constants.SearchToolbarAddWidth;
        }
        
        private void DrawProgressBar()
        {
            _currentX += 1;
            
            var progressBarWidth = Mathf.Max(
                Constants.SearchToolbarProgressBarMinWidth, 
                _areaRect.width 
                - Constants.SearchToolbarMoveWidth * 2 
                - Constants.SearchToolbarSpaceWidth 
                - Constants.SearchToolbarRunWidth 
                - Constants.SearchToolbarAutoRunWidth
                - Constants.SearchToolbarAddWidth);
            
            var progressBarRect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, progressBarWidth);

            var searchProgress = WatchEditorServices.SearchEngine.SearchProgress;
            EditorGUI.ProgressBar(progressBarRect.OffsetByHeight(-2).OffsetByY(1), searchProgress, GetProgressStatus());
            
            _currentX += progressBarWidth + 2;

            string GetProgressStatus()
            {
                if (WatchEditorServices.SearchEngine.TotalResultsCount == 0)
                    return "No results";

                return $"{WatchEditorServices.SearchEngine.CurrentResultIndex + 1}/{WatchEditorServices.SearchEngine.TotalResultsCount}";
            }
        }

        private void DrawPrevButton()
        {
            var prevButtonRect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchToolbarMoveWidth);
            if (GUI.Button(prevButtonRect, "←", EditorStyles.toolbarButton))
            {
                WatchEditorServices.SearchEngine.SetPreviousResult();
            }
            _currentX += Constants.SearchToolbarMoveWidth;
        }

        private void DrawNextButton()
        {
            var nextButtonRect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchToolbarMoveWidth);
            if (GUI.Button(nextButtonRect, "→", EditorStyles.toolbarButton))
            {
                WatchEditorServices.SearchEngine.SetNextResult();
            }
            _currentX += Constants.SearchToolbarMoveWidth;
        }

        private void DrawSpace()
        {
            _currentX += Constants.SearchToolbarSpaceWidth;
        }

        private void DrawRunButton()
        {
            var runButtonRect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchToolbarRunWidth);
            if (GUI.Button(runButtonRect, "Run", EditorStyles.toolbarButton))
            {
                IsRunRequested = true;
            }
            _currentX += Constants.SearchToolbarRunWidth;
        }

        private void DrawAutoRunToggle()
        {
            var autoRunRect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchToolbarAutoRunWidth);
            AutoRun = GUI.Toggle(autoRunRect, AutoRun, "AutoRun", EditorStyles.toolbarButton);
            _currentX += Constants.SearchToolbarAutoRunWidth;
        }
    }
}