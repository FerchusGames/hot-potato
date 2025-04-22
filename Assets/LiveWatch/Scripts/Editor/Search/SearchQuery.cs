using System;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class SearchQuery
    {
        public QueryConnective Connective;
        public bool Inverse;
        public QueryTarget Target;
        public QueryValuesType ValuesType;
        public QueryStringOperator StringOperator;
        public QueryBoolOperator BoolOperator;
        public QueryNumberOperator NumberOperator;
        public bool CaseSensitive;
        public string QueryText;

        public SearchQuery()
        {
            Clear();
        }
        
        public void Clear()
        {
            Target = QueryTarget.Name;
            StringOperator = QueryStringOperator.Contains;
            CaseSensitive = false;
            QueryText = string.Empty;
        }

        public void CopyTo(SearchQuery other)
        {
            other.Connective = Connective;
            other.Inverse = Inverse;
            other.Target = Target;
            other.ValuesType = ValuesType;
            other.StringOperator = StringOperator;
            other.BoolOperator = BoolOperator;
            other.NumberOperator = NumberOperator;
            other.CaseSensitive = CaseSensitive;
            other.QueryText = new string(QueryText);
        }
        
        public static bool operator ==(SearchQuery left, SearchQuery right)
        {
            return left.Connective == right.Connective
                   && left.Inverse == right.Inverse
                   && left.Target == right.Target
                   && left.ValuesType == right.ValuesType
                   && left.StringOperator == right.StringOperator
                   && left.BoolOperator == right.BoolOperator
                   && left.NumberOperator == right.NumberOperator
                   && left.CaseSensitive == right.CaseSensitive
                   && left.QueryText == right.QueryText;
        }

        public static bool operator !=(SearchQuery left, SearchQuery right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}