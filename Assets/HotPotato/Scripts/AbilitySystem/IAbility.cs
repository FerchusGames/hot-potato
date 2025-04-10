using Cysharp.Threading.Tasks;

namespace HotPotato.AbilitySystem
{
    public enum AbilityType
    {
        NoAbility,
        SkipAbility,
    }
    
    public interface IAbility
    {
        public AbilityType AbilityType { get; }
        public string Message { get; }
        public UniTaskVoid Execute();
    }
}