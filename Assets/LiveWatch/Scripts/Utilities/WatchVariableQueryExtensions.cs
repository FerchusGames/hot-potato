using System;
using System.Collections.Generic;

namespace Ingvar.LiveWatch
{
    [Flags]
    public enum WatchFilters
    {
        None = 0,
        FoldedIfChilds = 1 << 0,
        NoValues = 1 << 1,
        NoChilds = 1 << 2,
    }
    
    public static class WatchVariableQueryExtensions
    {
        public static void GetAllChildRecursive(this WatchStorage storage, List<WatchVariable> variables, WatchFilters breakFilters, WatchFilters continueFilters, bool includeItself = true)
        {
            variables.Clear();
            
            foreach (var variableName in storage.SortedNames)
            {
                FindAllChildsRecursive(storage.Items[variableName], 0, variables, breakFilters, continueFilters, includeItself);
            }
        }
        
        private static void FindAllChildsRecursive(WatchVariable variable, int recursionDepth, List<WatchVariable> results, WatchFilters breakFilters, WatchFilters continueFilters, bool includeItself = true)
        {
            if (includeItself && !SkippedByQuery(variable, continueFilters))
            {
                results.Add(variable);
            }

            if (recursionDepth >= Watch.MaxRecursionDepth 
                || SkippedByQuery(variable, breakFilters))
            {
                return;
            }

            foreach (var childVariableName in variable.Childs.SortedNames)
            {
                FindAllChildsRecursive(variable.Childs.Items[childVariableName], recursionDepth + 1, results, breakFilters, continueFilters);
            }
        }

        private static bool SkippedByQuery(WatchVariable variable, WatchFilters filters)
        {
            if (filters == WatchFilters.None)
            {
                return false;
            }

            if (filters.HasFlag(WatchFilters.FoldedIfChilds) && variable.HasChilds && !variable.EditorMeta.IsExpanded)
            {
                return true;
            }
            
            if (filters.HasFlag(WatchFilters.NoValues) && !variable.HasValues)
            {
                return true;
            }
            
            if (filters.HasFlag(WatchFilters.NoChilds) && !variable.HasChilds)
            {
                return true;
            }

            return false;
        }
    }
}