using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo.UI
{
    public class SpeedPanelUI : MonoBehaviour
    {
        [SerializeField] private List<SpeedToggle> _speedToggles;
        private LevelScene _levelScene;
        
        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }

        private void OnEnable()
        {
            _levelScene.SpeedManager.SpeedChanged += RefreshToggles;
            
            foreach (var speedToggle in _speedToggles)
                speedToggle.Toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(speedToggle, isOn));

            RefreshToggles();
        }

        private void OnDisable()
        {
            _levelScene.SpeedManager.SpeedChanged -= RefreshToggles;
            
            foreach (var speedToggle in _speedToggles)
                speedToggle.Toggle.onValueChanged.RemoveAllListeners();
        }

        private void RefreshToggles()
        {
            foreach (var speedToggle in _speedToggles)
                speedToggle.Toggle.SetIsOnWithoutNotify(speedToggle.Speed == _levelScene.SpeedManager.CurrentSpeed);
        }
        
        private void OnToggleChanged(SpeedToggle speedToggle, bool isOn)
        {
            if (!isOn)
                return;
            
            _levelScene.SpeedManager.SetGameSpeed(speedToggle.Speed);
        }
        
        [Serializable]
        private class SpeedToggle
        {
            public Toggle Toggle;
            public SpeedMode Speed;
        }
    }
}