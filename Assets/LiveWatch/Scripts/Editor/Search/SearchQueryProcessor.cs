using System;

namespace Ingvar.LiveWatch.Editor
{
    public static class SearchQueryProcessor
    {
        public const float CompareTolerance = 0.00001f;
        
        public static SearchQueryResult CheckString(string target, string query, QueryStringOperator queryOperator, bool caseSensitive)
        {
            return queryOperator switch
            {
                QueryStringOperator.Contains => StringContains(target, query, caseSensitive),
                QueryStringOperator.Equals => StringEquals(target, query, caseSensitive),
                _ => throw new ArgumentOutOfRangeException(nameof(queryOperator), queryOperator, null)
            };
        }
        
        public static SearchQueryResult CheckBool(bool target, QueryBoolOperator queryOperator)
        {
            return queryOperator switch
            {
                QueryBoolOperator.True => target ? SearchQueryResult.True() : SearchQueryResult.False(),
                QueryBoolOperator.False => target ? SearchQueryResult.False() : SearchQueryResult.True(),
                _ => throw new ArgumentOutOfRangeException(nameof(queryOperator), queryOperator, null)
            };
        }

        public static SearchQueryResult CheckDouble(double target, string query, QueryNumberOperator queryOperator)
        {
            if (!double.TryParse(query, out var queryValue))
            {
                return SearchQueryResult.False();
            }

            return queryOperator switch
            {
                QueryNumberOperator.Equals => IsEqual(target, queryValue) ? SearchQueryResult.True() : SearchQueryResult.False(),
                QueryNumberOperator.Greater => !IsEqual(target, queryValue) && target > queryValue ? SearchQueryResult.True() : SearchQueryResult.False(),
                QueryNumberOperator.GreaterOrEqual => IsEqual(target, queryValue) || target > queryValue ? SearchQueryResult.True() : SearchQueryResult.False(),
                QueryNumberOperator.Less => !IsEqual(target, queryValue) && target < queryValue ? SearchQueryResult.True() : SearchQueryResult.False(),
                QueryNumberOperator.LessOrEqual => IsEqual(target, queryValue) || target <= queryValue ? SearchQueryResult.True() : SearchQueryResult.False(),
                _ => throw new ArgumentOutOfRangeException(nameof(queryOperator), queryOperator, null)
            };

            bool IsEqual(double a, double b)
            {
                return Math.Abs(a - b) < CompareTolerance;
            }
        }
        
        private static SearchQueryResult StringContains(string stringTarget, string stringQuery, bool caseSensitive)
        {
            if (string.IsNullOrEmpty(stringQuery))
                return SearchQueryResult.False();
            
            var startIndex = stringTarget.IndexOf(stringQuery, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
            
            return startIndex >= 0 
                ? SearchQueryResult.True() 
                : SearchQueryResult.False();
        }

        private static SearchQueryResult StringEquals(string stringTarget, string stringQuery, bool caseSensitive)
        {
            if (string.IsNullOrEmpty(stringQuery))
                return SearchQueryResult.False();
            
            return stringQuery.Equals(stringTarget, caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) 
                ? SearchQueryResult.True() 
                : SearchQueryResult.False();
        }

    }
}