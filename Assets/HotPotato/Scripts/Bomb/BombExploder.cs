using FishNet.Object;
using HotPotato.Audio;
using HotPotato.GameFlow.TurnStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Bomb
{
    public class BombExploder : NetworkBehaviour
    {
        private EventBinding<ModuleExplodedExitStateEvent> _explodedEventBinding;
        private EventBinding<ModuleDefusedExitStateEvent> _defusedEventBinding;

        [Required] [SerializeField] private ParticleSystem _explodeParticles;

        [Required] [SerializeField] private EventReferenceSO _explodeSoundEventReference;

        [Required] [SerializeField] private EventReferenceSO _defuseSoundEventReference;

        public override void OnStartServer()
        {
            _explodedEventBinding = new EventBinding<ModuleExplodedExitStateEvent>(Explode);
            EventBus<ModuleExplodedExitStateEvent>.Register(_explodedEventBinding);
            
            _defusedEventBinding = new EventBinding<ModuleDefusedExitStateEvent>(Defuse);
            EventBus<ModuleDefusedExitStateEvent>.Register(_defusedEventBinding);
        }

        public override void OnStopServer()
        {
            EventBus<ModuleExplodedExitStateEvent>.Deregister(_explodedEventBinding);
            EventBus<ModuleDefusedExitStateEvent>.Deregister(_defusedEventBinding);
        }

        [Server]
        private void Explode()
        {
            ExplodeClientRpc();
        }

        [ObserversRpc]
        private void ExplodeClientRpc()
        {
            EventBus<BombExplodedClientEvent>.Raise(new BombExplodedClientEvent());
            _explodeParticles.Play();
            AudioManager.Instance.PlayOneShot(_explodeSoundEventReference.EventReference, transform.position);
        }

        [Server]
        public void Defuse()
        {
            DefuseClientRpc();
        }

        [ObserversRpc]
        private void DefuseClientRpc()
        {
            AudioManager.Instance.PlayOneShot(_defuseSoundEventReference.EventReference, transform.position);
        }
    }
}