using System.Collections.Generic;
using System.Linq;
using HotPotato.Bomb;

namespace HotPotato.Clues
{
    public class ClueData
    {
        private int[] _moduleTypeCount = new int[VariationsCount];
        private int[] _moduleColorCount = new int[VariationsCount];
        private int[] _moduleNumberCount = new int[VariationsCount];
        private int[] _moduleLetterCount = new int[VariationsCount];

        private const int VariationsCount = 5; 
        
        public ClueData(List<BombModuleSettings> moduleSettings, bool isCountingTraps)
        {
            foreach (var moduleSetting in moduleSettings.Where(moduleSetting => moduleSetting.IsTrap == isCountingTraps))
            {
                _moduleTypeCount[moduleSetting.ModuleTypeIndex]++;
                _moduleColorCount[moduleSetting.ColorIndex]++;
                _moduleNumberCount[moduleSetting.NumberIndex]++;
                _moduleLetterCount[moduleSetting.LetterIndex]++;
            }
        }
    }
}
