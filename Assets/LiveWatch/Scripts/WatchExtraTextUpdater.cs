namespace Ingvar.LiveWatch
{
    public class WatchExtraTextUpdater
    {
        public void PushValueExtraText<T>(WatchReference<T> watchReference, string extraText)
        {
            PushValueExtraText(watchReference.WatchVariable, extraText);
        }
        
        internal void PushValueExtraText(WatchVariable watchVariable, string extraText)
        {
            watchVariable.RuntimeMeta.ExtraTexts ??= new RepetitiveValueList<string>();
            watchVariable.RuntimeMeta.ExtraTexts.Add(extraText);
        }
        
        internal void CatchUpExtraTextsWithCount(WatchVariable watchVariable, int count)
        {
            watchVariable.RuntimeMeta.ExtraTexts ??= new RepetitiveValueList<string>();
            var extraTexts = watchVariable.RuntimeMeta.ExtraTexts;
            
            if (extraTexts.Count >= count)
                return;

            if (extraTexts.Count == 0)
            {
                extraTexts.Add(string.Empty);
                extraTexts.Expand(count - extraTexts.Count);
                return;
            }

            for (var i = extraTexts.Count; i < count; i++)
            {
                if (!watchVariable.Values.IsOriginalAt(i))
                    extraTexts.Expand(1);
                else
                {
                    extraTexts.Add(string.Empty);
                    extraTexts.Expand(count - i - 1);
                    break;
                }
            }
        }
    }
}