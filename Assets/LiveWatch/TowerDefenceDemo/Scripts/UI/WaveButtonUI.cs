using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo.UI
{
    public class WaveButtonUI : MonoBehaviour
    {
        [SerializeField] private Button playWaveButton;
        
        private LevelScene _levelScene;

        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }

        private void OnEnable()
        {
            _levelScene.LevelStateManager.StateChanged += RefreshButton;
            playWaveButton.onClick.AddListener(OnPlayButtonClicked);

            RefreshButton();
        }

        private void OnDisable()
        {
            _levelScene.LevelStateManager.StateChanged -= RefreshButton;
            playWaveButton.onClick.RemoveListener(OnPlayButtonClicked);
        }
        
        private void RefreshButton()
        {
            playWaveButton.interactable = _levelScene.LevelStateManager.CurrentState == LevelStateType.Waiting;
        }

        private void OnPlayButtonClicked()
        {
            _levelScene.WaveManager.StartWave();
        }
    }
}