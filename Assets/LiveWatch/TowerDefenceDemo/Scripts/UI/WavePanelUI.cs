using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo.UI
{
    public class WavePanelUI : MonoBehaviour
    {
        [SerializeField] private Text waveText;
        
        private LevelScene _levelScene;

        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }

        private void OnEnable()
        {
            _levelScene.WaveManager.WaveChanged += RefreshWave;
        }

        private void OnDisable()
        {
            _levelScene.WaveManager.WaveChanged -= RefreshWave;
        }

        private void Start()
        {
            RefreshWave();
        }

        private void RefreshWave()
        {
            waveText.text = $"WAVE: {_levelScene.WaveManager.CurrentWave+1}/{_levelScene.WaveManager.MaxWave+1}";
        }
    }
}