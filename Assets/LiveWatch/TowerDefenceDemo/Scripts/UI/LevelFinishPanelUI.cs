using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo.UI
{
    public class LevelFinishPanelUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panelBase;
        [SerializeField] private Text statusText;
        [SerializeField] private Button restartButton;
        
        private LevelScene _levelScene;
        
        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }

        private void OnEnable()
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
            _levelScene.LevelStateManager.StateChanged += LevelStateManagerOnStateChanged;

            SetShown(false);
        }
        
        private void OnDisable()
        {
            restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _levelScene.LevelStateManager.StateChanged -= LevelStateManagerOnStateChanged;
        }

        private void LevelStateManagerOnStateChanged()
        {
            if (_levelScene.LevelStateManager.CurrentState is not LevelStateType.Win and not LevelStateType.Lose)
                return;

            SetShown(true);
            
            statusText.text = _levelScene.LevelStateManager.CurrentState == LevelStateType.Win
                ? "YOU WON"
                : "YOU LOST";
        }
        
        private void OnRestartButtonClicked()
        {
            SetShown(false);
            _levelScene.LevelStateManager.StartLevel();
        }

        private void SetShown(bool isOn)
        {
            panelBase.alpha = isOn ? 1 : 0;
            panelBase.blocksRaycasts = isOn;
        }
    }
}