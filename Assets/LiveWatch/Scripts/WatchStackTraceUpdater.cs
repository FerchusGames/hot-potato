namespace Ingvar.LiveWatch
{
    public class WatchStackTraceUpdater
    {
        internal void PushStackTrace(WatchVariable watchVariable, string stackTrace)
        {
            watchVariable.RuntimeMeta.StackTraces ??= new RepetitiveValueList<string>();
            watchVariable.RuntimeMeta.StackTraces.Add(stackTrace);
        }
        
        internal void CatchUpStackTracesWithCount(WatchVariable watchVariable, int count)
        {
            watchVariable.RuntimeMeta.StackTraces ??= new RepetitiveValueList<string>();
            var stackTraces = watchVariable.RuntimeMeta.StackTraces;
            
            if (stackTraces.Count >= count)
                return;

            if (stackTraces.Count == 0)
            {
                stackTraces.Add(string.Empty);
                stackTraces.Expand(count - stackTraces.Count);
                return;
            }

            for (var i = stackTraces.Count; i < count; i++)
            {
                if (!watchVariable.Values.IsOriginalAt(i))
                    stackTraces.Expand(1);
                else
                {
                    stackTraces.Add(string.Empty);
                    stackTraces.Expand(count - i - 1);
                    break;
                }
            }
        }
    }
}