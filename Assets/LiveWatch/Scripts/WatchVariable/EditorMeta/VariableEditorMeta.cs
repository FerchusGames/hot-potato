using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public class VariableEditorMeta
    {
        public bool IsExpanded;
        
        [NonSerialized] public int LastStringToNumberValue = -1;
        [NonSerialized] public Dictionary<string, int> StringToNumberValues;
        [NonSerialized] public VariableSearchResultMeta SearchResult;
        [NonSerialized] public int LastNonShrinkableIndexOfKey = -1;

        [NonSerialized] public int LastMinMaxCalcIndexOfKey = -1;
        [NonSerialized] public double GlobalMinValue;
        [NonSerialized] public double GlobalMaxValue;
    }
}