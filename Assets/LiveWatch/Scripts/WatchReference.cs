using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch
{
    /// <summary>
    /// Type for generic Watch methods when you don't know what type it is, or don't want use type based functionality
    /// </summary>
    public class Any
    {
        
    }
    
    public struct WatchReference<T>
    {
        internal WatchVariable WatchVariable { get; }

        public int ChildCount
        {
            get
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
                return WatchVariable.Childs.Count;
#else  
                return 0;
#endif
            }
        }
        
        internal WatchReference(WatchVariable watchVariable)
        {
            WatchVariable = watchVariable;
        }

        public WatchReference<T> SetAlwaysCollapsable()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD 
            WatchVariable.RuntimeMeta.AlwaysShrinkable = true;
#endif
            return this;
        }

        public WatchReference<T> SetSortOrder(int value)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            if (!WatchVariable.RuntimeMeta.IsOrderSet)
            {
                WatchVariable.RuntimeMeta.SortOrder = value;
                WatchVariable.RuntimeMeta.IsOrderSet = true;
                WatchVariable.RuntimeMeta.IsSortingRequired = true;
            }
#endif
            return this;
        }
        
        public WatchReference<T> SetDecimalPlaces(int value)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.DecimalPlaces = value;
#endif
            return this;
        }
        
        public WatchReference<T> SetTitleFormat(WatchTitleFormat format)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.Formatting.TitleFormat = format;
#endif
            return this;
        }
        
        public WatchReference<T> SetDefaultValueFormat(WatchValueFormat format)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.Formatting.DefaultValueFormat = format;
#endif
            return this;
        }
        
        public WatchReference<T> AddConditionalValueFormat(Func<T, bool> condition, WatchValueFormat format)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.Formatting.ConditionalFormats ??= new List<WatchValueConditionalFormat>();
            WatchVariable.RuntimeMeta.Formatting.ConditionalFormats.Add(new WatchValueConditionalFormat<T>()
            {
                Condition = condition,
                Format = format,
            });
#endif
            return this;
        }
        
        public WatchReference<T> AddConditionalValueFormat(Func<T, bool> condition, Func<T, WatchValueFormat> dynamicFormat)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.Formatting.ConditionalFormats ??= new List<WatchValueConditionalFormat>();
            WatchVariable.RuntimeMeta.Formatting.ConditionalFormats.Add(new WatchValueConditionalFormat<T>()
            {
                Condition = condition,
                DynamicFormat = dynamicFormat,
            });
#endif
            return this;
        }

        public WatchReference<T> PushValueFormat(WatchValueFormat format)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            if (Watch.IsLive)
                WatchServices.ValueFormatUpdater.PushValueFormat(WatchVariable, format);
#endif
            return this;
        }

        public WatchReference<T> PushExtraText(string extraText)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            if (Watch.IsLive)
                WatchServices.ExtraTextUpdater.PushValueExtraText(WatchVariable, extraText);
#endif        
            return this;
        }
        
        public WatchReference<T> PushEmptyValue(bool withRoot = true, int maxRecursionDepth = 10)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            if (Watch.IsLive)
                WatchServices.ReferenceCreator.PushEmpty(this, withRoot, maxRecursionDepth);
#endif        
            return this;
        }
        
        public WatchReference<T> PushStackTrace()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            if (Watch.IsLive)
                WatchServices.StackTraceUpdater.PushStackTrace(WatchVariable, Environment.NewLine + StackTraceUtility.ExtractStackTrace());
#endif     
            return this;
        }
        
        public WatchReference<T> SetTraceable()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.IsTraceable = true;
            
            if (string.IsNullOrEmpty(WatchVariable.RuntimeMeta.CreationStackTrace))
                WatchVariable.RuntimeMeta.CreationStackTrace = StackTraceUtility.ExtractStackTrace();
#endif
            return this;
        }

        public WatchReference<T> SetMinMaxModeAsGlobal()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.MinMax = VariableMinMaxMeta.Global();
#endif
            return this;
        }
        
        public WatchReference<T> SetMinMaxModeAsCustom(double minValue, double maxValue)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.MinMax = VariableMinMaxMeta.Custom(minValue, maxValue);
#endif
            return this;
        }
        
        public WatchReference<T> SetCustomAction(string name, Action action)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            WatchVariable.RuntimeMeta.CustomActions ??= new Dictionary<string, Action>();
            WatchVariable.RuntimeMeta.CustomActions[name] = action;
#endif
            return this;
        }
        
        public WatchReference<V> GetOrAdd<V>(string path)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            return WatchServices.ReferenceCreator.GetOrAdd<V, T>(this, path);
#else
            return WatchServices.ReferenceCreator.Empty<V>();
#endif
        }
        
        public IEnumerable<string> GetChildNames()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD && LIVE_WATCH_BUILD
            return WatchVariable.Childs.SortedNames;
#else
            return Array.Empty<string>();
#endif
        }
    }
}