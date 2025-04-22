using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Ingvar.LiveWatch.Editor
{
    [Serializable]
    public class WatchSearchEngine
    {
        public bool IsSearchProcessing { get; private set; }
        public int SearchId { get; private set; }
        public bool HasCurrentResultIndexChanged { get; set; }
        public int CurrentResultIndex
        {
            get => _currentResultIndex;
            set => _currentResultIndex = Mathf.Clamp(value, 0, TotalResultsCount-1);
        }
        public int TotalResultsCount => Results.Count;
        public WatchSearchResultPointer CurrentResult => Results[CurrentResultIndex];
        public float SearchProgress => _totalSearchEntities > 0 ? _completedSearchEntities / (float)_totalSearchEntities : 0;
        public List<WatchSearchResultPointer> Results { get; } = new(1024);

        private int _currentResultIndex;
        private int _completedSearchEntities;
        private int _totalSearchEntities;
        
        private CancellationTokenSource _searchToken;
        private List<WatchVariable> _variables = new(32);
        private List<SearchQueryGroup> _queryGroups = new(2);

        public WatchSearchEngine()
        {
            _searchToken = new CancellationTokenSource();
        }

        public void StartSearch(WatchStorage watches, List<SearchQuery> queries)
        {
            if (IsSearchProcessing)
            {
                StopSearch();
            }

            ClearResults(watches);
            ResetProgress();
            
            _searchToken = new CancellationTokenSource();
            SearchId++;
            Task.Run(() => Searching(queries, _searchToken.Token));
        }

        public void StopSearch()
        {
            if (!IsSearchProcessing)
            {
                return;
            }
            
            _searchToken.Cancel();
        }

        public void ClearResults(WatchStorage watches)
        {
            _currentResultIndex = 0;
            Results.Clear();
            
            watches.GetAllChildRecursive(_variables, WatchFilters.None, WatchFilters.None);
            
            foreach (var watch in _variables)
            {
                watch.EditorMeta.SearchResult.Clear();
            }
        }
        
        public void SetPreviousResult()
        {
            if (IsSearchProcessing || Results.Count == 0)
                return;
            
            HasCurrentResultIndexChanged = true;

            if (CurrentResultIndex == 0)
                CurrentResultIndex = TotalResultsCount - 1;
            else
                CurrentResultIndex--;
        }
        
        public void SetNextResult()
        {
            if (IsSearchProcessing || Results.Count == 0)
                return;
            
            HasCurrentResultIndexChanged = true;

            if (CurrentResultIndex == TotalResultsCount - 1)
                CurrentResultIndex = 0;
            else
                CurrentResultIndex++;
        }
        
        private void ResetProgress()
        {
            _completedSearchEntities = 0;
            _totalSearchEntities = 0;

            foreach (var watch in _variables)
            {
                _totalSearchEntities += watch.Values.OriginalKeys.Count + 1;
            }
        }
        
        private void Searching(List<SearchQuery> queries, CancellationToken token)
        {
            IsSearchProcessing = true;

            SplitQueriesIntoGroups(queries);
            
            foreach (var variable in _variables)
            {
                if (token.IsCancellationRequested)
                {
                    IsSearchProcessing = false;
                    break;
                }

                var anyPositiveOrNeutralName = ProcessVariableNameWithQueryGroups(variable, token);
                ProcessVariableValuesWithQueryGroups(variable, anyPositiveOrNeutralName, token);
                PostProcessVariableNameWithQueryGroups(variable, token);
                PostProcessVariableValuesWithQueryGroups(variable, token);
            }
            
            IsSearchProcessing = false;
            LiveWatchWindow.IsRepaintRequested = true;
        }

        private void SplitQueriesIntoGroups(List<SearchQuery> rawQueries)
        {
            _queryGroups.Clear();

            var currentGroup = new SearchQueryGroup();

            for (var i = 0; i < rawQueries.Count; i++)
            {
                if (i == 0 || rawQueries[i].Connective == QueryConnective.OR)
                {
                    if (i != 0)
                        currentGroup = new SearchQueryGroup();
                    
                    _queryGroups.Add(currentGroup);
                }

                currentGroup.AnyNameQuery |= rawQueries[i].Target == QueryTarget.Name;
                currentGroup.AnyValueQuery |= rawQueries[i].Target == QueryTarget.Value;
                
                currentGroup.Queries.Add(rawQueries[i]);
            }
        }

        private bool ProcessVariableNameWithQueryGroups(WatchVariable variable, CancellationToken token)
        {
            var anyPositiveOrNeutral = false;

            foreach (var queryGroup in _queryGroups)
            {
                if (!queryGroup.AnyNameQuery)
                {
                    anyPositiveOrNeutral = true;
                    continue;
                }

                var queryGroupResult = SearchQueryResult.True();

                foreach (var query in queryGroup.Queries)
                {
                    if (token.IsCancellationRequested)
                    {
                        return false;
                    }

                    if (query.Target != QueryTarget.Name)
                    {
                        continue;
                    }
                    
                    var queryResult = SearchQueryProcessor.CheckString(
                        variable.Name,
                        query.QueryText,
                        query.StringOperator,
                        query.CaseSensitive);

                    if (query.Inverse)
                        queryResult = SearchQueryResult.Inverse(queryResult);
                    
                    queryGroupResult = SearchQueryResult.And(queryGroupResult, queryResult);
                    
                    if (!queryResult.IsPositive)
                        break;
                }

                queryGroup.NameResult = queryGroupResult;
                anyPositiveOrNeutral |= queryGroup.NameResult.IsPositive;
            }

            return anyPositiveOrNeutral;
        }
        
        private int ProcessVariableValuesWithQueryGroups(WatchVariable variable, bool anyPositiveOrNeutralName, CancellationToken token)
        {
            var resultsCount = 0;

            foreach (var queryGroup in _queryGroups)
                queryGroup.ValueResultsCount = 0;
            
            if (!anyPositiveOrNeutralName)
            {
                _completedSearchEntities += variable.Values.OriginalKeys.Count;
                return resultsCount;
            }
            
            foreach (var originalIndex in variable.Values.OriginalKeys)
            {
                var totalResult = SearchQueryResult.False();
                
                foreach (var queryGroup in _queryGroups)
                {
                    if (!queryGroup.AnyValueQuery || (!queryGroup.NameResult.IsPositive && queryGroup.AnyNameQuery))
                    {
                        continue;
                    }
                    
                    var queryGroupResult = SearchQueryResult.True();
                    
                    foreach (var query in queryGroup.Queries)
                    {
                        if (token.IsCancellationRequested)
                        {
                            return 0;
                        }

                        if (query.Target != QueryTarget.Value)
                        {
                            continue;
                        }

                        var queryResult = ProcessVariableValueWitchQuery(variable, originalIndex, query);
                        
                        if (query.Inverse)
                            queryResult = SearchQueryResult.Inverse(queryResult);
                        
                        queryGroupResult = SearchQueryResult.And(queryGroupResult, queryResult);

                        if (!queryResult.IsPositive)
                            break;
                    }

                    totalResult = SearchQueryResult.Or(totalResult, queryGroupResult);

                    if (queryGroupResult.IsPositive)
                        queryGroup.ValueResultsCount++;
                }

                if (!totalResult.IsPositive)
                {
                    continue;
                }

                variable.EditorMeta.SearchResult.ValueResults ??= new Dictionary<int, SearchQueryResult>();
                variable.EditorMeta.SearchResult.ValueResults[originalIndex] = totalResult;
                
                resultsCount++;
            }

            return resultsCount;
        }

        private bool PostProcessVariableNameWithQueryGroups(WatchVariable variable, CancellationToken token)
        {
            var totalResult = SearchQueryResult.False();

            foreach (var queryGroup in _queryGroups)
            {
                if (token.IsCancellationRequested)
                    return false;
                
                if (!queryGroup.AnyNameQuery || queryGroup.AnyValueQuery)
                {
                    continue;
                }
                
                totalResult = SearchQueryResult.Or(totalResult, queryGroup.NameResult);
            }

            if (totalResult.IsPositive && Results.Count < int.MaxValue)
            {
                variable.EditorMeta.SearchResult.NameResult = totalResult;
                variable.EditorMeta.SearchResult.NameResult.IndexOfResultInTotalList = Results.Count;
                Results.Add(WatchSearchResultPointer.FromNameResult(variable));
            }

            _completedSearchEntities++;

            return totalResult.IsPositive;
        }

        private void PostProcessVariableValuesWithQueryGroups(WatchVariable variable, CancellationToken token)
        {
            if (variable.EditorMeta.SearchResult.ValueResults != null)
            {
                foreach (var valueKey in variable.EditorMeta.SearchResult.ValueResults.Keys.ToList())
                {
                    if (token.IsCancellationRequested)
                        return;

                    if (Results.Count < int.MaxValue)
                    {
                        var valueResult = variable.EditorMeta.SearchResult.ValueResults[valueKey];
                        valueResult.IndexOfResultInTotalList = Results.Count;
                        variable.EditorMeta.SearchResult.ValueResults[valueKey] = valueResult;

                        Results.Add(WatchSearchResultPointer.FromValueResult(variable, valueKey));
                    }
                }
            }

            _completedSearchEntities += variable.Values.OriginalKeys.Count;
        }

        private SearchQueryResult ProcessVariableValueWitchQuery(WatchVariable variable, int index, SearchQuery query)
        {
            if (variable.Values.IsEmptyAt(index))
            {
                return SearchQueryResult.False();
            }
            
            return variable.Values.Type switch
            {
                WatchValueType.Float => query.ValuesType is QueryValuesType.Numeric or QueryValuesType.Decimal
                    ? SearchQueryProcessor.CheckDouble(variable.Values.FloatList[index].Value, query.QueryText, query.NumberOperator)
                    : SearchQueryResult.False(),
                
                WatchValueType.Double => query.ValuesType is QueryValuesType.Numeric or QueryValuesType.Decimal
                    ? SearchQueryProcessor.CheckDouble(variable.Values.DoubleList[index].Value, query.QueryText, query.NumberOperator)
                    : SearchQueryResult.False(),
                
                WatchValueType.Int => query.ValuesType is QueryValuesType.Numeric or QueryValuesType.Integer
                    ? SearchQueryProcessor.CheckDouble(variable.Values.IntList[index].Value, query.QueryText, query.NumberOperator)
                    : SearchQueryResult.False(),
                
                WatchValueType.Bool => query.ValuesType is QueryValuesType.Bool
                    ? SearchQueryProcessor.CheckBool(variable.Values.BoolList[index].Value, query.BoolOperator)
                    : SearchQueryResult.False(),
                
                WatchValueType.String => query.ValuesType is QueryValuesType.String
                    ? SearchQueryProcessor.CheckString(variable.Values.StringList[index].Value, query.QueryText, query.StringOperator, query.CaseSensitive)
                    : SearchQueryResult.False(),
                
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private class SearchQueryGroup
        {
            public List<SearchQuery> Queries = new();
            public bool AnyNameQuery;
            public SearchQueryResult NameResult = SearchQueryResult.False();
            public bool AnyValueQuery;
            public int ValueResultsCount;
        }
    }
}