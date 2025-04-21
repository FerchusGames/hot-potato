using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class TextureDrawGUI
    {
        [SerializeField] private Texture2D _mainTexture;

        private int _textureWidth;
        private int _textureHeight;
        private Rect _frameRect;
        private Color[] _colorsArray = new Color[2048 * 2048];
        
        public void Prepare(Rect frameRect)
        {
            _frameRect = frameRect;
            
            _textureWidth = Mathf.CeilToInt(frameRect.width);
            _textureHeight = Mathf.CeilToInt(frameRect.height);
            
            if (_mainTexture == null)
            {
                _mainTexture = new Texture2D(_textureWidth, _textureHeight);
            }
            else if (_mainTexture.width != _textureWidth || _mainTexture.height != _textureHeight)
            {
                _mainTexture.Reinitialize(_textureWidth, _textureHeight);
                _mainTexture.Apply(false);
            }
        }
        
        public void DrawResult()
        {
            _mainTexture.Apply(false);
            GUI.DrawTexture(_frameRect, _mainTexture);
        }

        public void DrawColorAll(Color color)
        {
            FillColorArray(_textureWidth * _textureHeight + 1, color);
            _mainTexture.SetPixels(0, 0, _textureWidth, _textureHeight, _colorsArray);
        }

        public void DrawTestGraph(Color backColor,  GraphPointInfo[] graphPoints, int pointsCount)
        {
            var pixelIndex = 0;
            
            for (var y = 0; y < _textureHeight; y++)
            {
                for (var x = 0; x < _textureWidth; x++)
                {
                    if (x >= pointsCount)
                    {
                        _colorsArray[pixelIndex++] = backColor;
                        continue;
                    }

                    var color = backColor;
                    var point = graphPoints[x];

                    if (point.IsEmpty)
                    {
                        _colorsArray[pixelIndex++] = color;
                        continue;
                    }

                    if (y <= 0)
                    {
                        color = point.BottomLineColor;
                    }
                    else  if (y == point.PixelHeight || y == point.PixelHeight - 1)
                    {
                        if (point.WithLine)
                            color = point.TopLineColor;
                    }
                    else if (y < point.PixelHeight)
                    {
                        color = point.FillColor;
                    }

                    
                    _colorsArray[pixelIndex++] = color;
                }
            }
            
            _mainTexture.SetPixels(_colorsArray);
        }
        
        public void DrawColorRect(Rect rect, Color color)
        {
            var fitRect = rect.FitInRect(_frameRect);
                
            if (fitRect.width <= 0 || fitRect.height <= 0)
                return;
            
            var drawRect = fitRect
                .RelativeToOther(_frameRect)
                .ToRectInt();
            
            FillColorArray(drawRect.width * drawRect.height + 1, color);
            _mainTexture.SetPixels(drawRect.x, drawRect.y, drawRect.width, drawRect.height, _colorsArray);
        }

        private void FillColorArray(int count, Color color)
        {
            if (_colorsArray.Length < count)
                Array.Resize(ref _colorsArray, 2048*2048);

            for (var i = 0; i < count; i++)
                _colorsArray[i] = color;
        }
        
        public struct GraphPointInfo
        {
            public bool IsEmpty;
            public bool WithLine;
            public int PixelHeight;
            public Color TopLineColor;
            public Color BottomLineColor;
            public Color FillColor;
        }
    }
}