using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class VariableGraphGUI
    {
        public bool Search { get; set; }
        public int RowIndex { get; set; }
        public WatchVariable Variable { get; set; }
        public List<int> IndicesToDisplay { get; set; } = new();
        public int StartIndex { get; set; }
        public int IndicesCount { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public float ValueColumnWidth { get; set; }
        public Color BackgroundColor { get; set; }

        [SerializeField] private TextureDrawGUI _textureDrawGUI = new();
        
        private TextureDrawGUI.GraphPointInfo[] _graphPoints = new TextureDrawGUI.GraphPointInfo[1000];
        private WatchValueFormat defaultFormat => Variable.RuntimeMeta.Formatting.DefaultValueFormat;
        private RepetitiveValueList<WatchValueFormat> valueFormats => Variable.RuntimeMeta.Formatting.ValueFormats;
        private RepetitiveValueList<string> extraTexts => Variable.RuntimeMeta.ExtraTexts;
        
        public void DrawValues(Rect rect)
        {
            var isEmptyVariable = Variable.Values.Count == 0 || Variable.Values.OriginalKeys.Count == 1 && Variable.Values.IsEmptyAt(0);
            
            if (isEmptyVariable && (valueFormats == null || valueFormats.Count == 0) && (extraTexts == null || extraTexts.Count == 0))
            {
                if (defaultFormat is { FillColor: { IsSet: true } })
                    EditorGUI.DrawRect(rect, defaultFormat.FillColor.Value);
                
                return;
            }
            
            var graphRect = isEmptyVariable ? rect : rect.Extrude(ExtrudeFlags.Top | ExtrudeFlags.Bottom, -3);
            var valueColumnWidthInt = Mathf.RoundToInt(ValueColumnWidth);
            var graphPointsCount = 0;
            
            foreach (var index in IndicesToDisplay)
            {
                var noValue = Variable.Values.IsEmptyAt(index);
                
                var valueFormat = valueFormats == null || valueFormats.Count == 0 ? WatchValueFormat.Empty : valueFormats[index];
                var noFormats = !defaultFormat.FillAndGraphColorOverriden && !valueFormat.FillAndGraphColorOverriden;
                var noExtraTexts = extraTexts == null || extraTexts.Count == 0 || !extraTexts.AnyAt(index) || string.IsNullOrWhiteSpace(extraTexts[index]);

                var filColor = Colors.GraphFill;
                var topLineColor = Colors.GraphLine;
                var bottomLineColor = Colors.GraphFill;
                var pixelHeight = Mathf.RoundToInt(graphRect.height);

                if (!noFormats)
                {
                    if (defaultFormat is { GraphLineColor: { IsSet: true } })
                        topLineColor = defaultFormat.GraphLineColor.Value;
                    if (valueFormat is { GraphLineColor: { IsSet: true } })
                        topLineColor = valueFormat.GraphLineColor.Value;

                    if (defaultFormat is { FillColor: { IsSet: true } })
                        filColor = defaultFormat.FillColor.Value;
                    if (valueFormat is { FillColor: { IsSet: true } })
                        filColor = valueFormat.FillColor.Value;
                }

                if (!noValue)
                {
                    var value = Variable.GetValueNumber(index);
                    var normValue = MaxValue - MinValue < 0.000001 ? 1 : (value - MinValue) / (MaxValue - MinValue);
                    pixelHeight = Mathf.RoundToInt(graphRect.height * (float)normValue);
                    var originalIndex = Variable.Values.GetOriginalKey(index);
                    if (Search
                        && Variable.EditorMeta.SearchResult.ValueResults != null
                        && Variable.EditorMeta.SearchResult.ValueResults.ContainsKey(originalIndex))
                    {
                        topLineColor = Colors.GraphLineSearch;
                    }
                }

                if (!noExtraTexts)
                {
                    bottomLineColor = Colors.ExtraTextLineGraph;
                }
                
                for (var i = 0; i < valueColumnWidthInt; i++)
                {
                    var isDivider = ValueColumnWidth > 1 && index != IndicesToDisplay[0] && i == 0;

                    var point = new TextureDrawGUI.GraphPointInfo()
                    {
                        IsEmpty = noValue && noFormats,
                        WithLine = !noValue,
                        PixelHeight = pixelHeight,
                        TopLineColor = topLineColor,
                        BottomLineColor = bottomLineColor,
                        FillColor = isDivider ? filColor + new Color32(10, 10, 10, 10) : filColor
                    };

                    _graphPoints[graphPointsCount++] = point;

                    if (graphPointsCount == _graphPoints.Length)
                        Array.Resize(ref _graphPoints, _graphPoints.Length * 2);
                }
            }

            _textureDrawGUI.Prepare(graphRect);
            _textureDrawGUI.DrawTestGraph(
                BackgroundColor, 
                _graphPoints, 
                graphPointsCount);
            _textureDrawGUI.DrawResult();
        }
        
    }
}