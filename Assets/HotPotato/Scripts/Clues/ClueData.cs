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
        
        public Dictionary<int, int> GetModuleTypeData => _moduleTypeData;
        public Dictionary<int, int> GetModuleColorData => _moduleColorData;
        public Dictionary<int, int> GetModuleNumberData => _moduleNumberData;
        public Dictionary<int, int> GetModuleLetterData => _moduleLetterData;
    }
}
