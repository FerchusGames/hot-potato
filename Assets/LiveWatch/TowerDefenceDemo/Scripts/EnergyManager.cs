using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class EnergyManager : MonoBehaviour
    {
        public event Action EnergyChanged;
        
        public int CurrentEnergy
        {
            get => _energy;
            set
            {
                if (_energy == value)
                    return;

                _energy = value;
                EnergyChanged?.Invoke();
            }
        }
        
        private int _energy;
        private LevelScene _levelScene;
        
        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }

        private void Start()
        {
            Watch.GetOrAdd("Energy", () => CurrentEnergy)
                .SetSortOrder(TD_WatchSortOrder.Energy)
                .SetTitleFormat(WatchTitleFormat.Blue)
                .SetDefaultValueFormat(WatchValueFormat.Cyan)
                .SetMinMaxModeAsGlobal()
                .SetCustomAction("+10", () => CurrentEnergy += 10)
                .SetCustomAction("+20", () => CurrentEnergy += 20)
                .SetCustomAction("+30", () => CurrentEnergy += 30);
        }

        private void OnEnable()
        {
            _levelScene.MobManager.MobDiedByHp += OnMobDiedByHp;
        }

        public void ResetEnergy()
        {
            CurrentEnergy = _levelScene.LevelConfig.StartGold;
            
            Watch.PushExtraText("Energy", $"Reset").PushStackTrace();
        }
        
        private void OnMobDiedByHp(MobMain mob)
        {
            var reward = _levelScene.EconomyConfig.MobKillRewards[mob.Type];
            CurrentEnergy += reward;

            Watch.PushExtraText("Energy", $"{mob.Id} killed");
        }
    }
}