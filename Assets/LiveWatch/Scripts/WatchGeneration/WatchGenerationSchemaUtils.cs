using System;
using System.Collections.Generic;

namespace Ingvar.LiveWatch.Generation
{
    public static class WatchGenerationSchemaUtils
    {
        public static Dictionary<Type, WatchVariableDescriptor> GenerationWatches(WatchGenerationSchema schema)
        {
            return schema.GenerationWatches;
        }
        
        public static Dictionary<Type, WatchVariableDescriptor> DefineWatches(WatchGenerationSchema schema)
        {
            return schema.DefineWatches;
        }
        
        public static Dictionary<Type, WatchVariableDescriptor> DefineWatchesInherited(WatchGenerationSchema schema)
        {
            return schema.DefineWatchesInherited;
        }
    }
}