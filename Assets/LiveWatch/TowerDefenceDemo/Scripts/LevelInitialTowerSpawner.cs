using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class LevelInitialTowerSpawner : MonoBehaviour
    {
        [SerializeField] private List<TowerSpawnSetup> _towers;

        private LevelScene _levelScene;
        
        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }

        public void SpawnAllTowers()
        {
            foreach (var towerSpawn in _towers)
            {
                _levelScene.TowerBuildManager.BuildTower(towerSpawn.Slot, towerSpawn.Type, false);
            }
        }
        
        [Serializable]
        public class TowerSpawnSetup
        {
            public TowerBuildSlot Slot;
            public TowerType Type;
        }
    }
}