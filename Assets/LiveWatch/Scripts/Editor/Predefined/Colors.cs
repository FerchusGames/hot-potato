using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    public static class Colors
    {
        public static readonly Color Background;
        public static readonly Color BackgroundOdd;

        public static readonly Color SegmentFrame;
        public static readonly Color SegmentFrameInner;
        public static readonly Color SegmentFill;
        public static readonly Color SegmentFillOdd;
        public static readonly Color GraphFill;
        public static readonly Color GraphFillOdd;
        public static readonly Color GraphLine;
        public static readonly Color GraphLineSearch;
        
        public static readonly Color CellDivider;
        public static readonly Color CellBackgroundSelected;
        public static readonly Color CellBackgroundSelectedGraph;
        public static readonly Color CellSelectionLine;
        public static readonly Color CellSelectionLineGraph;

        public static readonly Color MetaInfoLine;
        public static readonly Color ExtraTextLineGraph;
        
        public static readonly Color32 PreviewCellEmpty;
        public static readonly Color32 PreviewCellHasValue;
        public static readonly Color32 PreviewCellHasValueOdd;
        public static readonly Color32 PreviewCellOriginal;
        public static readonly Color32 PreviewCellOriginalOdd;
        public static readonly Color32 PreviewCellSearched;
        public static readonly Color32 PreviewCellSearchedOdd;
        public static readonly Color32 PreviewCellSearchedOriginal;
        public static readonly Color32 PreviewCellSearchedOriginalOdd;
        public static readonly Color32 PreviewOriginalEdge;
        
        static Colors()
        {
            Background = EditorGUIUtility.isProSkin ? new Color32 (56, 56, 56, 255) : new Color32 (194, 194, 194, 255);
            BackgroundOdd = Background + new Color32(6, 6, 6, 0);

            SegmentFrame = EditorGUIUtility.isProSkin ? new Color32(80, 80, 80, 255) : new Color32(130, 130, 130, 255);
            SegmentFrameInner = EditorGUIUtility.isProSkin ? new Color32(90, 90, 90, 40) : new Color32(140, 140, 140, 255);
            SegmentFill = EditorGUIUtility.isProSkin ? new Color32(70, 70, 70, 255) : new Color32(160, 160, 160, 255);
            SegmentFillOdd = SegmentFill + new Color32(6, 6, 6, 0);
            
            GraphFill = EditorGUIUtility.isProSkin ? new Color32(80, 80, 80, 255) : new Color32(170, 170, 170, 255);
            GraphFillOdd = SegmentFill + new Color32(6, 6, 6, 0);
            GraphLine = EditorGUIUtility.isProSkin ? new Color32(100, 100, 100, 255) : new Color32(140, 140, 140, 255);
            GraphLineSearch = new Color32(0, 100, 150, 255);
            
            CellDivider = new Color32(90, 90, 90, 100);
            CellBackgroundSelected = EditorGUIUtility.isProSkin ? new Color32(100, 100, 100, 200) : new Color32(150, 150, 150, 200);;
            CellBackgroundSelectedGraph = EditorGUIUtility.isProSkin ? new Color32(120, 120, 120, 175) : new Color32(140, 140, 140, 225);;
            CellSelectionLine = new Color32(0, 100, 150, 255);
            CellSelectionLineGraph = new Color32(0, 100, 200, 100);
            
            MetaInfoLine = EditorGUIUtility.isProSkin ? new Color32(130, 130, 130, 255) : new Color32(255, 255, 255, 255);
            ExtraTextLineGraph = EditorGUIUtility.isProSkin ? new Color32(170, 170, 170, 255) : new Color32(255, 255, 255, 255);

            PreviewCellEmpty = new Color32(0, 0, 0, 0);
            PreviewOriginalEdge = new Color32(50, 50, 50, 80);
            PreviewCellHasValue = new Color32(60, 60, 60, 80);
            PreviewCellHasValueOdd = new Color32(70, 70, 70, 80);
            PreviewCellOriginal = new Color32(100, 100, 100, 80);
            PreviewCellOriginalOdd = new Color32(110, 110, 110, 80);
            PreviewCellSearched = new Color32(0, 150, 250, 30);
            PreviewCellSearchedOdd = new Color32(0, 160, 255, 35);
            PreviewCellSearchedOriginal = new Color32(0, 150, 250, 50);
            PreviewCellSearchedOriginalOdd = new Color32(0, 160, 255, 55);
        }
    }
}