using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class LaserTower : TowerBase
    {
        public override TowerType Type => TowerType.Laser;

        [SerializeField] private float _cooldown = 0.5f;
        [SerializeField] private int _damage = 5;
        [SerializeField] private float _radius = 4f;
        [SerializeField] private Transform _turretPivot;
        [SerializeField] private ParticleSystem _laserEffect;

        private LevelScene _levelScene;
        private bool _isActive;
        private float _lastFireTime;
        private float _lastTargetUpdateTime;
        private Vector2 ourPos => new Vector2(transform.position.x, transform.position.z);
        private MobMain _previousTarget;

        private const float targetFindCooldown = 0.05f;
        private const float rotationSpeed = 5;
        
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

            UpdateRotation();
            
            if (Time.time < _lastFireTime + _cooldown)
                return;
            
            _lastFireTime = Time.time;
            Fire();
        }

        private void UpdateRotation()
        {
            if (Time.time > _lastTargetUpdateTime + targetFindCooldown)
                _previousTarget = FindNearestTarget();
            
            if (_previousTarget == null || !_previousTarget.IsAlive)
                return;

            var targetDirection = _previousTarget.transform.position - _turretPivot.position;
            var directionNew = Vector3.Slerp(_turretPivot.forward, targetDirection, Time.unscaledDeltaTime * rotationSpeed);
            _turretPivot.forward = directionNew;
        }

        private MobMain FindNearestTarget()
        {
            MobMain targetMob = null;
            
            foreach (var mob in _levelScene.MobManager.Mobs.Values)
            {
                var mobPos = new Vector2(mob.transform.position.x, mob.transform.position.z);
                var sqrDist = (mobPos - ourPos).sqrMagnitude;
                
                if (sqrDist > _radius * _radius)
                    continue;

                if (targetMob == null || mob.WaypointMover.TravelledDist > targetMob.WaypointMover.TravelledDist)
                {
                    targetMob = mob;
                }
            }

            return targetMob;
        }
        
        private void Fire()
        {
            var targetMob = FindNearestTarget();

            if (targetMob != null)
            {          
                if (targetMob.Health.CurrentHealth <= _damage)
                    Watch.Push("MobLog", $"{targetMob.Id} killed by {Id}")
                        .SetSortOrder(TD_WatchSortOrder.MobLog)
                        .PushValueFormat(WatchValueFormat.Yellow);
                
                _turretPivot.forward = targetMob.transform.position - _turretPivot.position;
                _laserEffect.transform.LookAt(targetMob.transform.position);
                _laserEffect.Play();
                
                targetMob.Health.TakeDamage(_damage);
            }
        }
    }
}
