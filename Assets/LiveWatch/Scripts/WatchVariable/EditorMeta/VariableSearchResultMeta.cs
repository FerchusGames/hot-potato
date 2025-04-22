using System;
using System.Collections.Generic;

namespace Ingvar.LiveWatch
{
    public struct VariableSearchResultMeta
    {
        public bool IsValueResults => ValueResults is { Count: > 0 };
        public SearchQueryResult NameResult;
        public Dictionary<int, SearchQueryResult> ValueResults;

        public void Clear()
        {
            NameResult.IsPositive = false;
            ValueResults?.Clear();
        }
    }
}