using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public static class GUIExtensions
    {
        public static void DrawSelectionFrame(Rect rect, int density = 2)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }
            
            Styles.ElementBackground.Draw(rect.CropFromStartToPosition(CropEdge.LeftLocal, density), false, true, true, true);
            Styles.ElementBackground.Draw(rect.CropFromStartToPosition(CropEdge.TopLocal, density), false, true, true, true);
            Styles.ElementBackground.Draw(rect.CropFromStartToPosition(CropEdge.RightLocal, density), false, true, true, true);
            Styles.ElementBackground.Draw(rect.CropFromStartToPosition(CropEdge.BottomLocal, density), false, true, true, true);
        }
        
        public static void DrawColorFrame(Rect rect, Color color, int density = 2)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }
            
            EditorGUI.DrawRect(rect.CropFromStartToPosition(CropEdge.LeftLocal, density), color);
            EditorGUI.DrawRect(rect.CropFromStartToPosition(CropEdge.TopLocal, density), color);
            EditorGUI.DrawRect(rect.CropFromStartToPosition(CropEdge.RightLocal, density), color);
            EditorGUI.DrawRect(rect.CropFromStartToPosition(CropEdge.BottomLocal, density), color);
        }
    }
}