using System;
using System.Collections.Generic;
using System.Linq;

namespace Ingvar.LiveWatch
{
    public class WatchConditionalFormatUpdater
    {
        public void TryUpdateLastValueFormat<T>(WatchReference<T> watchReference, T value)
        {
            var formatting = watchReference.WatchVariable.RuntimeMeta.Formatting;
            var values = watchReference.WatchVariable.Values;
            
            var canCheck = formatting is { ConditionalFormats: { Count: > 0 } } && values.Count > 0 ;
            
            if (!canCheck)
                return;

            for (var i = formatting.ConditionalFormats.Count - 1; i >= 0; i--)
            {
                var result = false;
                var conditionalFormat = (WatchValueConditionalFormat<T>)formatting.ConditionalFormats[i];
                var resultFormat = conditionalFormat.Format;
                
                try
                {
                    result = conditionalFormat.Condition(value);

                    if (result && conditionalFormat.DynamicFormat != null)
                        resultFormat = conditionalFormat.DynamicFormat(value);
                }
                catch (Exception e)
                {
                    watchReference.PushExtraText(e.Message);
                }

                if (result)
                {
                    PushValueFormat(watchReference, resultFormat);
                    break;
                }
            }
        }

        public void PushValueFormat<T>(WatchReference<T> watchReference, WatchValueFormat format)
        {
            PushValueFormat(watchReference.WatchVariable, format);
        }
        
        internal void PushValueFormat(WatchVariable watchVariable, WatchValueFormat format)
        {
            watchVariable.RuntimeMeta.Formatting.ValueFormats ??= new RepetitiveValueList<WatchValueFormat>();
            watchVariable.RuntimeMeta.Formatting.ValueFormats.Add(format);
        }
        
        internal void CatchUpFormatValuesWithCount(WatchVariable watchVariable, int count)
        {
            watchVariable.RuntimeMeta.Formatting.ValueFormats ??= new RepetitiveValueList<WatchValueFormat>();
            var valueFormats = watchVariable.RuntimeMeta.Formatting.ValueFormats;
            
            if (valueFormats.Count >= count)
                return;

            if (valueFormats.Count == 0)
            {
                valueFormats.Add(WatchValueFormat.Empty);
                valueFormats.Expand(count - valueFormats.Count);
                return;
            }

            for (var i = valueFormats.Count; i < count; i++)
            {
                if (!watchVariable.Values.IsOriginalAt(i))
                    valueFormats.Expand(1);
                else
                {
                    valueFormats.Add(WatchValueFormat.Empty);
                    valueFormats.Expand(count - i - 1);
                    break;
                }
            }
        }
    }
}