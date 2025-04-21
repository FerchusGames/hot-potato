using System;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class ScrollbarGUI
    {
        public bool AllowStickToLast;
        public bool IsHorizontal;
        public Rect ScrollRect;
        public float Size;
        public float StartValue;
        public float EndValue;

        public float ScrollValue;
        public float ScrollValueNormalized => EndValue - Size <= 0 ? 0 : ScrollValue / (EndValue - Size);
        public bool IsStickingToLast { get; set; }
        
        public ScrollbarGUI(bool isHorizontal)
        {
            IsHorizontal = isHorizontal;
            IsStickingToLast = false;
        }

        public void Prepare(Rect scrollRect, float size, float startValue, float endValue)
        {
            ScrollRect = scrollRect;
            Size = size;
            StartValue = startValue;
            EndValue = endValue;
        }

        public void DrawPlaceholder()
        {
            if (Event.current.type == EventType.Repaint)
            {
                var style = new GUIStyle(GUI.skin.horizontalScrollbar);
                style.Draw(ScrollRect, false, false, false, false);
            }
        }
        
        public void Draw()
        {
            if (EndValue <= Size)
            {
                ScrollValue = 0;
                return;
            }

            if (IsStickingToLast)
                ScrollValue = EndValue - Size;
            
            var scrollValueBefore = ScrollValue;
            
            ScrollValue = IsHorizontal
                ? GUI.HorizontalScrollbar(ScrollRect, ScrollValue, Size, StartValue, EndValue)
                : GUI.VerticalScrollbar(ScrollRect, ScrollValue, Size, StartValue, EndValue);
            
            var scrollValueAfter = ScrollValue;

            if (AllowStickToLast && !IsStickingToLast && scrollValueBefore < scrollValueAfter &&
                ScrollValueNormalized >= 0.99f)
            {
                IsStickingToLast = true;
            }
            else if (IsStickingToLast && scrollValueAfter < scrollValueBefore)
            {
                IsStickingToLast = false;
            }
        }

        public void SetNormalizedPosition(float normPos, float normAnchor = 0.5f)
        {
            normPos = Mathf.Clamp01(normPos);
            normAnchor = Mathf.Clamp01(normAnchor);
            
            ScrollValue = Mathf.Lerp(StartValue, EndValue, normPos);
            ScrollValue = Mathf.Clamp(ScrollValue - Size * normAnchor, 0, EndValue - Size);
        }
        
        public void ScrollToValue(float value, float normAnchor = 0.5f)
        {
            ScrollValue = Mathf.Clamp(
                value - Mathf.Clamp01(normAnchor) * Size, 
                0, 
                EndValue - Size);
        }

        public void ResizeRelativeToPointer(float pointerPosition, float resizeFactor)
        {
            var normAnchor = IsHorizontal
                ? Mathf.InverseLerp(ScrollRect.xMin, ScrollRect.xMax, pointerPosition)
                : Mathf.InverseLerp(ScrollRect.yMin, ScrollRect.yMax, pointerPosition);
            
            var newScrollPos = resizeFactor * (ScrollValue + Size * normAnchor);

            EndValue *= resizeFactor;
            
            ScrollToValue(newScrollPos, normAnchor);
        }
    }
}