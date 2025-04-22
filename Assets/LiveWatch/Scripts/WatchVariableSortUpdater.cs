using UnityEngine;

namespace Ingvar.LiveWatch
{
    public class WatchVariableSortUpdater
    {
        public const float InitialOrderStep = 0.0001f;

        public void TrySortByVariable(WatchVariable variable)
        {
            if (!variable.RuntimeMeta.IsSortingRequired)
                return;

            SortWatches(variable.Parent == null ? Watch.Watches : variable.Parent.Childs);
        }
        
        public void SortWatches(WatchStorage storage)
        {
            if (storage.SortedNames.Count <= 1)
                return;

            var previousDefaultOrder = 0f;
            
            for (var i = 0; i < storage.SortedNames.Count - 1; i++)
            {
                for (var j = i + 1; j < storage.SortedNames.Count; j++)
                {
                    if (GetSortOrder(i) > GetSortOrder(j))
                        ReplaceVariables(i, j);
                }
            }

            float GetSortOrder(int index)
            {
                var runtimeMeta = storage.Get(storage.SortedNames[index]).RuntimeMeta;
                runtimeMeta.IsSortingRequired = false;

                if (!runtimeMeta.IsOrderSet && Mathf.Abs(runtimeMeta.SortOrder) < InitialOrderStep / 2f)
                {
                    runtimeMeta.SortOrder = previousDefaultOrder + InitialOrderStep;
                    previousDefaultOrder = runtimeMeta.SortOrder;
                }

                return runtimeMeta.SortOrder;
            }
            
            void ReplaceVariables(int index1, int index2)
            {
                var tempStr = storage.SortedNames[index1];
                storage.SortedNames[index1] = storage.SortedNames[index2];
                storage.SortedNames[index2] = tempStr;
            }
        }
    }
}