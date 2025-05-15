using FishNet.Object;
using HotPotato.GameFlow.TurnStateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Bomb
{
    public class BombExploder : NetworkBehaviour
    {
        EventBinding<ModuleExplodedExitStateEvent> _explodedEventBinding;
        
        [Required]
        [SerializeField] private ParticleSystem _explodeParticles;

        public override void OnStartServer()
        {
            _explodedEventBinding = new EventBinding<ModuleExplodedExitStateEvent>(Explode);
            EventBus<ModuleExplodedExitStateEvent>.Register(_explodedEventBinding);
        }

        public override void OnStopServer()
        {
            EventBus<ModuleExplodedExitStateEvent>.Deregister(_explodedEventBinding);
        }

        [Server]
        private void Explode()
        {
            ExplodeClientRpc();
        }
        
        [ObserversRpc]
        private void ExplodeClientRpc()
        {
            _explodeParticles.Play();
        }
    }
}