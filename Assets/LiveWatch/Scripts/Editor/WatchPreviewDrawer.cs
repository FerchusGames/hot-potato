using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ingvar.LiveWatch.Editor
{
    public class WatchPreviewDrawer
    {
        private static int PreviousStartIndex;
        private static float StartIndexChangedTime;
        
        public bool Search { get; set; }
        
        private ConcurrentDictionary<WatchVariable, PreviewInfo> _previewInfos = new();
        private bool canShowSearch => Search
                                      && !WatchEditorServices.SearchEngine.IsSearchProcessing 
                                      && WatchEditorServices.SearchEngine.TotalResultsCount > 0;
        
        public void DrawPreview(Rect rect, Rect drawRect, WatchVariable variable, List<int> indicesToDisplay, int columnWidth)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (PreviousStartIndex != indicesToDisplay[0])
            {
                PreviousStartIndex = indicesToDisplay[0];
                StartIndexChangedTime = Time.realtimeSinceStartup;
            }
            
            if (_previewInfos.TryGetValue(variable, out var previewInfo)
                && IsValidPreview(previewInfo, rect, indicesToDisplay)
                && (previewInfo.Texture != null || previewInfo.State is not PreviewInfo.StateType.ReadyToUse))
            {
                if (previewInfo.State is PreviewInfo.StateType.Computing)
                    return;

                if (previewInfo.State is PreviewInfo.StateType.ReadyToSetPixels)
                {
                    if (previewInfo.MinReadyTime > Time.realtimeSinceStartup)
                        return;
                    
                    PrepareTexture(previewInfo.Size, ref previewInfo.Texture);
                    previewInfo.Texture.SetPixels32(0, 0, previewInfo.Size.x, previewInfo.Size.y, previewInfo.Colors);
                    previewInfo.Texture.Apply(false);
                    previewInfo.State = PreviewInfo.StateType.ReadyToUse;
                }
                
                GUI.DrawTexture(drawRect, previewInfo.Texture);
                
                if (IsValidPreviewSearch(previewInfo))
                    return;
            }

            if (previewInfo == null)
            {
                previewInfo = new PreviewInfo();
                _previewInfos.TryAdd(variable, previewInfo);
            }
            
            previewInfo.Token?.Cancel();
            previewInfo.Token?.Dispose();
            previewInfo.Variable = variable;
            previewInfo.Rect = rect;
            previewInfo.StartIndex = indicesToDisplay[0];
            previewInfo.EndIndex = indicesToDisplay[^1];
            previewInfo.Token = new CancellationTokenSource();
            previewInfo.MinReadyTime = StartIndexChangedTime + 0.2f;
            previewInfo.HasSearchResults = canShowSearch;
            previewInfo.SearchId = WatchEditorServices.SearchEngine.SearchId;
            
            if (IsValidPreview(previewInfo, rect, indicesToDisplay) && !IsValidPreviewSearch(previewInfo))
                previewInfo.State = PreviewInfo.StateType.ComputingSilently;
            else
                previewInfo.State = PreviewInfo.StateType.Computing;

            Task.Factory.StartNew(() => BuildPreviewTexture(previewInfo, indicesToDisplay, columnWidth));
        }

        private void BuildPreviewTexture(PreviewInfo preview, List<int> keysToDisplay, int columnWidth)
        {
            preview.CalcMeta.KeysToDisplay = keysToDisplay.ToList();
            keysToDisplay = preview.CalcMeta.KeysToDisplay;
            
            if (preview.Token.IsCancellationRequested)
                return;

            PrepareColors(preview.Size, ref preview.Colors);
            RefreshPixelInfos(preview);

            if (preview.Token.IsCancellationRequested)
                return;

            var blocksCount = preview.CalcMeta.PixelBlockCount;
            var pixelBlocks = preview.CalcMeta.PixelBlocks;

            for (var k = 0; k < keysToDisplay.Count; k++)
            {
                if (preview.Token.IsCancellationRequested)
                    return;

                var startX = k * columnWidth;
                var endX = startX + columnWidth - 1;

                var currentBlockTopY = preview.Size.y - 1;
                for (var b = 0; b < blocksCount; b++)
                {
                    if (preview.Token.IsCancellationRequested)
                        return;

                    var endY = currentBlockTopY;
                    var startY = endY - pixelBlocks[b].HeightPixels + 1;

                    var mode = ValueModeType.None;
                    GetValueModForChilds(preview.Variable, keysToDisplay[k], pixelBlocks[b].ChildNames, ref mode);

                    if (!preview.HasSearchResults)
                        mode &= ~ValueModeType.Searched;
                    
                    var blockColor = GetColorFromMode(mode, b % 2 == 0);
                    
                    for (var x = startX; x <= endX; x++)
                    {
                        if (preview.Token.IsCancellationRequested)
                            return;

                        for (var y = startY; y <= endY; y++)
                        {
                            if (preview.Token.IsCancellationRequested)
                                return;

                            var pixelIndex = GetPixelIndex(x, y, preview.Size);

                            if ((mode & ValueModeType.HasValue) == ValueModeType.HasValue && x == startX && columnWidth > 1)
                            {
                                preview.Colors[pixelIndex] = Colors.PreviewOriginalEdge;
                            }
                            else
                            {
                                preview.Colors[pixelIndex] = blockColor;
                            }
                        }
                    }

                    currentBlockTopY = startY - 1;
                }
            }

            if (preview.Token.IsCancellationRequested)
                return;
            
            preview.State = PreviewInfo.StateType.ReadyToSetPixels;
            LiveWatchWindow.IsRepaintRequested = true;
        }

        private int GetPixelIndex(int x, int y, Vector2Int textureSize)
        {
            return y * textureSize.x + x;
        }

        private Color32 SumColors(Color32 left, Color32 right)
        {
            return new Color32(
                (byte)Mathf.Clamp(left.r + right.r, 0, 255), 
                (byte)Mathf.Clamp(left.g + right.g, 0, 255), 
                (byte)Mathf.Clamp(left.b + right.b, 0, 255), 
                (byte)Mathf.Clamp(left.a + right.a, 0, 255));
        }
        
        private void RefreshPixelInfos(PreviewInfo preview)
        {
            var cm = preview.CalcMeta;

            var pixelsPerChild = (float)preview.Size.y / preview.Variable.Childs.Count;

            if (pixelsPerChild > 1f)
            {
                cm.PixelBlockCount = preview.Variable.Childs.Count;
                PreparePixelBlocks(cm.PixelBlockCount);
                
                var roundedPixelsPerChild = Mathf.FloorToInt(pixelsPerChild);

                for (var i = 0; i < cm.PixelBlockCount; i++)
                {
                    if (preview.Token.IsCancellationRequested)
                        return;
                    
                    cm.PixelBlocks[i] ??= new PixelBlockInfo();
                    cm.PixelBlocks[i].Clear();
                    cm.PixelBlocks[i].HeightPixels = roundedPixelsPerChild;
                    cm.PixelBlocks[i].ChildNames.Add(preview.Variable.Childs.SortedNames[i]);
                }

                var pixelsLeft = preview.Size.y - roundedPixelsPerChild * preview.Variable.Childs.Count;
                
                var currentBlockIndex = 0;
                while (pixelsLeft > 0)
                {
                    if (preview.Token.IsCancellationRequested)
                        return;
                    
                    pixelsLeft--;
                    cm.PixelBlocks[currentBlockIndex].HeightPixels++;
                    
                    currentBlockIndex++;
                }
            }
            else
            {
                cm.PixelBlockCount = preview.Size.y;
                PreparePixelBlocks(cm.PixelBlockCount);
                
                cm.ChildCountPerBlock ??= new List<int>();
                cm.ChildCountPerBlock.Clear();
                
                var childPerPixel = 1f / pixelsPerChild;
                var roundedChildPerBlock = Mathf.FloorToInt(childPerPixel);
                
                for (var i = 0; i < cm.PixelBlockCount; i++)
                {
                    if (preview.Token.IsCancellationRequested)
                        return;
                    
                    cm.PixelBlocks[i] ??= new PixelBlockInfo();
                    cm.PixelBlocks[i].HeightPixels = 1;
                    cm.ChildCountPerBlock.Add(roundedChildPerBlock);
                }

                var childLeft = preview.Variable.Childs.Count - cm.PixelBlockCount * roundedChildPerBlock;
                var currentBlockIndex = cm.PixelBlockCount - 1;

                while (childLeft > 0)
                {
                    if (preview.Token.IsCancellationRequested)
                        return;
                    
                    childLeft--;
                    cm.ChildCountPerBlock[currentBlockIndex]++;
                    currentBlockIndex--;
                }

                var currentChildIndex = 0;

                for (var i = 0; i < cm.PixelBlockCount; i++)
                {
                    if (preview.Token.IsCancellationRequested)
                        return;
                    
                    cm.PixelBlocks[i].Clear();

                    for (var c = 0; c < cm.ChildCountPerBlock[i]; c++)
                    {
                        if (preview.Token.IsCancellationRequested)
                            return;
                        
                        cm.PixelBlocks[i].ChildNames.Add(preview.Variable.Childs.SortedNames[currentChildIndex++]);
                    }
                }
            }
            
            void PreparePixelBlocks(int count)
            {
                if (cm.PixelBlocks == null)
                    cm.PixelBlocks = new PixelBlockInfo[count];
                else if (cm.PixelBlocks.Length < count)
                    Array.Resize(ref cm.PixelBlocks, count);
            }
        }
        
        private void PrepareColors(Vector2Int size, ref Color32[] colors)
        {
            var targetCount = Mathf.CeilToInt(size.x) * Mathf.CeilToInt(size.y) * 2;

            if (colors == null)
                colors = new Color32[targetCount];
            else if (colors.Length < targetCount)
                Array.Resize(ref colors, targetCount);
            
            for (var i = 0; i < colors.Length; i++)
                colors[i] = new Color32(0, 0, 0, 0);
        }
        
        private void PrepareTexture(Vector2Int size, ref Texture2D texture)
        {
            if (texture == null)
            {
                texture = new Texture2D(size.x, size.y);
            }
            else if (texture.width != size.x || texture.height != size.y)
            {
                texture.Reinitialize(size.x, size.y);
                texture.Apply(false);
            }
        }
        
        private bool IsValidPreview(PreviewInfo preview, Rect rect, List<int> indicesToDisplay)
        {
            return preview.Size.x == Mathf.CeilToInt(rect.width)
                   && preview.Size.y == Mathf.CeilToInt(rect.height)
                   && preview.StartIndex == indicesToDisplay[0]
                   && preview.EndIndex == indicesToDisplay[^1];
        }

        private bool IsValidPreviewSearch(PreviewInfo preview)
        {
            return preview.HasSearchResults == canShowSearch
                   && preview.SearchId == WatchEditorServices.SearchEngine.SearchId;
        }
        
        private Color32 GetColorFromMode(ValueModeType mode, bool isOdd)
        {
            var resultColor = Colors.PreviewCellEmpty;

            if ((mode & ValueModeType.EmptyValue) == ValueModeType.EmptyValue)
                resultColor = Colors.PreviewCellEmpty;

            if ((mode & ValueModeType.HasValue) != ValueModeType.HasValue)
                return resultColor;

            resultColor = isOdd ? Colors.PreviewCellHasValueOdd : Colors.PreviewCellHasValue;

            if ((mode & ValueModeType.Original) == ValueModeType.Original)
                resultColor = isOdd ? Colors.PreviewCellOriginalOdd : Colors.PreviewCellOriginal;

            if ((mode & ValueModeType.Searched) == ValueModeType.Searched)
            {
                resultColor = isOdd
                    ? ((mode & ValueModeType.Original) == ValueModeType.Original) ? Colors.PreviewCellSearchedOriginalOdd : Colors.PreviewCellSearchedOdd
                    : ((mode & ValueModeType.Original) == ValueModeType.Original) ? Colors.PreviewCellSearchedOriginal : Colors.PreviewCellSearched;
            }

            return resultColor;
        }
        
        private void GetValueModForChilds(WatchVariable variable, int key, List<string> childNames, ref ValueModeType mode)
        {
            foreach (var childName in childNames)
            {
                var child = variable.Childs.Get(childName);
                GetValueModeRecursive(child, key, ref mode);
            }
        }
        
        private void GetValueModeRecursive(WatchVariable variable, int key, ref ValueModeType mode)
        {
            if (variable.HasChilds)
            {
                foreach (var childName in variable.Childs.SortedNames)
                {
                    var child = variable.Childs.Get(childName);
                    GetValueModeRecursive(child, key, ref mode);
                }
            }
            else
            {
                if (!variable.Values.AnyAt(key))
                    return;
                
                if (variable.Values.IsEmptyAt(key))
                    mode |= ValueModeType.EmptyValue;
                else 
                    mode |= ValueModeType.HasValue;

                if (variable.Values.IsOriginalAt(key) && key > 0 && !variable.Values.IsEmptyAt(key - 1))
                    mode |= ValueModeType.Original;
                
                if (variable.EditorMeta.SearchResult.ValueResults != null
                    && variable.EditorMeta.SearchResult.ValueResults.ContainsKey(variable.Values.GetOriginalKey(key)))
                    mode |= ValueModeType.Searched;
            }
        }
        
        private class PreviewInfo
        {
            public StateType State;
            public float MinReadyTime;
            public WatchVariable Variable;
            public Rect Rect;
            public int StartIndex;
            public int EndIndex;
            public bool HasSearchResults;
            public int SearchId;
            public Texture2D Texture;
            public Color32[] Colors;
            public CancellationTokenSource Token;
            public PreviewCalcMeta CalcMeta = new();
            public Vector2Int Size => new (Mathf.CeilToInt(Rect.width), Mathf.CeilToInt(Rect.height));

            public enum StateType
            {
                Computing,
                ComputingSilently,
                ReadyToSetPixels,
                ReadyToUse
            }
        }
        
        private class PreviewCalcMeta
        {
            public List<int> KeysToDisplay;
            public List<int> ChildCountPerBlock;
            public int PixelBlockCount;
            public PixelBlockInfo[] PixelBlocks;
        }
        
        private class PixelBlockInfo
        {
            public int HeightPixels;
            public List<string> ChildNames = new();
            public Dictionary<int, ValueModeType> ModePerKeys = new();
            
            public void Clear()
            {
                ChildNames.Clear();
                ModePerKeys.Clear();
            }
        }

        [Flags]
        private enum ValueModeType
        {
            None = 0,
            HasValue = 1 << 0, 
            EmptyValue = 1 << 1,
            Original = 1 << 2,
            Searched = 1 << 3,
            
            OriginalWithValue = HasValue | Original,
        }
    }
}