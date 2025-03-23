using System.Collections.Generic;
using System.Linq;
using HotPotato.Bomb;

namespace HotPotato.Clues
{
    public class ClueData
    {
        public Dictionary<int, int> ModuleTypeData { get; } = new();
        public Dictionary<int, int> ModuleColorData { get; } = new();
        public Dictionary<int, int> ModuleNumberData { get; } = new();
        public Dictionary<int, int> ModuleLetterData { get; } = new();
        
        public ClueData(List<BombModuleSettings> moduleSettings, bool isCountingTraps)
        {
            var filteredModules = 
                moduleSettings.Where(m => m.IsTrap == isCountingTraps);
            
            foreach (var module in filteredModules)
            {
                IncrementCount(ModuleTypeData, module.ModuleTypeIndex);
                IncrementCount(ModuleColorData, module.ColorIndex);
                IncrementCount(ModuleNumberData, module.NumberIndex);
                IncrementCount(ModuleLetterData, module.LetterIndex);
            }
        }

        private void IncrementCount(Dictionary<int, int> dictionary, int key)
        {
            if (!dictionary.TryAdd(key, 1))
                dictionary[key]++;
        }
    }
}
