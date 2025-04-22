using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class LevelStateManager : MonoBehaviour
    {
        public event Action StateChanged;

        public LevelStateType CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState == value)
                    return;

                _currentState = value;
                StateChanged?.Invoke();
            }
        }

        private LevelStateType _currentState;
        private LevelScene _levelScene;

        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }

        private void Start()
        {
            StartLevel();

            TD_Watches.GetOrAdd("State", () => CurrentState).SetSortOrder(TD_WatchSortOrder.State);
        }

        public void StartLevel()
        {
            if (CurrentState == LevelStateType.Playing)
                return;

            CurrentState = LevelStateType.Waiting;
            
            _levelScene.SpeedManager.SetDefaultSpeed();
            _levelScene.HealthManager.ResetHealth();
            _levelScene.EnergyManager.ResetEnergy();
            _levelScene.WaveManager.ResetWaves();
            _levelScene.TowerBuildManager.DestroyAllTowers();
            _levelScene.InitialTowerSpawner.SpawnAllTowers();
            
            _levelScene.HealthManager.HealthChanged += OnHealthChanged;
            _levelScene.WaveManager.WaveStarted += OnWaveStarted;
            _levelScene.WaveManager.WaveFinished += OnWaveFinished;
        }

        public void FinishLevel(bool isWin)
        {
            if (CurrentState is LevelStateType.Win or LevelStateType.Lose)
                return;

            CurrentState = isWin ? LevelStateType.Win : LevelStateType.Lose;
            
            _levelScene.TowerManager.DisableTowers();
            _levelScene.MobManager.DespawnMobsAll();
            _levelScene.WaveManager.FinishWave();

            _levelScene.HealthManager.HealthChanged -= OnHealthChanged;
            _levelScene.WaveManager.WaveStarted -= OnWaveStarted;
            _levelScene.WaveManager.WaveFinished -= OnWaveFinished;
        }
        
        private void OnHealthChanged()
        {
            if (_levelScene.HealthManager.CurrentHealth > 0)
                return;

            FinishLevel(false);
        }

        private void OnWaveStarted()
        {
            CurrentState = LevelStateType.Playing;
            
            _levelScene.TowerManager.EnabledTowers();
        }

        private void OnWaveFinished()
        {
            _levelScene.TowerManager.DisableTowers();
            _levelScene.MobManager.DespawnMobsAll();
            
            if (_levelScene.WaveManager.CurrentWave >= _levelScene.WaveManager.MaxWave)
            {
                FinishLevel(true);
                return;
            }

            _levelScene.WaveManager.SetNextWave();
            CurrentState = LevelStateType.Waiting;
        }
    }
}