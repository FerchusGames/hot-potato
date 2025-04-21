using System;

namespace Ingvar.LiveWatch.Generation
{
    public enum ClassModifier
    {
        @public,
        @internal,
    }
    
    [Serializable]
    public class ClassSettings
    {
        public bool IsStatic;
        public bool IsPartial;
        public ClassModifier ClassModifier;

        public string ModifierStr => ClassModifier.ToString();
        public string StaticStr => IsStatic ? "static" : string.Empty;
        public string PartialStr => IsPartial ? "partial" : string.Empty;
    }
}