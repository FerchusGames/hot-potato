using System;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public struct SearchQueryResult
    {
        public bool IsPositive;
        public bool IsWholeSelection;
        public int SelectionStartIndex;
        public int SelectionEndIndex;
        public int IndexOfResultInTotalList;

        public static SearchQueryResult True() => new ()
            { IsPositive = true, IsWholeSelection = true};
        public static SearchQueryResult True(int selectionStart, int selectionEnd) => new ()
            { IsPositive = true, SelectionStartIndex = selectionStart, SelectionEndIndex = selectionEnd};
        public static SearchQueryResult False() => new ()
            { IsPositive = false };

        public static SearchQueryResult And(SearchQueryResult left, SearchQueryResult right)
        {
            if (!left.IsPositive || !right.IsPositive)
            {
                return SearchQueryResult.False();
            }

            if (left.IsWholeSelection && right.IsWholeSelection)
            {
                return SearchQueryResult.True();
            } 
            
            if (!left.IsWholeSelection && right.IsWholeSelection)
            {
                return left;
            }
            
            if (left.IsWholeSelection && !right.IsWholeSelection)
            {
                return right;
            }
            
            if (!left.IsWholeSelection && !right.IsWholeSelection)
            {
                var greater = right.SelectionEndIndex >= left.SelectionEndIndex ? right : left;
                var lesser = greater.Equals(right) ? left : right;

                if (lesser.SelectionEndIndex < greater.SelectionStartIndex)
                {
                    return SearchQueryResult.False();
                }
                else
                {
                    return SearchQueryResult.True(lesser.SelectionEndIndex, greater.SelectionEndIndex);
                }
            }

            return SearchQueryResult.False();
        }

        public static SearchQueryResult Or(SearchQueryResult left, SearchQueryResult right)
        {
            if (!left.IsPositive && !right.IsPositive)
            {
                return SearchQueryResult.False();
            }
            
            if (left.IsWholeSelection || right.IsWholeSelection)
            {
                return SearchQueryResult.True();
            }

            if (left.IsPositive && !right.IsPositive)
            {
                return left;
            }
            
            if (!left.IsPositive && right.IsPositive)
            {
                return right;
            }
            
            var startIndex = Mathf.Min(left.SelectionStartIndex, right.SelectionStartIndex);
            var endIndex = Mathf.Min(left.SelectionEndIndex, right.SelectionEndIndex);

            return SearchQueryResult.True(startIndex, endIndex);
        }

        public static SearchQueryResult Inverse(SearchQueryResult result)
        {
            return result.IsPositive ? SearchQueryResult.False() : SearchQueryResult.True();
        }
    }
}