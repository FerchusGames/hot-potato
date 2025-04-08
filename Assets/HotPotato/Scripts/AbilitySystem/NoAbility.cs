using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HotPotato.AbilitySystem
{
    public class NoAbility : IAbility
    {
        public async UniTaskVoid Execute()
        {
            Debug.Log("No ability executed"); 
            await UniTask.Delay(1000);
            EventBus<AbilityFinishedEvent>.Raise(new AbilityFinishedEvent());
        }
    }
}