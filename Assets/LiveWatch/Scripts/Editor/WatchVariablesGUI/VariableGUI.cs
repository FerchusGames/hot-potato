using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class VariableGUI
    {
        public int Index { get; set; }
        public int IndentLevel { get; set; }
        public float LabelAreaWidth { get; set; }
        public float ValueColumnWidth { get; set; }
        public float ValueRowHeight { get; set; }
        public bool IsMouseDraggingOverValues { get; set; }
        public float HorizontalScrollValue { get; set; }
        public int SelectedColumnIndex { get; set; }
        public bool Search { get; set; }
        public bool Collapse { get; set; }
        public Rect VariablesTotalArea { get; set; }
        public int StartIndex { get; set; }
        public float RectOffset { get; set; }
        public List<int> IndicesToDisplay { get; set; }
        public int IndicesCount { get; set; }
        public bool IsSelected { get; set; }
        public VariableSelectedFitInfo SelectedFitInfo { get; set; }
        public bool PreferRightSelection { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public bool IsNarrowMode => ValueColumnWidth <= 12;
        public VariableClickInfo ClickInfo => _clickInfo;
        
        protected Color BackgroundColor => Index % 2 == 0 ? Colors.Background : Colors.BackgroundOdd;
        protected VariableCellGUI CellGUI
        {
            get { return _cellGUI ??= new VariableCellGUI(); }
        }
        protected VariableGraphGUI GraphGUI
        {
            get { return _graphGUI ??= new VariableGraphGUI(); }
        }
        
        [SerializeField] private VariableCellGUI _cellGUI;
        [SerializeField] private VariableGraphGUI _graphGUI;
        
        private WatchVariable _variable;
        private VariableClickInfo _clickInfo;
        private List<int> _segmentIndices = new(100);

        public void Draw(Rect rect, WatchVariable variable)
        {
            _variable = variable;

            _clickInfo = new VariableClickInfo()
            {
                CurrentPositionIndex = -1
            };

            var valuesRect = rect.CropFromPositionToEnd(CropEdge.LeftLocal, LabelAreaWidth);

            CellGUI.IsSelected = IsSelected;
            CellGUI.Search = Search;
            CellGUI.Variable = variable;
            CellGUI.SelectedColumnIndex = SelectedColumnIndex;
            CellGUI.IsNarrowMode = IsNarrowMode;
            CellGUI.ValueColumnWidth = ValueColumnWidth;
            CellGUI.ValuesRect = valuesRect;
            CellGUI.SelectedFitInfo = SelectedFitInfo;
            CellGUI.PreferRightSelection = PreferRightSelection;
            CellGUI.IndicesToDisplay = IndicesToDisplay;
            
            ProcessRowEvents(rect);

            DrawValuesBackground(valuesRect);
            DrawValues(valuesRect);

            var labelBackgroundRect = rect.CropFromPositionToPosition(CropEdge.LeftLocal, 0, LabelAreaWidth);
            var labelRect = rect.CropFromPositionToPosition(CropEdge.LeftLocal,
                Constants.VariableLabelOffset + Constants.VariableLabelIndentWidth * IndentLevel, LabelAreaWidth);

            DrawLabelBackground(labelBackgroundRect);
            ProcessLabelEvents(labelRect);
            DrawLabelContent(labelRect);
            DrawMinMaxForGraph(labelRect);
                
            if (IsSelected && variable.HasValues)
            {
                GUIExtensions.DrawColorFrame(SelectedColumnIndex > 0 ? rect : labelBackgroundRect, Colors.CellSelectionLine, 3);
            }
        }

        private void DrawLabelBackground(Rect rect)
        {
            EditorGUI.DrawRect(rect, BackgroundColor);
            
            var titleFormat = _variable.RuntimeMeta.Formatting.TitleFormat;
            if (titleFormat is { BackColor: { IsSet: true } })
                EditorGUI.DrawRect(rect, titleFormat.BackColor.Value);
        }
        
        #region Full row

        private void ProcessRowEvents(Rect rect)
        {
            if (!VariablesTotalArea.Contains(Event.current.mousePosition) 
                || Event.current.type != EventType.MouseDown && GUIUtility.hotControl != 0)
                return;

            GUIUtility.hotControl = 0;
            
            if (rect.Contains(Event.current.mousePosition))
            {
                if (Event.current.isMouse && Event.current.button == 0)
                {
                    _clickInfo.IsMouse = true;
                }
            }
        }

        #endregion

        #region Label zone

        private void ProcessLabelEvents(Rect rect)
        {
            if (!VariablesTotalArea.Contains(Event.current.mousePosition) || IsMouseDraggingOverValues)
            {
                return;
            }
            
            if (rect.Contains(Event.current.mousePosition))
            {
                _clickInfo.CurrentPositionIndex = -1;
                _clickInfo.IsOverTitleArea = true;
            }
        }

        private void DrawLabelContent(Rect rect)
        {
            if (!_variable.HasChilds)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    DrawLabelWitchSearchResult(
                        rect, 
                        _variable.Name, 
                        _variable.EditorMeta.SearchResult.NameResult,
                        Styles.VariableLabel);
                }
                return;
            }

            if (Event.current.type == EventType.Layout || Event.current.type == EventType.Repaint || rect.Contains(Event.current.mousePosition))
            {
                _variable.EditorMeta.IsExpanded = EditorGUI.Foldout(
                    rect,
                    _variable.EditorMeta.IsExpanded,
                    string.Empty,
                    Styles.VariableFoldoutLabel);
                
                if (Event.current.type == EventType.Used)
                    _clickInfo.IsOverTitleArea = false;
            }

            if (Event.current.type == EventType.Repaint)
            {
                DrawLabelWitchSearchResult(
                    rect.CropFromPositionToEnd(CropEdge.LeftLocal, 15),
                    _variable.Name, 
                    _variable.EditorMeta.SearchResult.NameResult,
                    Styles.VariableLabel);
            }
        }

        private void DrawMinMaxForGraph(Rect rect)
        {
            if (Event.current.type != EventType.Repaint
                || _variable.Values.Type is WatchValueType.String or WatchValueType.Bool or WatchValueType.NotSet
                || !IsNarrowMode
                || ValueRowHeight < 38)
            {
                return;
            }

            var maxValueRect = rect
                .CropFromPositionWithSize(CropEdge.TopLocal, 2 , Constants.VariableGraphMinValueHeight)
                .CropFromPositionToEnd(CropEdge.RightLocal, 2);
            
            var minValueRect = rect
                .CropFromPositionWithSize(CropEdge.BottomLocal, 2, Constants.VariableGraphMinValueHeight)
                .CropFromPositionToEnd(CropEdge.RightLocal, 2);

            GUI.Label(maxValueRect, MaxValue.ToString(), Styles.VariableGraphMaxLabel);
            GUI.Label(minValueRect, MinValue.ToString(), Styles.VariableGraphMinLabel);
        }
        
        private void DrawLabelWitchSearchResult(Rect labelRect, string text, SearchQueryResult searchResult, GUIStyle style)
        {
            if (searchResult.IsPositive)
            {
                var charStartIndex = searchResult.IsWholeSelection ? 0 : searchResult.SelectionStartIndex;
                var charEndIndex = searchResult.IsWholeSelection ? text.Length : searchResult.SelectionEndIndex;
                    
                style.DrawWithTextSelection(
                    labelRect, 
                    WatchEditorServices.GUICache.GetContent(text), 
                    -1, 
                    charStartIndex, 
                    charEndIndex);
            }
            else
            {
                style.Draw(labelRect, WatchEditorServices.GUICache.GetContent(text), 0);
            }
        }
        
        #endregion

        #region Values zone

        private void DrawValuesBackground(Rect rect)
        {
            if (Event.current.type is not EventType.Repaint)
                return;
            
            EditorGUI.DrawRect(rect, BackgroundColor);
        }
        
        private void DrawValues(Rect rect)
        {
            if (IndicesToDisplay.Count > 0
                && (Event.current.type is EventType.MouseDown && VariablesTotalArea.Contains(Event.current.mousePosition) 
                 || IsMouseDraggingOverValues)
                && Event.current.isMouse
                && Event.current.button == 0
                && GUIUtility.hotControl == 0)
            {
                if (rect.Contains(Event.current.mousePosition))
                {
                    var clickedValueIndex = (int)Mathf.Floor((Event.current.mousePosition.x - (rect.x + RectOffset)) / ValueColumnWidth);
                    clickedValueIndex = Mathf.Clamp(clickedValueIndex, 0, IndicesToDisplay.Count - 1);

                    _clickInfo.IsMouse = true;
                    _clickInfo.CurrentPositionIndex = IndicesToDisplay[clickedValueIndex];
                    _clickInfo.MouseButton = 0;
                }
                else
                {
                    var selectedIndex = 0;
                    _clickInfo.CurrentPositionIndex = IndicesToDisplay[selectedIndex];
                }
            }
            
            if (IndicesCount == 0 || Event.current.type != EventType.Repaint)
            {
                TryDrawSelectedCell();
                return;
            }

            if (IsNarrowMode)
                DrawValuesAsGraph();
            else
                DrawValuesAsCells();

            TryDrawSelectedCell();

            if (_variable.HasChilds && !_variable.EditorMeta.IsExpanded)
            {
                var previewRect = rect
                    .CropFromPositionWithSize(CropEdge.LeftLocal, RectOffset, IndicesCount * ValueColumnWidth)
                    .Extrude(ExtrudeFlags.Top | ExtrudeFlags.Bottom, -1);
                var drawRect = IsSelected ? previewRect.Extrude(ExtrudeFlags.Top | ExtrudeFlags.Bottom, -3) : previewRect;

                WatchEditorServices.PreviewDrawer.Search = Search;
                WatchEditorServices.PreviewDrawer.DrawPreview(previewRect, drawRect, _variable, IndicesToDisplay, Mathf.CeilToInt(ValueColumnWidth));
            }

            void DrawValuesAsGraph()
            {
                GraphGUI.Search = Search;
                GraphGUI.RowIndex = Index;
                GraphGUI.Variable = _variable;
                GraphGUI.IndicesToDisplay = IndicesToDisplay;
                GraphGUI.StartIndex = StartIndex;
                GraphGUI.IndicesCount = IndicesCount;
                GraphGUI.MinValue = MinValue;
                GraphGUI.MaxValue = MaxValue;
                GraphGUI.ValueColumnWidth = ValueColumnWidth;
                GraphGUI.BackgroundColor = BackgroundColor;

                var graphRect = rect.CropFromPositionWithSize(CropEdge.LeftLocal, RectOffset, IndicesCount * ValueColumnWidth);
                GraphGUI.DrawValues(graphRect);
            }
            
            void DrawValuesAsCells()
            {
                _segmentIndices.Clear();
                var previousLocalIndex = 0;
                var previousKeyIndex = _variable.Values.GetOriginalKey(IndicesToDisplay[0]);

                for (var localIndex = 0; localIndex < IndicesCount; localIndex++)
                {
                    var key = IndicesToDisplay[localIndex];
                    var keyIndex = _variable.Values.GetOriginalKey(key);
                
                    var isOriginal = localIndex != 0 && previousKeyIndex != keyIndex;
                
                    if (isOriginal)
                    {
                        previousKeyIndex = keyIndex;
                        
                        DrawSegment(previousLocalIndex, localIndex - 1, _segmentIndices);
                        previousLocalIndex = localIndex;
                        _segmentIndices.Clear();
                    }
                
                    _segmentIndices.Add(key);
                }

                DrawSegment(previousLocalIndex, IndicesCount - 1, _segmentIndices);
            }
            
            void TryDrawSelectedCell()
            {
                var isAnySelected = IndicesToDisplay.Contains(SelectedColumnIndex);
                
                if (!isAnySelected)
                    return;
                
                var cellRect = rect.CropFromPositionWithSize(
                    CropEdge.LeftLocal, 
                    RectOffset + IndicesToDisplay.IndexOf(SelectedColumnIndex) * ValueColumnWidth, 
                    ValueColumnWidth);

                if (!_variable.Values.IsEmptyAt(SelectedColumnIndex) && SelectedColumnIndex < _variable.Values.Count)
                {
                    CellGUI.DrawSelectedCell(cellRect, _variable.GetValueText(SelectedColumnIndex), SelectedColumnIndex);
                }
                else
                {
                    CellGUI.DrawSelectedCelLine(cellRect);
                }
            }
            
            void DrawSegment(int startLocalIndex, int endLocalIndex, List<int> contentIndices)
            {
                var valueIndex = contentIndices[0];
                var numberValue = _variable.GetValueNumber(valueIndex);
                
                var segmentRect = rect.CropFromPositionWithSize(
                    CropEdge.LeftLocal,
                    RectOffset + startLocalIndex * ValueColumnWidth,
                    ValueColumnWidth * (endLocalIndex - startLocalIndex + 1));

                var progress = 0f;

                if (!double.IsNaN(numberValue))
                {
                    progress = (float)((MaxValue - MinValue) < 0.000001
                        ? 1
                        : (numberValue - MinValue) / (MaxValue - MinValue));
                }

                CellGUI.DrawValueCellSegment(
                    segmentRect, 
                    _variable.GetValueText(valueIndex), 
                    contentIndices, 
                    (float)progress);
            }
        }

        #endregion
    }
    
    public struct VariableClickInfo
    {
        public bool IsMouse;
        public int MouseButton;
        public int CurrentPositionIndex;
        public bool IsOverTitleArea;
    }

    public class VariableSelectedFitInfo
    {
        public bool CanFitLeft = true;
        public bool CanFitRight = true;

        public void Reset()
        {
            CanFitLeft = true;
            CanFitRight = true;
        }

        public void MergeWith(VariableSelectedFitInfo other)
        {
            CanFitLeft &= other.CanFitLeft;
            CanFitRight &= other.CanFitRight;
        }
    }
}