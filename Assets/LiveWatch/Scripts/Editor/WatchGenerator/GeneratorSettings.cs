using System;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch.Generation
{
    [Serializable]
    public class GeneratorSettings
    {
        public bool GenerateCollectionsCount = true;
        public bool GenerateExtensions = false;
        public int MaxGenerationDepth = 10;
        public int MaxPushDepth = 10;
        public int MaxCollectionSize = 100;
        public int MaxDictionarySize = 100;
    }
}