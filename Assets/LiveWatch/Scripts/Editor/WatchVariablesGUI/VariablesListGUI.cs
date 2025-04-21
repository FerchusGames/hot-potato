using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class VariablesListGUI
    {
        public const int MaxDrawDepth = 100;
        public WatchStorage Variables => WatchStorageSO.instance.Watches;
        public bool Search { get; set; }
        public float NameColumnWidth { get; set; } = 50;

        public bool Collapse 
        {
            get => WatchStorageSO.instance.Collapse;
            set => WatchStorageSO.instance.Collapse = value;
        }
        
        public float ValueColumnWidth
        {
            get => WatchStorageSO.instance.ColumnWidth;
            set => WatchStorageSO.instance.ColumnWidth = value;
        }

        public float ValueRowHeight
        {
            get => WatchStorageSO.instance.RowHeight;
            set => WatchStorageSO.instance.RowHeight = value; 
        }

        public bool IsLeftSelection
        {
            get => WatchStorageSO.instance.IsLeftSelection;
            set => WatchStorageSO.instance.IsLeftSelection = value; 
        }

        public int SelectedColumnIndex
        {
            get => LiveWatchWindow.SelectedColumnIndex;
            set => LiveWatchWindow.SelectedColumnIndex = value; 
        }
        
        public WatchVariable SelectedVariable { get; private set; }
        public bool IsMouseDraggingOverValues { get; set; }
        public bool IsTitleSelected => _isSelectedTitle;
        
        protected VariableGUI VariableGUI
        {
            get { return _variableGUI ??= new VariableGUI(); }
        }

        private SortedSet<int> _nonShrinkableColumns = new SortedSet<int>();
        private List<int> _indicesToDisplay = new List<int>(100);
        private int _valueStartIndex;
        private float _valueRectOffset;
        private ScrollbarGUI _horizontalScrollbar = new ScrollbarGUI(true);
        private ScrollbarGUI _verticalScrollbar = new ScrollbarGUI(false);

        private Rect _frame;
        private Rect _variableListRect;
        private Rect _currentSelectionRect;
        private bool _isSelectedTitle;
        private int _variableDrawnCounter;
        private int _variableTotalCounter;
        private int _valriableValuesMaxCount;
        private VariableGUI _variableGUI;
        private GUIStyle _horizontalScrollStyle;
        private GUIStyle _verticalScrollStyle;
        private VariableSelectedFitInfo _selectedFitInfo = new();
        private List<WatchVariable> _variables = new (10);

        public void OnEnable()
        {
            if (SelectedVariable != null)
            {
                Variables.GetAllChildRecursive(_variables, WatchFilters.None, WatchFilters.None);

                var found = false;

                foreach (var variable in _variables)
                    found |= variable == SelectedVariable;

                if (!found)
                {
                    SelectedVariable = null;
                    SelectedColumnIndex = -1;
                }
            }

            if (Variables.Count == 0)
            {
                _horizontalScrollbar.IsStickingToLast = true;
            }
        }

        public void OnGUI(Rect frame)
        {
            _frame = frame;

            ProcessEvents(Event.current);

            PrepareVariables();
            
            DoList();

            DoHorizontalScroll();
            DoVerticalScroll();
            
            ProcessSearchResultsJump();
        }

        public void Clear()
        {
            SelectedColumnIndex = -1;
            _valriableValuesMaxCount = 0;
            _isSelectedTitle = true;
            
            _nonShrinkableColumns.Clear();
            _indicesToDisplay.Clear();

            _horizontalScrollbar.ScrollValue = 0;
            _horizontalScrollbar.IsStickingToLast = true;
        }

        private void PrepareVariables()
        {
            RefreshNonShrinkableColumns();
            RefreshIndicesToDisplay();
        }

        private void DoList()
        {
            _variableDrawnCounter = 0;
            _variableTotalCounter = 0;

            var posY = _frame.y - _verticalScrollbar.ScrollValue;

            _variableListRect = _frame
                .CropFromPositionToEnd(CropEdge.RightLocal, Constants.VerticalScrollWidth)
                .CropFromPositionToEnd(CropEdge.BottomLocal, Constants.HorizontalScrollHeight);
            
            VariableGUI.LabelAreaWidth = NameColumnWidth;
            VariableGUI.ValueColumnWidth = ValueColumnWidth;
            VariableGUI.ValueRowHeight = ValueRowHeight;
            VariableGUI.HorizontalScrollValue = _horizontalScrollbar.ScrollValue;
            VariableGUI.SelectedColumnIndex = SelectedColumnIndex;
            VariableGUI.Search = Search;
            VariableGUI.Collapse = Collapse;
            VariableGUI.VariablesTotalArea = _variableListRect;
            VariableGUI.IndicesToDisplay = _indicesToDisplay;
            VariableGUI.StartIndex = _valueStartIndex;
            VariableGUI.RectOffset = _valueRectOffset;
            VariableGUI.IsMouseDraggingOverValues = IsMouseDraggingOverValues;

            foreach (var variableName in Variables.SortedNames)
            {
                posY = DrawWatchVariableRowRecursive(posY, Variables.Get(variableName), 0);
            }

            if (SelectedVariable != null)
            {
                GUIExtensions.DrawColorFrame(_currentSelectionRect, Colors.CellSelectionLine, 3);
            }
        }
        
        private void DoVerticalScroll()
        {
            if (Event.current.type == EventType.Repaint)
            {
                var placeholderRect = _frame
                    .CropFromStartToPosition(CropEdge.RightLocal, Constants.VerticalScrollWidth)
                    .CropFromPositionToEnd(CropEdge.BottomLocal, Constants.HorizontalScrollHeight);
            
                EditorGUI.DrawRect(placeholderRect, Colors.Background);
                
                _verticalScrollStyle ??= new GUIStyle(GUI.skin.verticalScrollbar);
                _verticalScrollStyle.Draw(placeholderRect, false, false, false, false);
            }

            var scrollRect = _frame
                .CropFromStartToPosition(CropEdge.RightLocal, Constants.VerticalScrollWidth)
                .CropFromPositionToEnd(CropEdge.BottomLocal, Constants.HorizontalScrollHeight);
                
            var variablesTotalHeight = GetVariablesTotalHeight();

            _verticalScrollbar.Prepare(scrollRect, scrollRect.height, 0, variablesTotalHeight);
            _verticalScrollbar.Draw();
        }
        
        private void DoHorizontalScroll()
        {
            if (Event.current.type == EventType.Repaint)
            {
                var placeholderRect = _frame.CropFromStartToPosition(CropEdge.BottomLocal, Constants.HorizontalScrollHeight);

                EditorGUI.DrawRect(placeholderRect, Colors.Background);
                
                _horizontalScrollStyle ??= new GUIStyle(GUI.skin.horizontalScrollbar);
                _horizontalScrollStyle.Draw(placeholderRect, false, false, false, false);
            }

            var scrollRect = _frame
                .CropFromStartToPosition(CropEdge.BottomLocal, Constants.HorizontalScrollHeight)
                .CropFromPositionToEnd(CropEdge.LeftLocal, NameColumnWidth);
                
            var variablesMaxWidth = GetVariablesMaxWidth();

            _horizontalScrollbar.AllowStickToLast = true;
            _horizontalScrollbar.Prepare(scrollRect, scrollRect.width-Constants.VerticalScrollWidth, 0, variablesMaxWidth);
            _horizontalScrollbar.Draw();
        }

        private void ProcessEvents(Event e)
        {
            if (e.type == EventType.MouseUp && IsMouseDraggingOverValues)
            {
                e.Use();
                IsMouseDraggingOverValues = false;
            } 
            else if (e.type == EventType.MouseDown && _frame.Contains(e.mousePosition))
            {
                GUIUtility.hotControl = 0;
                GUIUtility.keyboardControl  = 0;
            } 
            else if (e.modifiers.HasFlag(UserSettings.WidthZoomKeys) && e.isScrollWheel)
            {
                e.Use();
                
                float delta = -e.delta.y;
                var newColumnWidth = Mathf.Clamp(ValueColumnWidth + delta, Constants.VariableValueColumnWidthMin, Constants.VariableValueColumnWidthMax);

                _horizontalScrollbar.ResizeRelativeToPointer(e.mousePosition.x, newColumnWidth/ValueColumnWidth);
                
                ValueColumnWidth = newColumnWidth;
            }
            else if (e.modifiers.HasFlag(UserSettings.HeightZoomKeys) && e.isScrollWheel)
            {
                e.Use();
                
                float delta = -e.delta.y;
                var newRowHeight = Mathf.Clamp(ValueRowHeight + delta, Constants.VariableRowHeighMin, Constants.VariableRowHeightMax);

                _verticalScrollbar.ResizeRelativeToPointer(e.mousePosition.y, newRowHeight/ValueRowHeight);
                
                ValueRowHeight = newRowHeight;
            }
            else if (e.modifiers.HasFlag(UserSettings.ScrollValuesKeys) && e.isScrollWheel)
            {
                e.Use();
                
                float delta = e.delta.y * Constants.MouseScrollbarMultiplier;
                _horizontalScrollbar.ScrollValue += delta;
            }
            else if (e.isScrollWheel)
            {
                e.Use();
                
                float delta = e.delta.y * Constants.MouseScrollbarMultiplier;
                _verticalScrollbar.ScrollValue += delta;
            }
            else if (e.keyCode == UserSettings.FlipSelectionKey && e.type == EventType.KeyDown)
            {
                e.Use();
                
                IsLeftSelection = !IsLeftSelection;
            }
            else if (e.keyCode == UserSettings.ExpandVariableKey && e.type == EventType.KeyDown)
            {
                e.Use();

                if (SelectedVariable != null)
                    SelectedVariable.EditorMeta.IsExpanded = !SelectedVariable.EditorMeta.IsExpanded;
            }
            else if (e.keyCode == UserSettings.PreviousVariableKey && e.type == EventType.KeyDown)
            {
                e.Use();
                
                MoveToVariable(-1);
            }
            else if (e.keyCode == UserSettings.NextVariableKey && e.type == EventType.KeyDown)
            {
                e.Use();
                
                MoveToVariable(1);
            }
            else if (e.keyCode == UserSettings.PreviousValueKey && e.type == EventType.KeyDown)
            {
                e.Use();
                
                MoveToVariable(0, -1);
            }
            else if (e.keyCode == UserSettings.NextValueKey && e.type == EventType.KeyDown)
            {
                e.Use();
                
                MoveToVariable(0, 1);
            }
        }
        
        private void RefreshNonShrinkableColumns()
        {
            _valriableValuesMaxCount = 0;
            Variables.GetAllChildRecursive(_variables, WatchFilters.None, WatchFilters.NoValues);
            
            foreach (var variable in _variables)
            {
                _valriableValuesMaxCount = Mathf.Max(variable.Values.Count, _valriableValuesMaxCount);
                
                if (WatchServices.VariableCreator.IsAlwaysShrinkable(variable))
                {
                    continue;
                }

                var currentIndexOfOriginalKey = variable.EditorMeta.LastNonShrinkableIndexOfKey;
                var maxIndexOfOriginalKeys = variable.Values.OriginalKeys.Count - 1;

                while (currentIndexOfOriginalKey < maxIndexOfOriginalKeys)
                {
                    currentIndexOfOriginalKey++;

                    _nonShrinkableColumns.Add(variable.Values.OriginalKeys[currentIndexOfOriginalKey]);
                }

                variable.EditorMeta.LastNonShrinkableIndexOfKey = currentIndexOfOriginalKey;
            }
        }

        private void RefreshIndicesToDisplay()
        {
            _valueStartIndex = Mathf.FloorToInt(_horizontalScrollbar.ScrollValue / ValueColumnWidth);
            _valueRectOffset = _valueStartIndex * ValueColumnWidth - _horizontalScrollbar.ScrollValue;
            var valuesWidth = _frame.width - Constants.VerticalScrollWidth - NameColumnWidth;
            var columnsCount = Mathf.FloorToInt((valuesWidth - _valueRectOffset) / ValueColumnWidth);
            
            _indicesToDisplay.Clear();

            if (Collapse)
            {
                var rawIndex = 0;
                
                foreach (var column in _nonShrinkableColumns)
                {
                    if (_indicesToDisplay.Count > columnsCount)
                        break;
                    
                    if (rawIndex++ < _valueStartIndex)
                        continue;
                    
                    _indicesToDisplay.Add(column);
                }
            }
            else
            {
                for (var i = _valueStartIndex; i < _valriableValuesMaxCount; i++)
                {
                    if (_indicesToDisplay.Count > columnsCount)
                        break;
                
                    _indicesToDisplay.Add(i);
                }
            }
        }

        private float DrawWatchVariableRowRecursive(float positionY, WatchVariable variable, int recursionDepth)
        {
            if (recursionDepth > MaxDrawDepth)
            {
                return positionY;
            }

            _variableTotalCounter++;

            var listStartY = _frame.y;
            var listEndY = listStartY + _verticalScrollbar.Size;
            
            var positionFinishY = positionY + ValueRowHeight;

            if (positionY >= listEndY)
            {
                return positionY;
            }
            
            bool draw = positionFinishY > listStartY;

            var rect = new Rect(0, positionY, _frame.width - Constants.VerticalScrollWidth, ValueRowHeight);

            if (draw)
            {
                _variableDrawnCounter++;

                OnBeforeVariableDraw(variable, recursionDepth);

                VariableGUI.Draw(rect, variable);

                if (VariableGUI.ClickInfo.IsMouse)
                {
                    OnVariableClicked(variable);
                }
            }

            if (!variable.EditorMeta.IsExpanded)
            {
                if (SelectedVariable == variable)
                    _currentSelectionRect = rect;
                
                return positionFinishY;
            }

            foreach (var childVariableName in variable.Childs.SortedNames)
            {
                positionFinishY = DrawWatchVariableRowRecursive(positionFinishY, variable.Childs.Get(childVariableName), recursionDepth + 1);
            }

            if (SelectedVariable == variable)
            {
                _currentSelectionRect = rect.SetHeight(positionFinishY - positionY);
            }

            return positionFinishY;
        }

        private void OnBeforeVariableDraw(WatchVariable variable, int recursionDepth)
        {
            CalcIndicesCount();

            switch (variable.RuntimeMeta.MinMax.Mode)
            {
                case WatchMinMaxMode.Local:
                    CalcMinMaxLocal();
                    break;
                case WatchMinMaxMode.Global:
                    CalcMinMaxGlobal();
                    break;
                case WatchMinMaxMode.Custom:
                    VariableGUI.MinValue = variable.RuntimeMeta.MinMax.CustomMinValue;
                    VariableGUI.MaxValue = variable.RuntimeMeta.MinMax.CustomMaxValue;
                    VariableGUI.MaxValue = Math.Max(VariableGUI.MaxValue, VariableGUI.MinValue);
                    break;
            }
            
            VariableGUI.Index = _variableTotalCounter;
            VariableGUI.IndentLevel = recursionDepth;
            VariableGUI.IsSelected = variable == SelectedVariable;
            VariableGUI.SelectedFitInfo = _selectedFitInfo;
            VariableGUI.PreferRightSelection = !IsLeftSelection;

            void CalcIndicesCount()
            {
                VariableGUI.IndicesCount = 0;
                
                foreach (var index in _indicesToDisplay)
                {
                    if (variable.Values.Count <= index)
                        break;

                    VariableGUI.IndicesCount++;
                }
            }

            void CalcMinMaxLocal()
            {
                VariableGUI.MinValue = double.MaxValue;
                VariableGUI.MaxValue = double.MinValue;

                if (variable.Values.Type is WatchValueType.Bool)
                {
                    VariableGUI.MinValue = 0;
                    VariableGUI.MaxValue = 1;
                    return;
                }
                
                var previousOriginalKeyIndex = -1;

                for (int i = 0; i < VariableGUI.IndicesCount; i++)
                {
                    var key = _indicesToDisplay[i];
                    var keyIndex = variable.Values.GetOriginalKey(key);

                    if (keyIndex != previousOriginalKeyIndex)
                    {
                        previousOriginalKeyIndex = key;

                        var isValid = variable.IsValidNumberValue(key, out var value);

                        if (!isValid)
                            continue;

                        if (value > VariableGUI.MaxValue)
                            VariableGUI.MaxValue = value;

                        if (value < VariableGUI.MinValue)
                            VariableGUI.MinValue = value;
                    }
                }

                
                VariableGUI.MaxValue = Math.Max(VariableGUI.MaxValue, VariableGUI.MinValue);
            }
            
            void CalcMinMaxGlobal()
            {
                VariableGUI.MinValue = double.MaxValue;
                VariableGUI.MaxValue = double.MinValue;

                if (variable.Values.Type is WatchValueType.Bool)
                {
                    VariableGUI.MinValue = 0;
                    VariableGUI.MaxValue = 1;
                    return;
                }
                
                var currentIndexOfOriginalKey = variable.EditorMeta.LastMinMaxCalcIndexOfKey;
                var maxIndexOfOriginalKeys = variable.Values.OriginalKeys.Count - 1;

                if (currentIndexOfOriginalKey >= 0)
                {
                    VariableGUI.MinValue = variable.EditorMeta.GlobalMinValue;
                    VariableGUI.MaxValue = variable.EditorMeta.GlobalMaxValue;
                }
                
                if (currentIndexOfOriginalKey == maxIndexOfOriginalKeys)
                    return;
                
                while (currentIndexOfOriginalKey < maxIndexOfOriginalKeys)
                {
                    currentIndexOfOriginalKey++;

                    var isValid = variable.IsValidNumberValueByIndexOfKey(currentIndexOfOriginalKey, out var value);

                    if (!isValid)
                        continue;
                    
                    if (value > VariableGUI.MaxValue)
                        VariableGUI.MaxValue = value;

                    if (value < VariableGUI.MinValue)
                        VariableGUI.MinValue = value;
                }

                VariableGUI.MaxValue = Math.Max(VariableGUI.MaxValue, VariableGUI.MinValue);
                
                variable.EditorMeta.LastMinMaxCalcIndexOfKey = currentIndexOfOriginalKey;
                variable.EditorMeta.GlobalMinValue = VariableGUI.MinValue;
                variable.EditorMeta.GlobalMaxValue = VariableGUI.MaxValue;
            }
        }
        
        private void OnVariableClicked(WatchVariable variable)
        {
            SelectedVariable = variable;
            _isSelectedTitle = VariableGUI.ClickInfo.IsOverTitleArea;
                    
            if (_isSelectedTitle)
                SelectedColumnIndex = -1;
                    
            if (VariableGUI.ClickInfo.CurrentPositionIndex >= 0)
                SelectedColumnIndex = VariableGUI.ClickInfo.CurrentPositionIndex;

            if (Event.current.type == EventType.MouseDown && !_isSelectedTitle)
                IsMouseDraggingOverValues = true;
                    
            _selectedFitInfo.Reset();
            GUI.changed = true;

            if (_isSelectedTitle && variable.EditorMeta.SearchResult.NameResult.IsPositive)
            {
                WatchEditorServices.SearchEngine.CurrentResultIndex = variable.EditorMeta.SearchResult.NameResult.IndexOfResultInTotalList;
            }
            else if (!_isSelectedTitle && SelectedColumnIndex >= 0 && SelectedColumnIndex < variable.Values.Count)
            {
                var selectedOriginalKey = _isSelectedTitle ? 0 : variable.Values.GetOriginalKey(SelectedColumnIndex);
                    
                if (!_isSelectedTitle
                    && variable.EditorMeta.SearchResult.ValueResults != null
                    && variable.EditorMeta.SearchResult.ValueResults.TryGetValue(selectedOriginalKey, out var searchQueryResult)
                    && searchQueryResult.IsPositive)
                {
                    WatchEditorServices.SearchEngine.CurrentResultIndex = searchQueryResult.IndexOfResultInTotalList;
                }
            }
                   
            if (Event.current.type is not (EventType.Layout or EventType.Repaint))
                Event.current.Use();
        }
        
        private void ProcessSearchResultsJump()
        {
            if (WatchEditorServices.SearchEngine.IsSearchProcessing
                || !WatchEditorServices.SearchEngine.HasCurrentResultIndexChanged)
                return;

            WatchEditorServices.SearchEngine.HasCurrentResultIndexChanged = false;
            
            var currentResult = WatchEditorServices.SearchEngine.CurrentResult;

            var variable = currentResult.Variable;
            while (variable.Parent != null)
            {
                variable = variable.Parent;
                variable.EditorMeta.IsExpanded = true;
            }

            JumpToVariable(currentResult.Variable, currentResult.IsName ? -1 : currentResult.ValueIndex, true, true);
        }

        private void MoveToVariable(int variableOffset, int indexOffset = 0)
        {
            if (_variables.Count == 0)
                return;
            
            if (SelectedVariable == null)
            {
                SelectedVariable = _variables[0];
                _isSelectedTitle = true;
                JumpToSelectedVariable();
                return;
            }
            else if (variableOffset == 0 && indexOffset == 0)
            {
                JumpToSelectedVariable();
                return;
            }

            Variables.GetAllChildRecursive(_variables, WatchFilters.FoldedIfChilds, WatchFilters.None);

            if (indexOffset == 0)
            {
                if (variableOffset < 0)
                {
                    SelectedVariable = GetPrevious();
                    JumpToSelectedVariable();
                }
                else
                {
                    SelectedVariable = GetNext();
                    JumpToSelectedVariable();
                }
                return;
            }
            
            var currentSelectionIndex = _isSelectedTitle ? -1 : SelectedColumnIndex;
            currentSelectionIndex += indexOffset;
            currentSelectionIndex = Mathf.Clamp(currentSelectionIndex, 0, SelectedVariable.Values.Count-1);

            if (currentSelectionIndex < -1)
            {
                SelectedVariable = GetPrevious();
                SelectedColumnIndex = -1;

                if (SelectedVariable.Values.Count > 0)
                    SelectedColumnIndex = SelectedVariable.Values.Count - 1;
                
                _isSelectedTitle = SelectedVariable.Values.Count <= 0;
                
                JumpToSelectedVariable();
            }
            else if (currentSelectionIndex >= SelectedVariable.Values.Count)
            {
                SelectedVariable = GetNext();
                SelectedColumnIndex = -1;
                _isSelectedTitle = true;
                
                JumpToSelectedVariable();
            }
            else
            {
                SelectedColumnIndex = currentSelectionIndex;
                _isSelectedTitle = SelectedColumnIndex < 0;
                
                JumpToSelectedVariable();
            }
            
            void JumpToSelectedVariable()
            {
                JumpToVariable(
                    SelectedVariable, 
                    _isSelectedTitle ? -1 : SelectedColumnIndex, 
                    variableOffset != 0, 
                    indexOffset != 0);
            }
            
            WatchVariable GetPrevious()
            {
                WatchVariable previousVariable = null;
                
                foreach (var variable in _variables)
                {
                    if (variable == SelectedVariable)
                        return previousVariable ?? _variables[^1];

                    previousVariable = variable;
                }

                return null;
            }
            
            WatchVariable GetNext()
            {
                WatchVariable firstVariable = _variables[0];
                WatchVariable currentVariable = null;
                
                foreach (var nextVariable in _variables)
                {
                    if (currentVariable == SelectedVariable)
                        return nextVariable;
                        
                    currentVariable = nextVariable;
                }
                
                return firstVariable;
            }
        }
        
        private void JumpToVariable(WatchVariable variable, int index, bool vertical, bool horizontal)
        {
            _horizontalScrollbar.IsStickingToLast = false;

            if (vertical)
            {
                _verticalScrollbar.EndValue = GetVariablesTotalHeight();
                var indexOfVisibleVariable = _variables.IndexOf(variable);
                _verticalScrollbar.SetNormalizedPosition((float)indexOfVisibleVariable / _variables.Count);
            }

            SelectedVariable = variable;
            SelectedColumnIndex = index;
            
            if (horizontal && index >= 0)
            {
                if (Collapse && !_nonShrinkableColumns.Contains(index))
                {
                    Collapse = false;
                    _horizontalScrollbar.EndValue = VariableGUI.ValueColumnWidth * _valriableValuesMaxCount;
                }

                var valuesCount = Collapse ? _nonShrinkableColumns.Count : _valriableValuesMaxCount;
                var normPos = (float)index / valuesCount;
                _horizontalScrollbar.SetNormalizedPosition(normPos);
            }
        }
        
        private float GetVariablesTotalHeight()
        {
            Variables.GetAllChildRecursive(_variables, WatchFilters.FoldedIfChilds, WatchFilters.None);
            return _variables.Count * ValueRowHeight;
        }
        
        private float GetVariablesMaxWidth()
        {
            if (Collapse)
            {
                return _nonShrinkableColumns.Count * VariableGUI.ValueColumnWidth;
            }
            
            var maxWidth = 0f;

            Variables.GetAllChildRecursive(_variables, WatchFilters.FoldedIfChilds, WatchFilters.NoValues);
            
            foreach (var variable in _variables)
            {
                maxWidth = Mathf.Max(maxWidth, variable.Values.Count * VariableGUI.ValueColumnWidth);
            }

            return maxWidth;
        }
    }
}