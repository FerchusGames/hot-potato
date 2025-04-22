using System.Collections;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class MobMain : MonoBehaviour
    {
        public string Id { get; private set; }
        public MobType Type => _type;
        public bool IsAlive { get; private set; }
        public int CurrentHealth => Health.CurrentHealth;
        public float SpawnTime { get; private set; }
        public MobWaypointMover WaypointMover => _waypointMover;
        public MobHealth Health => _health;
        
        [SerializeField] private MobType _type;
        [SerializeField] private MobWaypointMover _waypointMover;
        [SerializeField] private MobHealth _health;
        [SerializeField] private GameObject _deathFxPrefab;

        private LevelScene _levelScene;
        
        public void Spawn(string id, LevelScene level)
        {
            Id = id;
            IsAlive = true;
            SpawnTime = Time.time;

            _levelScene = level;
            _waypointMover.StartMoving(level.Waypoints);
            _health.ResetHealth();
        }

        public void Despawn(bool showDeathFx = false, float delay = 0f)
        {
            if (!IsAlive)
                return;
            
            IsAlive = false;
            _waypointMover.StopMoving();
            
            StopAllCoroutines();

            if (delay <= 0f)
                Destroy(showDeathFx);
            else
                StartCoroutine(DelayedDestroying(showDeathFx, delay));
        }

        private IEnumerator DelayedDestroying(bool showDeathFx, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(showDeathFx);
        }
        
        private void Destroy(bool showDeathFx)
        {
            _levelScene.PoolManager.Push(gameObject);
            
            if (showDeathFx)
            {
                var deathFx = _levelScene.PoolManager.Get(_deathFxPrefab);
                deathFx.transform.position = transform.position + Vector3.up;
            }
        }
    }
}