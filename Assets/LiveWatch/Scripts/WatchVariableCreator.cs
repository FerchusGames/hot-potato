using System;
using System.Reflection;

namespace Ingvar.LiveWatch
{
    public class WatchVariableCreator
    {
        public WatchVariable CreateEmpty(string name)
        {
            var variable = new WatchVariable()
            {
                Name = name
            };

            return variable;
        }

        public WatchVariable GetOrAdd(WatchStorage storage, string path, bool directChild = true)
        {
            var currentStorage = storage;
            WatchVariable currentVariable = default;
            WatchVariable currentParent = null;

            if (directChild)
            {
                GetOrCreateVariable(path);
                return currentVariable;
            }

            var variableNames = path.Split(Watch.PathSeparator);

            foreach (var variableName in variableNames)
            {
                GetOrCreateVariable(variableName);

                currentStorage = currentVariable.Childs;
                currentParent = currentVariable;
            }

            void GetOrCreateVariable(string variableName)
            {
                var pathExists = currentStorage.TryGet(variableName, out currentVariable);

                if (!pathExists)
                {
                    currentVariable = CreateEmpty(variableName);
                    currentVariable.Parent = currentParent;
                    var catchUpCount = WatchServices.VariableUpdater.ValuesCount + (!WatchServices.VariableUpdater.AnyPushSinceUpdate ? 0 : -1);
                    
                    WatchServices.VariableUpdater.CatchUpWithValuesCount(currentVariable, catchUpCount);
                    currentStorage.Add(currentVariable);
                }
            }
            
            return currentVariable;
        }

        public void PushEmpty(WatchVariable variable, bool withRoot = true, int maxRecursionDepth = 10)
        {
            if (maxRecursionDepth <= 0)
                return;

            if (withRoot)
            {
                variable.Values.PushEmpty();
                WatchServices.VariableUpdater.UpdateTotalValuesCount(variable);
            }

            foreach (var childWatchPair in variable.Childs.Items)
                PushEmpty(childWatchPair.Value, true, maxRecursionDepth - 1);
        }

        public bool IsAlwaysShrinkable(WatchVariable variable)
        {
            if (variable.RuntimeMeta.AlwaysShrinkable)
                return true;

            if (variable.Parent != null && variable.Parent.RuntimeMeta.AlwaysShrinkable)
            {
                variable.RuntimeMeta.AlwaysShrinkable = true;
                return true;
            }

            return false;
        }
    }
}