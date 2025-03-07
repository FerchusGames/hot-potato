using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotPotato.Bomb;

namespace HotPotato.Clues
{
    public class ClueData
    {
        private readonly Dictionary<int, int> _moduleTypeData = new();
        private readonly Dictionary<int, int> _moduleColorData = new();
        private readonly Dictionary<int, int> _moduleNumberData = new();
        private readonly Dictionary<int, int> _moduleLetterData = new();

        public ClueData(List<BombModuleSettings> moduleSettings, bool isCountingTraps)
        {
            var filteredModules = moduleSettings.Where(m => m.IsTrap == isCountingTraps);
            
            foreach (var module in filteredModules)
            {
                IncrementCount(_moduleTypeData, module.ModuleTypeIndex);
                IncrementCount(_moduleColorData, module.ColorIndex);
                IncrementCount(_moduleNumberData, module.NumberIndex);
                IncrementCount(_moduleLetterData, module.LetterIndex);
            }
        }

        private void IncrementCount(Dictionary<int, int> dictionary, int key)
        {
            if (!dictionary.TryAdd(key, 1))
                dictionary[key]++;
        }

        public string GetDebugString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Clue Data Debug Info:");
            AppendDictionaryContents(sb, "Module Type Count", _moduleTypeData);
            AppendDictionaryContents(sb, "Module Color Count", _moduleColorData);
            AppendDictionaryContents(sb, "Module Number Count", _moduleNumberData);
            AppendDictionaryContents(sb, "Module Letter Count", _moduleLetterData);

            return sb.ToString();
        }

        private static void AppendDictionaryContents(StringBuilder sb, string title, Dictionary<int, int> dictionary)
        {
            sb.AppendLine($"- {title}:");
            if (dictionary.Count == 0)
            {
                sb.AppendLine("  (Empty)");
                return;
            }
            foreach (var kvp in dictionary)
            {
                sb.AppendLine($"  Index {kvp.Key}: {kvp.Value}");
            }
        }
        
        public Dictionary<int, int> GetModuleTypeData => _moduleTypeData;
        public Dictionary<int, int> GetModuleColorData => _moduleColorData;
        public Dictionary<int, int> GetModuleNumberData => _moduleNumberData;
        public Dictionary<int, int> GetModuleLetterData => _moduleLetterData;
    }
}
