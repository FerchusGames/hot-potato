using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class MobManager : MonoBehaviour
    {
        public event Action<MobMain> MobDiedByHp;
        public event Action<MobMain> MobReachedFinish;
        public Dictionary<string, MobMain> Mobs { get; } = new();

        private const float mobFinishDestroyDelay = 0.25f;
        private LevelScene _levelScene;
        private int _spawnedMobCount;
        
        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }
        
        private void Start()
        {
            TD_Watches.GetOrAdd("Mobs", () => Mobs).SetTraceable();
        }

        private void OnEnable()
        {
            _levelScene.WaveManager.WaveStarted += OnWaveStarted;
            _levelScene.FinishTriggerZone.OnMobEntered += OnMobTriggeredFinished;
        }

        private void OnDisable()
        {
            _levelScene.WaveManager.WaveStarted -= OnWaveStarted;
            _levelScene.FinishTriggerZone.OnMobEntered -= OnMobTriggeredFinished;
        }

        public void DespawnMobsAll()
        {
            foreach (var mob in Mobs.ToList())
                DespawnMob(mob.Value);
            
            Mobs.Clear();
        }

        private void OnWaveStarted()
        {
            _spawnedMobCount = 0;
        }
        
        private void OnMobTriggeredFinished(MobMain mob)
        {
            MobReachedFinish?.Invoke(mob);
            DespawnMob(mob, false, mobFinishDestroyDelay);
            
            Watch.Push("MobLog", $"{mob.Id} reached base")
                .SetSortOrder(TD_WatchSortOrder.MobLog)
                .PushValueFormat(WatchValueFormat.Magenta);
        }
        
        public MobMain SpawnMob(MobMain prefab)
        {
            var mobObj = _levelScene.PoolManager.Get(prefab.gameObject);
            var mob = mobObj.GetComponent<MobMain>();

            mob.transform.SetParent(transform);
            mob.transform.position = _levelScene.Waypoints[0].position;

            var mobId = $"Mob_{++_spawnedMobCount}";
            mob.Spawn(mobId, _levelScene);
            mob.Health.SetOnDeath(() =>
            {
                MobDiedByHp?.Invoke(mob);
                DespawnMob(mob, true);
            });
            Mobs.Add(mobId, mob);

            Watch.Push("MobLog", $"{mob.Id} spawned")
                .SetSortOrder(TD_WatchSortOrder.MobLog)
                .PushValueFormat(WatchValueFormat.Blue);
            
            return mob;
        }

        public void DespawnMob(MobMain mob, bool deathFx = false, float delay = 0f)
        {
            Mobs.Remove(mob.Id);
            mob.Despawn(deathFx, delay);
        }
    }
}