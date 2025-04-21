using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class TowerManager : MonoBehaviour
    {
        private Dictionary<string, TowerBase> _towers = new();
        
        private LevelScene _levelScene;
        private int _spawnedTowersCount;

        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }

        private void Start()
        {
            TD_Watches.GetOrAdd("Towers", () => _towers);
        }

        public void EnabledTowers()
        {
            foreach (var tower in _towers.Values)
            {
                tower.Enable(_levelScene);
            }
        }

        public void DisableTowers()
        {
            foreach (var tower in _towers.Values)
            {
                tower.Disable();
            }
        }

        public void AddTower(TowerBase tower)
        {
            tower.Id = $"{tower.Type}_{++_spawnedTowersCount}";
            _towers.Add(tower.Id, tower);
            
            if (_levelScene.LevelStateManager.CurrentState == LevelStateType.Playing)
                tower.Enable(_levelScene);
        }

        public void RemoveTower(TowerBase tower)
        {
            _towers.Remove(tower.Id);
            
            if (_levelScene.LevelStateManager.CurrentState == LevelStateType.Playing)
                tower.Disable();
        }
    }
}