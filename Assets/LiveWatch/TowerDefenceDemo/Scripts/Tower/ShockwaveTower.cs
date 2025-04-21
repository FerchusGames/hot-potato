using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class ShockwaveTower : TowerBase
    {
        public override TowerType Type => TowerType.Shockwave;

        [SerializeField] private float _cooldown = 1f;
        [SerializeField] private int _damage = 20;
        [SerializeField] private float _radius = 3f;
        [SerializeField] private ParticleSystem _waveEffect;

        private LevelScene _levelScene;
        private bool _isActive;
        private float _lastFireTime;
        private Vector2 ourPos => new Vector2(transform.position.x, transform.position.z);
        private List<MobMain> _pendingDamageMobs = new();

        public override void Enable(LevelScene levelScene)
        {
            _isActive = true;
            _lastFireTime = Time.time;
            _levelScene = levelScene;
        }

        public override void Disable()
        {
            _isActive = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _radius);
        }

        private void Update()
        {
            if (!_isActive)
                return;
            
            if (Time.time < _lastFireTime + _cooldown)
                return;
            
            _lastFireTime = Time.time;
            Fire();
        }

        private void Fire()
        {
            _pendingDamageMobs.Clear();
            
            foreach (var mob in _levelScene.MobManager.Mobs.Values)
            {
                var mobPos = new Vector2(mob.transform.position.x, mob.transform.position.z);
                var sqrDist = (mobPos - ourPos).sqrMagnitude;
                
                if (sqrDist > _radius * _radius)
                    continue;
                
                _pendingDamageMobs.Add(mob);
            }

            foreach (var mob in _pendingDamageMobs)
            {
                if (mob.Health.CurrentHealth <= _damage)
                {
                    Watch.Push("MobLog", $"{mob.Id} killed by {Id}")
                        .SetSortOrder(TD_WatchSortOrder.MobLog)
                        .PushValueFormat(WatchValueFormat.Yellow);
                }

                mob.Health.TakeDamage(_damage);
            }
            
            _waveEffect.Play();
        }
    }
}