using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class VariableCellGUI
    {
        public bool IsSelected { get; set; }
        public bool Search { get; set; }
        public int RowIndex { get; set; }
        public int SelectedColumnIndex { get; set; }
        public bool IsNarrowMode { get; set; }
        public float ValueColumnWidth { get; set; }
        public WatchVariable Variable { get; set; }
        public Rect ValuesRect { get; set; }
        public VariableSelectedFitInfo SelectedFitInfo { get; set; }
        public bool PreferRightSelection { get; set; }
        public List<int> IndicesToDisplay { get; set; }

        private VariableSelectedFitInfo _currentFitInfo = new();

        private float currentSideOffset;
        private WatchValueFormat defaultFormat => Variable.RuntimeMeta.Formatting.DefaultValueFormat;
        private RepetitiveValueList<WatchValueFormat> valueFormats => Variable.RuntimeMeta.Formatting.ValueFormats;
        private RepetitiveValueList<string> extraTexts => Variable.RuntimeMeta.ExtraTexts;
        private RepetitiveValueList<string> stackTraces => Variable.RuntimeMeta.StackTraces;
        private bool isRightSelection => PreferRightSelection ? SelectedFitInfo.CanFitRight : !SelectedFitInfo.CanFitLeft;

        public void DrawValueCellSegment(Rect rect, string value, List<int> contentIndices, float progress)
        {
            if (Event.current.type is not EventType.Repaint)
                return;

            currentSideOffset = 0;
            
            if (value == string.Empty)
            {
                DrawFormatCells(rect, contentIndices, Color.clear);
                DrawMetaInfoCells(rect, contentIndices);
                return;
            }

            var isSelected = contentIndices.Contains(SelectedColumnIndex);

            EditorGUI.DrawRect(rect.Extrude(ExtrudeFlags.All, -1f), Colors.SegmentFrame);
            EditorGUI.DrawRect(rect.Extrude(ExtrudeFlags.All, -2f), RowIndex % 2 == 0 ? Colors.Background : Colors.BackgroundOdd);

            var innerRect = rect.Extrude(ExtrudeFlags.All, -4f);
            currentSideOffset = 4;

            var progressPixels = Mathf.FloorToInt(Mathf.Clamp(progress * innerRect.height, 2, innerRect.height));
            var progressRect = innerRect.CropFromPositionWithSize(CropEdge.BottomLocal, 0, progressPixels);
            var progressColor = RowIndex % 2 == 0 ? Colors.SegmentFill : Colors.SegmentFillOdd;

            DrawFormatCells(progressRect, contentIndices, progressColor);
            DrawDividerCells(progressRect.Extrude(ExtrudeFlags.Left | ExtrudeFlags.Right, 4f), contentIndices);
            DrawMetaInfoCells(innerRect, contentIndices);

            if (isSelected)
                return;

            var estimatedSize = Styles.VariableValue.CalcSize(WatchEditorServices.GUICache.GetContent(value));
            estimatedSize.x -= 4;
            var valueRect = innerRect;

            if (estimatedSize.x > valueRect.width)
            {
                var dotsRect = valueRect
                    .CropFromPositionWithSize(CropEdge.RightLocal, 0, 7)
                    .CropFromPositionWithSize(CropEdge.BottomLocal, valueRect.height/2f - 6, 3);
                
                GUI.DrawTexture(dotsRect, Search && HasSearchResult(contentIndices[0], out _) ? Textures.BlueDots : Textures.Dots);
                valueRect = valueRect.Extrude(ExtrudeFlags.Right, -6);
            }

            if (valueRect.width > 5)
            {
                var style = estimatedSize.x > valueRect.width ? Styles.VariableValueLeft : Styles.VariableValue;
                DrawLabelWitchSearchResult(valueRect, Variable.GetValueText(contentIndices[0]), contentIndices[0], style);
            }
        }
        
        public void DrawSelectedCell(Rect rect, string value, int index)
        {
            if (Event.current.type is not (EventType.Repaint or EventType.Layout))
                return;
            
            var availableRect = rect.Extrude(ExtrudeFlags.All, -1);
            var estimatedSize = Styles.VariableValueSelected.CalcSize(WatchEditorServices.GUICache.GetContent(value)) 
                                + Vector2.right * 10;
            var resultWidth = Mathf.Max(availableRect.width, estimatedSize.x);
            
            if (Event.current.type is not EventType.Repaint)
            {
                var cellRectRight = availableRect.CropFromStartToPosition(CropEdge.LeftLocal, resultWidth);
                _currentFitInfo.CanFitRight = cellRectRight.xMax <= ValuesRect.xMax;
                
                var cellRectLeft = availableRect.CropFromStartToPosition(CropEdge.RightLocal, resultWidth);
                _currentFitInfo.CanFitLeft = cellRectLeft.xMin >= ValuesRect.xMin;

                SelectedFitInfo.MergeWith(_currentFitInfo);
                return;
            }

            var edge = PreferRightSelection 
                ? SelectedFitInfo.CanFitRight ? CropEdge.LeftLocal : CropEdge.RightLocal
                : SelectedFitInfo.CanFitLeft ? CropEdge.RightLocal : CropEdge.LeftLocal;

            var distToValuesEdge = edge == CropEdge.LeftLocal
                ? ValuesRect.xMax - availableRect.xMin
                : availableRect.xMax - ValuesRect.xMin;
            
            if (distToValuesEdge > 40)
                resultWidth = Mathf.Min(resultWidth, distToValuesEdge - 5);
            
            var cellBackRect = availableRect.CropFromStartToPosition(edge, resultWidth);
            
            if (IsNarrowMode)
            {
                cellBackRect = cellBackRect.OffsetByX(Mathf.Max(rect.width, Constants.VariableSelectionLineMinWidth) 
                                                      * (edge == CropEdge.LeftLocal ? 1 : - 1));
                cellBackRect = cellBackRect.Extrude(
                    ExtrudeFlags.Top | ExtrudeFlags.Bottom, 
                    -Mathf.Abs(cellBackRect.height - estimatedSize.y)/2f);
            }
            
            EditorGUI.DrawRect(cellBackRect, IsNarrowMode ? Colors.CellBackgroundSelectedGraph : Colors.CellBackgroundSelected);

            DrawPreviewTriangles(cellBackRect);
            DrawSelectedCelLine(rect);
            
            var cellValueRect = cellBackRect;
            var isTextFit = estimatedSize.x < cellValueRect.width;
            var textStyle = isTextFit ? Styles.VariableValueSelected : Styles.VariableValueSelectedLeft;
            cellValueRect = cellValueRect.CropFromPositionToEnd(CropEdge.LeftLocal, isTextFit ? 0 : 5);
            DrawLabelWitchSearchResult(cellValueRect, value, index, textStyle);
        }
        
        public void DrawSelectedCelLine(Rect rect)
        {
            if (Event.current.type is not EventType.Repaint)
                return;

            var edge = PreferRightSelection 
                ? SelectedFitInfo.CanFitRight ? CropEdge.LeftLocal : CropEdge.RightLocal
                : SelectedFitInfo.CanFitLeft ? CropEdge.RightLocal : CropEdge.LeftLocal;
            
            var lineRect = IsNarrowMode 
                ? rect 
                : rect.CropFromPositionWithSize(edge, 1, 3);
            
            if (IsNarrowMode && lineRect.width < Constants.VariableSelectionLineMinWidth)
            {
                lineRect = new Rect(
                    lineRect.center.x - Constants.VariableSelectionLineMinWidth / 2,
                    lineRect.y,
                    Constants.VariableSelectionLineMinWidth,
                    lineRect.height);
            }

            EditorGUI.DrawRect(lineRect, IsNarrowMode ? Colors.CellSelectionLineGraph : Colors.CellSelectionLine);
        }

        private void DrawLabelWitchSearchResult(Rect labelRect, string text, int index, GUIStyle style, bool forceFullSelection = false)
        {
            if (Event.current.type is not EventType.Repaint)
                return;

            if (labelRect.width > ValueColumnWidth)
                labelRect = labelRect.FitInRect(ValuesRect);

            if (Search && HasSearchResult(index, out var searchQueryResult))
            {
                var charStartIndex = searchQueryResult.IsWholeSelection || forceFullSelection ? 0 : searchQueryResult.SelectionStartIndex;
                var charEndIndex = searchQueryResult.IsWholeSelection || forceFullSelection  ? text.Length : searchQueryResult.SelectionEndIndex;
                    
                style.DrawWithTextSelection(
                    labelRect, 
                    WatchEditorServices.GUICache.GetContent(text), 
                    -1, 
                    charStartIndex, 
                    charEndIndex);
            }
            else
            {
                style.Draw(labelRect, WatchEditorServices.GUICache.GetContent(text), -1);
            }
        }

        private void DrawFormatCells(Rect rect, List<int> contentIndices, Color fallbackColor)
        {
            if (valueFormats == null || valueFormats.Count == 0)
            {
                TryDrawFormatRect(rect, defaultFormat);
                return;
            }

            var segmentStartIndex = 0;
            var segmentStartX = GetCellRect(0, rect, contentIndices).xMin;
            
            for (var i = 0; i < contentIndices.Count; i++)
            {
                var cellRect = GetCellRect(i, rect, contentIndices);
                
                if (i != 0 && valueFormats.IsOriginalAt(contentIndices[i]))
                {
                    var segmentFinishX = cellRect.xMin;
                    var segmentRect = new Rect(segmentStartX, rect.y, segmentFinishX - segmentStartX, rect.height);
                    TryDrawFormatRect(segmentRect, valueFormats[contentIndices[segmentStartIndex]]);

                    segmentStartIndex = i;
                    segmentStartX = cellRect.xMin;
                }
            }
            
            if (segmentStartIndex < contentIndices.Count)
                TryDrawFormatRect(new Rect(segmentStartX, rect.y, rect.xMax - segmentStartX, rect.height), valueFormats[contentIndices[segmentStartIndex]]);
            
            void TryDrawFormatRect(Rect formatRect, WatchValueFormat format)
            {
                if (format is { FillColor: { IsSet: true } })
                    EditorGUI.DrawRect(formatRect, format.FillColor.Value);
                else if (defaultFormat is { FillColor: { IsSet: true } })
                    EditorGUI.DrawRect(formatRect, defaultFormat.FillColor.Value);
                else if (fallbackColor != Color.clear)
                    EditorGUI.DrawRect(formatRect, fallbackColor);
            }
        }

        private void DrawDividerCells(Rect rect, List<int> contentIndices)
        {
            WatchEditorServices.CellDividerDrawer.Draw(rect, Mathf.FloorToInt(ValueColumnWidth));
        }
        
        private void DrawMetaInfoCells(Rect rect, List<int> contentIndices)
        {
            if (extraTexts == null || extraTexts.Count == 0 || extraTexts.Count == 1 && string.IsNullOrWhiteSpace(extraTexts[0]))
                return;
            
            if (stackTraces == null || stackTraces.Count == 0 || stackTraces.Count == 1 && string.IsNullOrWhiteSpace(stackTraces[0]))
                return;
            
            var segmentStartIndex = 0;
            var segmentStartX = GetCellRect(0, rect, contentIndices).xMin;
            
            for (var i = 0; i < contentIndices.Count; i++)
            {
                var cellRect = GetCellRect(i, rect, contentIndices);
                
                if (i != 0 && (extraTexts.IsOriginalAt(contentIndices[i]) || stackTraces.IsOriginalAt(contentIndices[i])))
                {
                    var segmentFinishX = cellRect.xMin;
                    var segmentRect = new Rect(segmentStartX, rect.y, segmentFinishX - segmentStartX, rect.height);
                    DrawMetaInfoLine(segmentRect, contentIndices[segmentStartIndex]);

                    segmentStartIndex = i;
                    segmentStartX = cellRect.xMin;
                }
            }
            
            if (segmentStartIndex < contentIndices.Count)
                DrawMetaInfoLine(new Rect(segmentStartX, rect.y, rect.xMax - segmentStartX, rect.height), contentIndices[segmentStartIndex]);
                
            void DrawMetaInfoLine(Rect cellRect, int contentIndex)
            {
                if (string.IsNullOrWhiteSpace(extraTexts[contentIndex]) && string.IsNullOrWhiteSpace(stackTraces[contentIndex]))
                    return;
                
                var bottomLineRect = cellRect
                    .CropFromStartToPosition(CropEdge.BottomLocal, 1);

                EditorGUI.DrawRect(bottomLineRect, Colors.MetaInfoLine);
            }
        }

        private bool HasSearchResult(int index, out SearchQueryResult searchQueryResult)
        {
            searchQueryResult = default;
            
            var originalIndex = Variable.Values.GetOriginalKey(index);
            return Variable.EditorMeta.SearchResult.ValueResults != null
                   && Variable.EditorMeta.SearchResult.ValueResults.TryGetValue(originalIndex, out searchQueryResult)
                   && searchQueryResult.IsPositive;
        }
        
        private Rect GetCellRect(int index, Rect rect, List<int> contentIndices)
        {
            var cellWidth = ValueColumnWidth;
            
            if (index == 0)
                cellWidth -= currentSideOffset;
                
            if (index == contentIndices.Count - 1)
                cellWidth -= currentSideOffset;
                
            var cellRect = rect.CropFromPositionWithSize(
                CropEdge.LeftLocal, 
                index * ValueColumnWidth + (index == 0 ? 0 : -currentSideOffset),
                cellWidth);

            return cellRect;
        }

        private void DrawPreviewTriangles(Rect rect)
        {
            var isSelectedNumberValid = Variable.IsValidNumberValue(SelectedColumnIndex, out var selectedNumberValue);
            
            if (Variable.Values.IsOriginalAt(SelectedColumnIndex))
            {
                var prevToSelectedIndex = IndicesToDisplay.IndexOf(SelectedColumnIndex) - 1;

                if (prevToSelectedIndex >= 0)
                {
                    var isPrevNumberValid = Variable.IsValidNumberValue(IndicesToDisplay[prevToSelectedIndex], out var prevNumberValue);
                    var isTopAngle = isPrevNumberValid && prevNumberValue >= selectedNumberValue;
                    
                    var triangleLeftRect = rect
                        .CropFromPositionWithSize(CropEdge.LeftLocal, IsNarrowMode ? 1 : (isRightSelection ? 3 : 0), 4)
                        .CropFromPositionWithSize(isTopAngle ? CropEdge.TopLocal : CropEdge.BottomLocal, IsSelected && !IsNarrowMode ? 3 : 0, 4);
                    GUI.DrawTexture(triangleLeftRect, isTopAngle ? Textures.WhiteTriangleTopLeft : Textures.WhiteTriangleBottomLeft);
                }
            }

            var nextToSelectedIndex = IndicesToDisplay.IndexOf(SelectedColumnIndex) + 1;
            if (nextToSelectedIndex < IndicesToDisplay.Count && Variable.Values.IsOriginalAt(IndicesToDisplay[nextToSelectedIndex]))
            {
                var isNextNumberValid = Variable.IsValidNumberValue(IndicesToDisplay[nextToSelectedIndex], out var nextNumberValue);
                var isTopAngle = isNextNumberValid && nextNumberValue >= selectedNumberValue;
                
                var triangleRightRect = rect
                    .CropFromPositionWithSize(CropEdge.RightLocal, isRightSelection || IsNarrowMode ? 0 : 3, 4)
                    .CropFromPositionWithSize(isTopAngle ? CropEdge.TopLocal : CropEdge.BottomLocal, IsSelected && !IsNarrowMode ? 3 : 0, 4);
                GUI.DrawTexture(triangleRightRect, isTopAngle ? Textures.WhiteTriangleTopRight : Textures.WhiteTriangleBottomRight);
            }
        }
    }
}