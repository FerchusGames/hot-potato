using Cysharp.Threading.Tasks;

namespace HotPotato.AbilitySystem.Abilities
{
    public class NoAbility : IAbility
    {
        public AbilityType AbilityType { get; private set; } = AbilityType.NoAbility;
        public string Message { get; } = "Time is ticking";

        public async UniTaskVoid Execute()
        {
            EventBus<AbilityPlayingEvent>.Raise(new AbilityPlayingEvent
            {
                Ability = this,
            });
            
            await UniTask.Delay(1000);
            EventBus<AbilityFinishedEvent>.Raise(new AbilityFinishedEvent());
        }
    }
}