using System;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class HealthManager : MonoBehaviour
    {
        public event Action HealthChanged;
        
        public int CurrentHealth
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;

                _health = value;
                HealthChanged?.Invoke();
            }
        }
        public int MaxHealth => _levelScene.LevelConfig.MaxHealth;

        private LevelScene _levelScene;
        private int _health;
        
        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }
        
        private void Start()
        {
            Watch.GetOrAdd("Health", () => CurrentHealth)
                .SetSortOrder(TD_WatchSortOrder.Health)
                .AddConditionalValueFormat(
                    (value) => value >= 17, 
                    new WatchValueFormat(WatchColors.Green))
                .AddConditionalValueFormat(
                    (value) => value is <= 17 and > 5, 
                    new WatchValueFormat(WatchColors.Yellow))
                .AddConditionalValueFormat(
                    (value) => value <= 5, 
                    new WatchValueFormat(WatchColors.Red))
                .SetTraceable()
                .SetMinMaxModeAsCustom(0, MaxHealth)
                .SetCustomAction("Reset", ResetHealth);
        }

        private void OnEnable()
        {
            _levelScene.MobManager.MobReachedFinish += OnMobReachedFinish;
        }

        private void OnDisable()
        {
            _levelScene.MobManager.MobReachedFinish -= OnMobReachedFinish;
        }

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;

            Watch.PushExtraText("Health", "Reset");
        }
        
        private void OnMobReachedFinish(MobMain mob)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - 1, 0, MaxHealth);
            HealthChanged?.Invoke();

            Watch.PushExtraText("Health", $"{mob.Id} reached Base");
            Watch.PushStackTrace("Health");
        }
    }
}