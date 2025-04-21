using System;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class MobTriggerZone : MonoBehaviour
    {
        public event Action<MobMain> OnMobEntered;

        [SerializeField] private ParticleSystem _attackFx;
        
        private void OnTriggerEnter(Collider other)
        {
            var mob = other.GetComponentInParent<MobMain>();
            
            if (mob == null)
                return;
            
            OnMobEntered?.Invoke(mob);
            _attackFx.Play();
        }
    }
}