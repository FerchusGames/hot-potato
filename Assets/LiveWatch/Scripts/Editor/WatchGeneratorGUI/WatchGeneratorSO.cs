using Ingvar.LiveWatch.Generation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch.Editor
{
    [CreateAssetMenu(menuName = "LiveWatch/Generator", fileName = "WatchGenerator")]
    public class WatchGeneratorSO : ScriptableObject
    {
        public string OutputNamespaceName;
        public string OutputClassName = "MyWatches";
        public TextAsset SchemaClassFile;
        public TextAsset OutputClassFile;
        public ClassSettings OutputClassSettings = new ()
        {
            IsStatic = true,
            ClassModifier = ClassModifier.@public
        };
        public GeneratorSettings GeneratorSettings = new();
        
        public bool AutoRegenOnChange = true;
        public int PreviousOutputHashcode;
    }
}