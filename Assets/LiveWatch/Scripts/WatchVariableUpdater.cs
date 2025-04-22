using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Ingvar.LiveWatch
{
    public class WatchVariableUpdater
    {
        public static string ErrorValue = "ERROR";
        public static WatchValueFormat ErrorFormat = new WatchValueFormat(
            new Color32(200, 100, 100, 255), 
            new Color32(200, 100, 100, 255));
        
        public bool AnyPushSinceUpdate { get; set; }
        public bool IsUpdatingNow { get; private set; }
        public int ValuesCount { get; private set; }

        private readonly string ErrorStackTracePrefix = Environment.NewLine + Environment.NewLine + Environment.NewLine;
        
        public void UpdateAll()
        {
            if (!Watch.IsLive)
                return;
            
            Profiler.BeginSample("Watch update all");

            IsUpdatingNow = true;
            
            foreach (var watch in Watch.Watches.Items)
                Update(watch.Value);
            
            foreach (var watch in Watch.Watches.Items)
                PostUpdate(watch.Value);
            
            IsUpdatingNow = false;
            AnyPushSinceUpdate = false;
            
            Profiler.EndSample();
        }

        private void Update(WatchVariable watchVariable)
        {
            if (watchVariable == null)
            {
                return;
            }
            
            if (watchVariable.RuntimeMeta.UpdateCall != null && watchVariable.RuntimeMeta.AutoUpdate)
            {
                try
                {
                    watchVariable.RuntimeMeta.UpdateCall();
                }
                catch (Exception e)
                {
                    if (watchVariable.Values.Type == WatchValueType.String)
                        watchVariable.Values.StringList.Add(new WatchValue<string>(ErrorValue));
                    else 
                        watchVariable.Values.PushEmpty();
                    
                    WatchServices.ExtraTextUpdater.PushValueExtraText(watchVariable, e.Message);
                    WatchServices.StackTraceUpdater.PushStackTrace(watchVariable, ErrorStackTracePrefix + e.StackTrace);
                    WatchServices.ValueFormatUpdater.PushValueFormat(watchVariable, ErrorFormat);
                }
            }
            
            foreach (var child in watchVariable.Childs.Items)
            {
                Update(child.Value);
            }
        }

        private void PostUpdate(WatchVariable watchVariable)
        {
            if (watchVariable == null)
            {
                return;
            }

            CatchUpWithValuesCount(watchVariable, ValuesCount);
            
            foreach (var child in watchVariable.Childs.Items)
            {
                PostUpdate(child.Value);
            }
        }
        
        public void ClearAll()
        {
            foreach (var watch in Watch.Watches.Items)
            {
                Clear(watch.Value, true);
            }

            ValuesCount = 0;
        }

        public void Clear(WatchVariable watchVariable, bool recursive = true)
        {
            watchVariable.Values.Clear();
            
            watchVariable.RuntimeMeta.Formatting.ValueFormats?.Clear();
            watchVariable.RuntimeMeta.ExtraTexts?.Clear();
            
            watchVariable.EditorMeta.SearchResult.Clear();
            watchVariable.EditorMeta.LastNonShrinkableIndexOfKey = -1;
            watchVariable.EditorMeta.LastMinMaxCalcIndexOfKey = -1;
            watchVariable.EditorMeta.LastStringToNumberValue = -1;

            if (recursive && watchVariable.HasChilds)
            {
                foreach (var child in watchVariable.Childs.Items)
                {
                    Clear(child.Value);
                }
            }
        }

        public void UpdateTotalValuesCount(WatchVariable variable)
        {
            ValuesCount = Mathf.Max(ValuesCount, variable.Values.Count);
        }
        
        public void CatchUpWithValuesCount(WatchVariable variable, int valuesCount)
        {
            WatchServices.ValueFormatUpdater.CatchUpFormatValuesWithCount(variable, valuesCount);
            WatchServices.ExtraTextUpdater.CatchUpExtraTextsWithCount(variable, valuesCount);
            WatchServices.StackTraceUpdater.CatchUpStackTracesWithCount(variable, valuesCount);
            
            if (valuesCount <= 0
                || variable.Values.Count >= valuesCount)
                return;
                
            if (variable.Values.Count == 0)
                variable.Values.PushEmpty();
                
            variable.Values.Expand(valuesCount - variable.Values.Count);
        }
    }
}