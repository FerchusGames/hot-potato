using Cysharp.Threading.Tasks;

namespace HotPotato.AbilitySystem.Abilities
{
    public class SkipAbility : IAbility
    {
        public AbilityType AbilityType { get; private set; } = AbilityType.SkipAbility;
        public string Message { get; } = "Skipping turn...";
        public async UniTaskVoid Execute()
        {
            EventBus<AbilityPlayingEvent>.Raise(new AbilityPlayingEvent
            {
                Ability = this,
            });
            
            await UniTask.Delay(2000);
            
            EventBus<AbilityFinishedEvent>.Raise(new AbilityFinishedEvent
            {
                AbilityType = AbilityType,
            });
        }
    }
}