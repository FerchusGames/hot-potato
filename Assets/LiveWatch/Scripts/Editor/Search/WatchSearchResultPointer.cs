namespace Ingvar.LiveWatch.Editor
{
    public struct WatchSearchResultPointer
    {
        public WatchVariable Variable;
        public bool IsName;
        public int ValueIndex;
        
        public static WatchSearchResultPointer FromNameResult(WatchVariable variable)
        {
            return new WatchSearchResultPointer()
            {
                Variable = variable,
                IsName = true
            };
        }
        
        public static WatchSearchResultPointer FromValueResult(WatchVariable variable, int index)
        {
            return new WatchSearchResultPointer()
            {
                Variable = variable,
                ValueIndex = index
            };
        }
    }
}