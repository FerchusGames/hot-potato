using Cysharp.Threading.Tasks;

namespace HotPotato.AbilitySystem
{
    public interface IAbility
    {
        public UniTaskVoid Execute();
    }
}