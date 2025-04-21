using System;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class ResizerGUI
    {
        public bool IsHorizontal;
        public float HandleSize = 8;
        public float LineSize = 1;
        public Color LineColor = Color.black;

        public Rect LocalArea;

        public float NormPosition;
        public float Position => GetPositionFromNorm(NormPosition);
        public bool IsResizing { get; private set; }
        public ResizerGUI(bool isHorizontal, float handleSize, float lineSize, Color lineColor, float normaPosition = 0)
        {
            IsHorizontal = isHorizontal;
            HandleSize = handleSize;
            LineSize = lineSize;
            LineColor = lineColor;
            NormPosition = normaPosition;
        }
        
        public void ProcessHandle()
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);

            var resizerControlRect = LocalArea.CropFromPositionWithSize(
                IsHorizontal ? CropEdge.VerticalGlobal : CropEdge.HorizontalGlobal, 
                Position - HandleSize / 2, 
                HandleSize);
            
            EditorGUIUtility.AddCursorRect(resizerControlRect, IsHorizontal ? MouseCursor.ResizeVertical : MouseCursor.ResizeHorizontal, controlId);
            
            if (Event.current.type == EventType.MouseDown
                && Event.current.button == 0
                && resizerControlRect.Contains(Event.current.mousePosition))
            {
                IsResizing = true;
                GUIUtility.hotControl = controlId;
                Event.current.Use();
            }

            if (IsResizing && Event.current.type == EventType.Layout)
            {
                var previousNormPos = NormPosition;
                NormPosition = GetNormFromPosition(IsHorizontal ? Event.current.mousePosition.y : Event.current.mousePosition.x);
                
                if (Mathf.Abs(NormPosition - previousNormPos) > 0.0001f)
                    GUI.changed = true;
            }

            if (Event.current.GetTypeForControl(controlId) == EventType.MouseUp)
            {
                IsResizing = false;
            }
        }

        public void DrawLine()
        {
            var resizerLineRect = LocalArea.CropFromPositionWithSize(
            IsHorizontal ? CropEdge.VerticalGlobal : CropEdge.HorizontalGlobal, 
            Position - LineSize / 2, 
            LineSize);
            
            EditorGUI.DrawRect(resizerLineRect, LineColor);
        }

        private float GetPositionFromNorm(float normPosition)
        {
            return IsHorizontal
                ? Mathf.Lerp(LocalArea.yMin, LocalArea.yMax, normPosition)
                : Mathf.Lerp(LocalArea.xMin, LocalArea.xMax, normPosition);
        }

        private float GetNormFromPosition(float position)
        {
            return IsHorizontal
                ? Mathf.InverseLerp(LocalArea.yMin, LocalArea.yMax, position)
                : Mathf.InverseLerp(LocalArea.xMin, LocalArea.xMax, position);
        }
    }
}