using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Ingvar.LiveWatch.Editor
{
    public class WatchCellDividerDrawer
    {
        public const int MaxColumnsPerTexture = 10;
        
        private Dictionary<int, BlockPerWidthInfo> _perWidthInfos = new();
        private Color32[] _colors = new Color32[100];
        
        public void Draw(Rect rect, int columnWidth)
        {
            var columnCount = Mathf.RoundToInt(rect.width / columnWidth);
            
            if (columnCount <= 1)
                return;
            
            var columnsLeft = columnCount - 1;
            var currentX = (float)columnWidth;

            while (columnsLeft > 0)
            {
                var columnsToDraw = columnsLeft >= MaxColumnsPerTexture ? MaxColumnsPerTexture : columnsLeft;
                columnsLeft -= columnsToDraw;

                var textureRect = rect.CropFromPositionWithSize(CropEdge.LeftLocal, currentX, columnsToDraw * columnWidth);
                var texture = GetOrCreateTexture(columnWidth, columnsToDraw);
                
                GUI.DrawTexture(textureRect, texture);

                currentX += textureRect.width;
            }
        }

        private Texture2D GetOrCreateTexture(int columnWidth, int columnsCount)
        {
            var hasBlockForWidth = _perWidthInfos.TryGetValue(columnWidth, out var blockPerWidth);

            if (!hasBlockForWidth)
            {
                blockPerWidth = new BlockPerWidthInfo();
                _perWidthInfos[columnWidth] = blockPerWidth;
            }

            var hasTextureForCount = blockPerWidth.TexturesPerCount.TryGetValue(columnsCount, out var texture) && texture != null;

            if (!hasTextureForCount)
            {
                texture = CreateDividerTexture(columnWidth, columnsCount, Colors.CellDivider);
                blockPerWidth.TexturesPerCount[columnsCount] = texture;
            }

            return texture;
        }

        private Texture2D CreateDividerTexture(int columnWidth, int columnsCount, Color color)
        {
            var pixelsCount = columnsCount * columnWidth;
            var texture = new Texture2D(pixelsCount, 1, TextureFormat.RGBA32, false);
            
            PrepareColors(pixelsCount);
            
            for (var c = 0; c < columnsCount; c++)
                _colors[c * columnWidth] = color;
            
            texture.SetPixels32(0, 0, pixelsCount, 1, _colors);
            texture.Apply(false);

            return texture;
        }

        private void PrepareColors(int count)
        {
            if (_colors.Length < count)
                Array.Resize(ref _colors, count);

            for (var i = 0; i < count; i++)
                _colors[i] = Color.clear;
        }
        
        private class BlockPerWidthInfo
        {
            public Dictionary<int, Texture2D> TexturesPerCount = new();
        }
    }
}