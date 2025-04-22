using System;
using System.Collections.Generic;
using System.Linq;
using LIngvar.LiveWatch.TowerDefenceDemo;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class TowerBuildManager : MonoBehaviour
    {
        public List<TowerBuildSlot> Slots => _slots;

        [SerializeField] private Transform _towersParent;
        [SerializeField] private List<TowerBuildSlot> _slots;
        [SerializeField] private List<TowerPrefab> _towerPrefabs;

        private LevelScene _levelScene;
        
        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();

            var slotId = 0;
            foreach (var slot in _slots)
                slot.Id = slotId++;

            Watch.GetOrAdd<string>("BuildLog")
                .SetSortOrder(TD_WatchSortOrder.TowerBuildLog)
                .SetTraceable();
        }

        private void Start()
        {
            TD_Watches.GetOrAdd("Slots", () => Slots);
        }

        public void DestroyAllTowers()
        {
            foreach (var slot in Slots)
            {
                if (!slot.IsOccupied)
                    continue;

                DestroyTower(slot, false);
            }
        }
        
        public void BuildTower(TowerBuildSlot slot, TowerType type, bool useGold = true)
        {
            var prefab = _towerPrefabs.First(p => p.Type == type).Prefab;
            var towerObj = _levelScene.PoolManager.Get(prefab.gameObject);
            var tower = towerObj.GetComponent<TowerBase>();

            towerObj.transform.SetParent(_towersParent);
            towerObj.transform.position = slot.BuildLocation;

            _levelScene.TowerManager.AddTower(tower);
            slot.Occupy(tower);

            Watch.Push("BuildLog", $"Built {tower.Id} at {slot.Id} slot");
            Watch.PushFormat("BuildLog", new WatchValueFormat(WatchColors.Blue));

            if (useGold)
            {
                var price = _levelScene.EconomyConfig.TowerBuildCosts[type];
                _levelScene.EnergyManager.CurrentEnergy -= price;
                
                Watch.PushExtraText("Energy", $"Built {tower.Id}");
            }
        }

        public void DestroyTower(TowerBuildSlot slot, bool useGold = true)
        {
            var tower = slot.Tower;
            Watch.Push("BuildLog", $"Destroyed {slot.Tower.Id} at {slot.Id}");
            Watch.PushFormat("BuildLog", new WatchValueFormat(WatchColors.Yellow));
            
            _levelScene.TowerManager.RemoveTower(tower);
            _levelScene.PoolManager.Push(tower.gameObject);
            slot.Empty();

            if (useGold)
            {
                var price = _levelScene.EconomyConfig.TowerSellPrices[tower.Type];
                _levelScene.EnergyManager.CurrentEnergy += price;
                
                Watch.PushExtraText("Energy", $"Destroyed {tower.Id}");
            }
        }
        
        [Serializable]
        public class TowerPrefab
        {
            public TowerType Type;
            public TowerBase Prefab;
        }
    }
}