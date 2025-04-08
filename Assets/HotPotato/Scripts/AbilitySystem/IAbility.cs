using Cysharp.Threading.Tasks;

namespace HotPotato.AbilitySystem
{
    public interface IAbility
    {
        public string Message { get; }
        public UniTaskVoid Execute();
    }
}