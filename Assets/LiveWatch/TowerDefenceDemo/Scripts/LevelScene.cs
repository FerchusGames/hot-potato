using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class LevelScene : MonoBehaviour
    {
        public PoolManager PoolManager;
        public GameSpeedManager SpeedManager;
        public MobManager MobManager;
        public HealthManager HealthManager;
        public WaveManager WaveManager;
        public EnergyManager EnergyManager;
        public TowerManager TowerManager;
        public TowerBuildManager TowerBuildManager;
        public LevelInitialTowerSpawner InitialTowerSpawner;
        public LevelStateManager LevelStateManager;

        public LevelConfig LevelConfig;
        public EconomyConfig EconomyConfig;
        public MobTriggerZone FinishTriggerZone;
        public List<Transform> Waypoints;

        private void Start()
        {
            TD_Watches.GetOrAdd(nameof(LevelConfig), () => LevelConfig).SetSortOrder(TD_WatchSortOrder.LevelConfig);
            TD_Watches.GetOrAdd(nameof(EconomyConfig), () => EconomyConfig).SetSortOrder(TD_WatchSortOrder.EconomyConfig);
        }
    }
}