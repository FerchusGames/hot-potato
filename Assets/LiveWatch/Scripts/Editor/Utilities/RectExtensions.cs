using System;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
   public enum CropEdge
   {
      LeftLocal = 1,
      RightLocal = 2,
      TopLocal = 3,
      BottomLocal = 4,
      HorizontalGlobal = 5,
      VerticalGlobal = 6
   }
    
   public enum WidthSetSpace
   {
      Left = 0,
      Center = 1,
      Right = 2
   }
    
   public enum HeightSetSpace
   {
      Top = 0,
      Center = 1,
      Bottom = 2
   }
    
   [Flags]
   public enum ExtrudeFlags
   {
      Left = 1 << 1,
      Right = 1 << 2,
      Top = 1 << 3,
      Bottom = 1 << 4,
      None = 0,
      All = ~None
   }
   
   public static class RectExtensions
   {
      #region Offset

      public static Rect Offset(this Rect rect, float xDelta, float yDelta, float widthDelta, float heightDelta)
      {
         return new Rect(rect.x + xDelta, rect.y + yDelta, rect.width + widthDelta, rect.height + heightDelta);
      }

      public static Rect OffsetByX(this Rect rect, float xDelta)
      {
         return new Rect(rect.x + xDelta, rect.y, rect.width, rect.height);
      }

      public static Rect OffsetByY(this Rect rect, float yDelta)
      {
         return new Rect(rect.x, rect.y + yDelta, rect.width, rect.height);
      }

      public static Rect OffsetByWidth(this Rect rect, float widthDelta)
      {
         return new Rect(rect.x, rect.y, rect.width + widthDelta, rect.height);
      }

      public static Rect OffsetByHeight(this Rect rect, float heightDelta)
      {
         return new Rect(rect.x, rect.y, rect.width, rect.height + heightDelta);
      }

      #endregion

      #region Extrude

      public static Rect Extrude(this Rect rect, ExtrudeFlags flags, float size)
      {
         var xMin = rect.xMin;
         var xMax = rect.xMax;
         var yMin = rect.yMin;
         var yMax = rect.yMax;

         if ((flags & ExtrudeFlags.Left) == ExtrudeFlags.Left)
            xMin -= size;

         if ((flags & ExtrudeFlags.Right) == ExtrudeFlags.Right)
            xMax += size;
         
         if ((flags & ExtrudeFlags.Top) == ExtrudeFlags.Top)
            yMin -= size;
         
         if ((flags & ExtrudeFlags.Bottom) == ExtrudeFlags.Bottom)
            yMax += size;

         return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
      }

      #endregion
      
      #region Set

      public static Rect SetX(this Rect rect, float x)
      {
         return new Rect(x, rect.y, rect.width, rect.height);
      }

      public static Rect SetY(this Rect rect, float y)
      {
         return new Rect(rect.x, y, rect.width, rect.height);
      }

      public static Rect SetWidth(this Rect rect, float width, WidthSetSpace setSpace = WidthSetSpace.Left)
      {
         switch (setSpace)
         {
            case WidthSetSpace.Left:
               return new Rect(rect.x, rect.y, width, rect.height);
            
            case WidthSetSpace.Center:
               return new Rect(rect.center.x - width/2, rect.y, width, rect.height);
            
            case WidthSetSpace.Right:
               return new Rect(rect.xMax - width, rect.y, width, rect.height);
            
            default:
               throw new ArgumentOutOfRangeException(nameof(setSpace), setSpace, null);
         }
      }

      public static Rect SetHeight(this Rect rect, float height, HeightSetSpace setSpace = HeightSetSpace.Top)
      {
         switch (setSpace)
         {
            case HeightSetSpace.Top:
               return new Rect(rect.x, rect.y, rect.width, height);
            
            case HeightSetSpace.Center:
               return new Rect(rect.x, rect.center.y - height / 2, rect.width, height);
            
            case HeightSetSpace.Bottom:
               return new Rect(rect.x, rect.yMax - height, rect.width, height);
                  
            default:
               throw new ArgumentOutOfRangeException(nameof(setSpace), setSpace, null);
         }
      }

      #endregion
      
      #region TakeEdge

      public static Rect CropFromStartToPosition(this Rect rect, CropEdge crop, float position)
      {
         switch (crop)
         {
            case CropEdge.LeftLocal:
               return new Rect(rect.x, rect.y, position, rect.height);

            case CropEdge.RightLocal:
               return new Rect(rect.xMax - position, rect.y, position, rect.height);

            case CropEdge.TopLocal:
               return new Rect(rect.x, rect.y, rect.width, position);

            case CropEdge.BottomLocal:
               return new Rect(rect.x, rect.yMax - position, rect.width, position);

            case CropEdge.HorizontalGlobal:
               return new Rect(rect.x, rect.y, position - rect.x, rect.height);

            case CropEdge.VerticalGlobal:
               return new Rect(rect.x, rect.y, rect.width, position - rect.y);

            default:
               throw new ArgumentOutOfRangeException(nameof(crop), crop, null);
         }
      }

      public static Rect CropFromPositionToEnd(this Rect rect, CropEdge crop, float position)
      {
         switch (crop)
         {
            case CropEdge.LeftLocal:
               return new Rect(rect.x + position, rect.y, rect.width - position, rect.height);

            case CropEdge.RightLocal:
               return new Rect(rect.x, rect.y, rect.width - position, rect.height);

            case CropEdge.TopLocal:
               return new Rect(rect.x, rect.y + position, rect.width, rect.height - position);

            case CropEdge.BottomLocal:
               return new Rect(rect.x, rect.y, rect.width, rect.height - position);

            case CropEdge.HorizontalGlobal:
               return new Rect(position, rect.y, rect.xMax - position, rect.height);

            case CropEdge.VerticalGlobal:
               return new Rect(rect.x, position, rect.width, rect.yMax - position);

            default:
               throw new ArgumentOutOfRangeException(nameof(crop), crop, null);
         }
      }

      public static Rect CropFromPositionWithSize(this Rect rect, CropEdge crop, float position, float size)
      {
         switch (crop)
         {
            case CropEdge.LeftLocal:
               return new Rect(rect.x + position, rect.y, size, rect.height);

            case CropEdge.RightLocal:
               return new Rect(rect.xMax - position - size, rect.y, size, rect.height);

            case CropEdge.TopLocal:
               return new Rect(rect.x, rect.y + position, rect.width, size);

            case CropEdge.BottomLocal:
               return new Rect(rect.x, rect.yMax - position - size, rect.width, size);

            case CropEdge.HorizontalGlobal:
               return new Rect(position, rect.y, size, rect.height);

            case CropEdge.VerticalGlobal:
               return new Rect(rect.x, position, rect.width, size);

            default:
               throw new ArgumentOutOfRangeException(nameof(crop), crop, null);
         }
      }

      public static Rect CropFromPositionToPosition(this Rect rect, CropEdge crop, float positionStart, float positionEnd)
      {
         switch (crop)
         {
            case CropEdge.LeftLocal:
               return new Rect(rect.x + positionStart, rect.y, positionEnd - positionStart, rect.height);

            case CropEdge.RightLocal:
               return new Rect(rect.xMax - positionEnd, rect.y, positionEnd - positionStart, rect.height);

            case CropEdge.TopLocal:
               return new Rect(rect.x, rect.y + positionStart, rect.width, positionEnd - positionStart);

            case CropEdge.BottomLocal:
               return new Rect(rect.x, rect.yMax - positionEnd, rect.width, positionEnd - positionStart);

            case CropEdge.HorizontalGlobal:
               return new Rect(positionStart, rect.y, positionEnd - positionStart, rect.height);

            case CropEdge.VerticalGlobal:
               return new Rect(rect.x, positionStart, rect.width, positionEnd - positionStart);

            default:
               throw new ArgumentOutOfRangeException(nameof(crop), crop, null);
         }
      }


      #endregion

      #region Clamp

      public static float ClampPositionX(this Rect rect, float xPosition)
      {
         return Mathf.Clamp(xPosition, rect.xMin, rect.xMax);
      }

      public static float ClampPositionY(this Rect rect, float yPosition)
      {
         return Mathf.Clamp(yPosition, rect.yMin, rect.yMax);
      }

      public static Vector2 ClampPosition(this Rect rect, Vector2 position)
      {
         return new Vector2(rect.ClampPositionX(position.x), rect.ClampPositionY(position.y));
      }
      
      #endregion

      public static Rect FitInRect(this Rect rect, Rect rectToFitIn)
      {
         return Rect.MinMaxRect(
            Mathf.Max(rect.xMin, rectToFitIn.xMin),
            Mathf.Max(rect.yMin, rectToFitIn.yMin),
            Mathf.Min(rect.xMax, rectToFitIn.xMax),
            Mathf.Min(rect.yMax, rectToFitIn.yMax));
      }

      public static Rect RelativeToOther(this Rect rect, Rect otherRect)
      {
         return new Rect(
            rect.x - otherRect.x, 
            rect.y - otherRect.y,
            rect.width,
            rect.height);
      }

      public static RectInt ToRectInt(this Rect rect)
      {
         return new RectInt(
            Mathf.RoundToInt(rect.x),
            Mathf.RoundToInt(rect.y),
            Mathf.RoundToInt(rect.width),
            Mathf.RoundToInt(rect.height));
      }
   }
}