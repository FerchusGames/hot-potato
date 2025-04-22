using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class EconomyConfig : ScriptableObject
    {
        public Dictionary<MobType, int> MobKillRewards
        {
            get
            {
                if (_mobRewardsDict != null)
                    return _mobRewardsDict;

                _mobRewardsDict = new Dictionary<MobType, int>();
                foreach (var mobKillReward in _mobKillRewards)
                {
                    _mobRewardsDict.Add(mobKillReward.Type, mobKillReward.GoldReward);
                }

                return _mobRewardsDict;
            }
        }
        
        public Dictionary<TowerType, int> TowerBuildCosts
        {
            get
            {
                if (_towerCostsDict != null)
                    return _towerCostsDict;

                _towerCostsDict = new Dictionary<TowerType, int>();
                foreach (var towerBuildCost in _towerCosts)
                {
                    _towerCostsDict.Add(towerBuildCost.Type, towerBuildCost.GoldCost);
                }

                return _towerCostsDict;
            }
        }
        
        public Dictionary<TowerType, int> TowerSellPrices
        {
            get
            {
                if (_towerSellPricesDict != null)
                    return _towerSellPricesDict;

                _towerSellPricesDict = new Dictionary<TowerType, int>();
                foreach (var towerBuildCost in _towerCosts)
                {
                    _towerSellPricesDict.Add(towerBuildCost.Type, towerBuildCost.GoldSellPrice);
                }

                return _towerSellPricesDict;
            }
        }

        [SerializeField] private MobRewardSetup[] _mobKillRewards;
        [SerializeField] private TowerCostSetup[] _towerCosts;

        private Dictionary<MobType, int> _mobRewardsDict;
        private Dictionary<TowerType, int> _towerCostsDict;
        private Dictionary<TowerType, int> _towerSellPricesDict;
            
        [Serializable]
        private class MobRewardSetup
        {
            public MobType Type;
            public int GoldReward;
        }
        
        [Serializable]
        private class TowerCostSetup
        {
            public TowerType Type;
            public int GoldCost;
            public int GoldSellPrice;
        }
    }
}