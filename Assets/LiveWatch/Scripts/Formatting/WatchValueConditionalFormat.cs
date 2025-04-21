using System;

namespace Ingvar.LiveWatch
{
    [Serializable]
    public abstract class WatchValueConditionalFormat
    {
        
    }
    
    [Serializable]
    public class WatchValueConditionalFormat<T> : WatchValueConditionalFormat
    {
        public Func<T, bool> Condition;
        public WatchValueFormat Format;
        public Func<T, WatchValueFormat> DynamicFormat;
    }
}