using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public class VariableRuntimeMeta
    {
        public bool AutoUpdate = true;
        public Type ValueType;
        public Action UpdateCall;
        public bool IsSortingRequired;
        public bool IsSetUp;
        public bool IsDictionaryKey;
        public bool IsDictionaryValue;
        public bool IsCollectionValue;
        public bool IsTraceable;
        public Dictionary<string, Action> CustomActions;

        public bool AlwaysShrinkable;
        public int DecimalPlaces = 2;
        public bool IsOrderSet;
        public float SortOrder;
        public string CreationStackTrace = string.Empty;
        public VariableMinMaxMeta MinMax;
        public VariableFormatMeta Formatting = new();
        public RepetitiveValueList<string> ExtraTexts;
        public RepetitiveValueList<string> StackTraces;
    } 
}