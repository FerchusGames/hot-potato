using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public class VariableFormatMeta
    {
        public WatchTitleFormat TitleFormat;
        public WatchValueFormat DefaultValueFormat;
        public List<WatchValueConditionalFormat> ConditionalFormats;
        public RepetitiveValueList<WatchValueFormat> ValueFormats;
    }
}