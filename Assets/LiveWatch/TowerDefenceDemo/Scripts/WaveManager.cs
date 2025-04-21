using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class WaveManager : MonoBehaviour
    {
        public event Action WaveStarted;
        public event Action WaveFinished;
        public event Action WaveChanged;
        
        public bool IsActiveWave { get; private set; }
        public int CurrentWave
        {
            get => _wave;
            set
            {
                if (_wave == value)
                    return;

                _wave = value;
                WaveChanged?.Invoke();
            }
        }
        public int MaxWave => _levelScene.LevelConfig.Waves.Count - 1;
        public int CurrentSpawn { get; private set; }
        
        private LevelScene _levelScene;
        private int _wave;
        private Coroutine _waveRoutine;
        private List<MobWave> waves => _levelScene.LevelConfig.Waves;

        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }
        
        private void Start()
        {
            TD_Watches.GetOrAdd("Waves", () => this)
                .GetOrAdd<Any>(nameof(MaxWave)).SetSortOrder(-10);
        }

        public void ResetWaves()
        {
            CurrentWave = 0;
        }

        public void SetNextWave()
        {
            CurrentWave = Mathf.Clamp(CurrentWave + 1, 0, MaxWave);
        }
        
        public void StartWave()
        {
            IsActiveWave = true;
            _waveRoutine = StartCoroutine(WaveProcessing(waves[CurrentWave]));
            WaveStarted?.Invoke();
        }

        public void FinishWave()
        {
            if (_waveRoutine != null)
                StopCoroutine(_waveRoutine);
            
            IsActiveWave = false;
            WaveFinished?.Invoke();
        }
        
        private IEnumerator WaveProcessing(MobWave wave)
        {
            CurrentSpawn = 0;
            
            foreach (var spawn in wave.Spawns)
            {
                yield return new WaitForSeconds(spawn.Delay);

                for (var i = 0; i < spawn.MobCount; i++)
                {
                    _levelScene.MobManager.SpawnMob(spawn.MobPrefab);
                    yield return new WaitForSeconds(spawn.SpawnDelayBetween); 
                }
                
                CurrentSpawn++;
            }
            
            yield return new WaitWhile(() => _levelScene.MobManager.Mobs.Count > 0);
            FinishWave();
        }
    }
}