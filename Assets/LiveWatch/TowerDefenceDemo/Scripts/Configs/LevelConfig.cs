using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class LevelConfig : ScriptableObject
    {
        public int MaxHealth = 20;
        public int StartGold = 100;
        public List<MobWave> Waves = new();
    }
    
    [Serializable]
    public class MobWave
    {
        public List<MobSpawn> Spawns = new();
    }

    [Serializable]
    public class MobSpawn
    {
        public float Delay;
        public int MobCount;
        public MobMain MobPrefab;
        public float SpawnDelayBetween = 0.1f;
    }
}