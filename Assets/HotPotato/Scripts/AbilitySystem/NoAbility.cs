using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HotPotato.AbilitySystem
{
    public class NoAbility : IAbility
    {
        public string Message { get; } = "No ability played";

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