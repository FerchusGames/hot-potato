using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public static class Textures
    {
        public static Texture2D Dots => EditorGUIUtility.isProSkin ? WhiteDots : BlackDots;
        public static Texture2D WhiteDots
        {
            get
            {
                if (whiteDots != null)
                    return whiteDots;
                
                whiteDots = new Texture2D(8, 3);
            
                var e = new Color32(255, 255, 255, 0);
                var w1 = new Color32(200, 200, 200, 255);
                var w2 = new Color32(200, 200, 200, 100);
                var w3 = new Color32(200, 200, 200, 30);
                var w4 = new Color32(200, 200, 200, 20);
                var w5 = new Color32(200, 200, 200, 5);
            
                whiteDots.SetPixels(new Color[]
                {
                    e, w5, e, e, e, e, w5, e,
                    w3, w1, w4, e, e, w3, w1, w4,
                    e, w2, e, e, e, e, w2, e,
                });
                whiteDots.Apply();

                return whiteDots;
            }
        }
        public static Texture2D BlueDots
        {
            get
            {
                if (blueDots != null)
                    return blueDots;
                
                blueDots = new Texture2D(8, 3);
            
                var e = new Color32(255, 255, 255, 0);
                var w1 = new Color32(0, 150, 255, 255);
                var w2 = new Color32(0, 150, 255, 150);
                var w3 = new Color32(0, 150, 255, 60);
                var w4 = new Color32(0, 150, 255, 40);
                var w5 = new Color32(0, 150, 255, 15);
            
                blueDots.SetPixels(new Color[]
                {
                    e, w5, e, e, e, e, w5, e,
                    w3, w1, w4, e, e, w3, w1, w4,
                    e, w2, e, e, e, e, w2, e,
                });
                blueDots.Apply();

                return blueDots;
            }
        }
        public static Texture2D BlackDots
        {
            get
            {
                if (blackDots != null)
                    return blackDots;
                
                blackDots = new Texture2D(8, 3);
            
                var e = new Color32(255, 255, 255, 0);
                var w1 = new Color32(0, 0, 0, 255);
                var w2 = new Color32(0, 0, 0, 150);
                var w3 = new Color32(0, 0, 0, 60);
                var w4 = new Color32(0, 0, 0, 40);
                var w5 = new Color32(0, 0, 0, 15);
            
                blackDots.SetPixels(new Color[]
                {
                    e, w5, e, e, e, e, w5, e,
                    w3, w1, w4, e, e, w3, w1, w4,
                    e, w2, e, e, e, e, w2, e,
                });
                blackDots.Apply();

                return blackDots;
            }
        }
        public static Texture2D WhiteTriangleTopLeft
        {
            get
            {
                if (whiteTriangleTopLeft != null)
                    return whiteTriangleTopLeft;
                
                whiteTriangleTopLeft = new Texture2D(4, 4);
            
                var e = new Color32(255, 255, 255, 0);
                var w1 = new Color32(TriangleColor, TriangleColor, TriangleColor, 255);
                var w2 = new Color32(TriangleColor, TriangleColor, TriangleColor, 150);
                
                whiteTriangleTopLeft.SetPixels(new Color[]
                { 
                    w2, e, e, e, 
                    w1, w2, e, e,
                    w1, w1, w2, e, 
                    w1, w1, w1, w2, 
                });
                whiteTriangleTopLeft.Apply();

                return whiteTriangleTopLeft;
            }
        }
        public static Texture2D WhiteTriangleTopRight
        {
            get
            {
                if (whiteTriangleTopRight != null)
                    return whiteTriangleTopRight;
                
                whiteTriangleTopRight = new Texture2D(4, 4);
            
                var e = new Color32(255, 255, 255, 0);
                var w1 = new Color32(TriangleColor, TriangleColor, TriangleColor, 255);
                var w2 = new Color32(TriangleColor, TriangleColor, TriangleColor, 150);
            
                whiteTriangleTopRight.SetPixels(new Color[]
                { 
                    e, e, e, w2, 
                    e, e, w2, w1,
                    e, w2, w1, w1, 
                    w2, w1, w1, w1, 
                });
                whiteTriangleTopRight.Apply();

                return whiteTriangleTopRight;
            }
        }
        public static Texture2D WhiteTriangleBottomLeft
        {
            get
            {
                if (whiteTriangleBottomLeft != null)
                    return whiteTriangleBottomLeft;
                
                whiteTriangleBottomLeft = new Texture2D(4, 4);
            
                var e = new Color32(255, 255, 255, 0);
                var w1 = new Color32(TriangleColor, TriangleColor, TriangleColor, 255);
                var w2 = new Color32(TriangleColor, TriangleColor, TriangleColor, 150);
                
                whiteTriangleBottomLeft.SetPixels(new Color[]
                { 
                    w1, w1, w1, w2, 
                    w1, w1, w2, e, 
                    w1, w2, e, e,
                    w2, e, e, e, 
                });
                whiteTriangleBottomLeft.Apply();

                return whiteTriangleBottomLeft;
            }
        }
        public static Texture2D WhiteTriangleBottomRight
        {
            get
            {
                if (whiteTriangleBottomRight != null)
                    return whiteTriangleBottomRight;
                
                whiteTriangleBottomRight = new Texture2D(4, 4);
            
                var e = new Color32(255, 255, 255, 0);
                var w1 = new Color32(TriangleColor, TriangleColor, TriangleColor, 255);
                var w2 = new Color32(TriangleColor, TriangleColor, TriangleColor, 150);
            
                whiteTriangleBottomRight.SetPixels(new Color[]
                { 
                    w2, w1, w1, w1,
                    e, w2, w1, w1,  
                    e, e, w2, w1,
                    e, e, e, w2, 
                });
                whiteTriangleBottomRight.Apply();

                return whiteTriangleBottomRight;
            }
        }
        
        private static Texture2D whiteDots;
        private static Texture2D blackDots;
        private static Texture2D blueDots;
        private static Texture2D whiteTriangleTopLeft;
        private static Texture2D whiteTriangleTopRight;
        private static Texture2D whiteTriangleBottomLeft;
        private static Texture2D whiteTriangleBottomRight;
        private const byte TriangleColor = 150;
    }
}