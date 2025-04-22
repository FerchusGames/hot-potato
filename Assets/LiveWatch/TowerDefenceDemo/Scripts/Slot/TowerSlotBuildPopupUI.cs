using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class TowerSlotBuildPopupUI : BasePopupUI
    {
        [SerializeField] private List<BuildButton> _buildButtons = new();

        public TowerBuildSlot TargetSlot { get; set; }
        
        protected override void OnOpened()
        {
            base.OnOpened();

            _levelScene.EnergyManager.EnergyChanged += OnEnergyChanged;
            
            foreach (var buildButton in _buildButtons)
            {
                RefreshButton(buildButton);
                buildButton.Button.onClick.AddListener(() => OnButtonClicked(buildButton));
            }
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            
            _levelScene.EnergyManager.EnergyChanged -= OnEnergyChanged;
            
            foreach (var buildButton in _buildButtons)
            {
                buildButton.Button.onClick.RemoveAllListeners();
            }
        }

        private void RefreshButton(BuildButton buildButton)
        {
            var price = _levelScene.EconomyConfig.TowerBuildCosts[buildButton.Type];
            var playerEnergy = _levelScene.EnergyManager.CurrentEnergy;

            buildButton.Button.interactable = playerEnergy >= price;
            buildButton.Text.text = $"{buildButton.Type}{Environment.NewLine}COST: -{price}".ToUpper();
        }
        
        private void OnEnergyChanged()
        {
            foreach (var buildButton in _buildButtons)
                RefreshButton(buildButton);
        }

        private void OnButtonClicked(BuildButton buildButton)
        {
            _levelScene.TowerBuildManager.BuildTower(TargetSlot, buildButton.Type);
        }
        
        [Serializable]
        public class BuildButton
        {
            [FormerlySerializedAs("Tower")] public TowerType Type;
            public Button Button;
            public Text Text;
        }
    }
}