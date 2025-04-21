using System;
using UnityEditor;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class SearchQueryGUI
    {
        public int Index;
        public bool IsQueryUpdated;
        public bool IsDeleteRequested;
        public SearchQuery CurrentSearchQuery => _currentSearchQuery;

        private Rect _areaRect;
        private float _currentX;
        private SearchbarGUI _searchbar;
        private SearchQuery _currentSearchQuery;
        private SearchQuery _lastSearchQuery;

        private const int TEXTFIELD_OFFSET_Y = 2;
        private const int SPACE_X = 2;
        
        public void OnGUI(Rect areaRect)
        {
            _searchbar ??= new SearchbarGUI();
            _currentSearchQuery ??= new SearchQuery();
            _lastSearchQuery ??= new SearchQuery();

            _areaRect = areaRect;
            _currentX = _areaRect.x;

            //DrawBackground(areaRect);
            DrawConnective();
            DrawInvert();
            DrawTargetToggle();
            DrawValuesType();
            DrawOperator();
            DrawSearchField();
            DrawCaseToggle();
            DrawDelete();

            if (Event.current.type == EventType.Repaint)
            {
                IsQueryUpdated = _currentSearchQuery != _lastSearchQuery;
                _currentSearchQuery.CopyTo(_lastSearchQuery);
            }
        }

        private void DrawBackground(Rect areaRect)
        {
            EditorGUI.DrawRect(areaRect, Index % 2 == 0 ? Colors.Background : Colors.BackgroundOdd);
        }
        
        private void DrawConnective()
        {
            if (Index == 0)
            {
                _currentX += Constants.SearchConnectiveWidth;
                return;
            }

            var rect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchConnectiveWidth);
            
            _currentSearchQuery.Connective = (QueryConnective)EditorGUI.EnumPopup(
                rect, 
                _currentSearchQuery.Connective, 
                EditorStyles.toolbarPopup);

            _currentX += Constants.SearchConnectiveWidth;
        }
        
        private void DrawInvert()
        {
            var rect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchInvertWidth);
            
            _currentSearchQuery.Inverse = GUI.Toggle(
                rect,
                _currentSearchQuery.Inverse, 
                new GUIContent("NOT", "Invert query"), 
                EditorStyles.toolbarButton);

            _currentX += Constants.SearchInvertWidth;
        }
        
        private void DrawTargetToggle()
        {
            var rect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchTargetWidth);
            
            _currentSearchQuery.Target = (QueryTarget)EditorGUI.EnumPopup(
                rect, 
                _currentSearchQuery.Target,
                EditorStyles.toolbarPopup);
            
            _currentX += Constants.SearchTargetWidth;
        }

        private void DrawValuesType()
        {
            var rect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchValueWidth);

            if (_currentSearchQuery.Target == QueryTarget.Name)
            {
                GUI.Label(rect.OffsetByX(1).OffsetByY(1), "String");
                _currentX += Constants.SearchValueWidth;
                return;
            }
            
            _currentSearchQuery.ValuesType = (QueryValuesType)EditorGUI.EnumPopup(
                rect,
                _currentSearchQuery.ValuesType,
                EditorStyles.toolbarPopup);
            
            _currentX += Constants.SearchValueWidth;
        }
        
        private void DrawOperator()
        {
            var rect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchOperatorWidth);
            
            if (_currentSearchQuery.Target == QueryTarget.Name)
            {
                _currentSearchQuery.StringOperator = (QueryStringOperator)EditorGUI.EnumPopup(rect, _currentSearchQuery.StringOperator, EditorStyles.toolbarPopup);
                _currentX += Constants.SearchOperatorWidth + SPACE_X;
                return;
            }

            switch (_currentSearchQuery.ValuesType)
            {
                case QueryValuesType.String:
                    _currentSearchQuery.StringOperator = (QueryStringOperator)EditorGUI.EnumPopup(rect, _currentSearchQuery.StringOperator, EditorStyles.toolbarPopup);
                    break;
                case QueryValuesType.Bool:
                    _currentSearchQuery.BoolOperator = (QueryBoolOperator)EditorGUI.EnumPopup(rect, _currentSearchQuery.BoolOperator, EditorStyles.toolbarPopup);
                    break;
                case QueryValuesType.Decimal:
                case QueryValuesType.Integer:
                case QueryValuesType.Numeric:
                    _currentSearchQuery.NumberOperator = (QueryNumberOperator)EditorGUI.EnumPopup(rect, _currentSearchQuery.NumberOperator, EditorStyles.toolbarPopup);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _currentX += Constants.SearchOperatorWidth + SPACE_X;
        }
        
        private void DrawSearchField()
        {
            var size = Mathf.Max(
                Constants.SearchQueryMinWidth, 
                _areaRect.xMax - _currentX - Constants.SearchCaseWidth - Constants.SearchDeleteWidth - SPACE_X);
            
            if (_currentSearchQuery.Target == QueryTarget.Value && _currentSearchQuery.ValuesType == QueryValuesType.Bool)
            {
                _currentX += size;
                return;
            }
            
            var rect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, size-2);
            
            _searchbar.Rect = rect.OffsetByY(TEXTFIELD_OFFSET_Y);
            _searchbar.Draw();

            _currentSearchQuery.QueryText = _searchbar.QueryText;
            _currentX += size;
        }

        private void DrawCaseToggle()
        {
            if (_currentSearchQuery.Target == QueryTarget.Value && _currentSearchQuery.ValuesType != QueryValuesType.String)
            {
                _currentX += Constants.SearchCaseWidth;
                return;
            }
            
            var rect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchCaseWidth);
            
            _currentSearchQuery.CaseSensitive = GUI.Toggle(
                rect,
                _currentSearchQuery.CaseSensitive, 
                new GUIContent("Case", "Is case sensitive?"), 
                EditorStyles.toolbarButton);
            
            _currentX += Constants.SearchCaseWidth;
        }

        private void DrawDelete()
        {
            if (Index == 0)
            {
                _currentX += Constants.SearchDeleteWidth;
                return;
            }
            
            var rect = _areaRect.CropFromPositionWithSize(CropEdge.LeftLocal, _currentX, Constants.SearchDeleteWidth);
            
            if (GUI.Button(rect, "X", EditorStyles.toolbarButton))
            {
                IsDeleteRequested = true;
            }
        }
    }
}