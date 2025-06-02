using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using Unity.Cinemachine;

namespace HotPotato.Bomb
{
    public class ExplosionScreenShake : NetworkBehaviour
    {
        private EventBinding<BombExplodedClientEvent> _bombExplodedBinding;

        [SerializeField] private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        
        [SerializeField] private float _amplitudeGain = 2f;
        [SerializeField] private float _duration = .2f;

        public override void OnStartClient()
        {
            if (!IsOwner) return;
            
            _bombExplodedBinding = new EventBinding<BombExplodedClientEvent>(ShakeScreen);
            EventBus<BombExplodedClientEvent>.Register(_bombExplodedBinding);
        }

        public override void OnStopClient()
        {
            if (!IsOwner) return;
            
            EventBus<BombExplodedClientEvent>.Deregister(_bombExplodedBinding);
        }

        private void ShakeScreen()
        {
            ShakeScreenAsync().Forget();
        }
        
        private async UniTaskVoid ShakeScreenAsync()
        {
            _multiChannelPerlin.AmplitudeGain = _amplitudeGain;
            await UniTask.Delay(TimeSpan.FromSeconds(_duration));
            _multiChannelPerlin.AmplitudeGain = 0f;
        }
    }
}