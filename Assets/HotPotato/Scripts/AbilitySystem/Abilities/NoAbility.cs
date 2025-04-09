using Cysharp.Threading.Tasks;

namespace HotPotato.AbilitySystem.Abilities
{
    public class NoAbility : IAbility
    {
        public AbilityType AbilityType { get; private set; } = AbilityType.NoAbility;
        public string Message { get; } = "No ability played";

        public async UniTaskVoid Execute()
        {
            EventBus<AbilityPlayingEvent>.Raise(new AbilityPlayingEvent
            {
                Ability = this,
            });
            
            await UniTask.Delay(2000);
            EventBus<AbilityFinishedEvent>.Raise(new AbilityFinishedEvent());
        }
    }
}