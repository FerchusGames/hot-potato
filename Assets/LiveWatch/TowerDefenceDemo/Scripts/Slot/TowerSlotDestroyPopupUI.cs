using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class TowerSlotDestroyPopupUI : BasePopupUI
    {
        public TowerBuildSlot TargetSlot { get; set; }
        
        [SerializeField] private Button sellButton;
        [SerializeField] private Text sellPriceText;

        protected override void OnOpened()
        {
            base.OnOpened();
            
            var sellPrice = _levelScene.EconomyConfig.TowerSellPrices[TargetSlot.Tower.Type];
            sellPriceText.text = $"Destroy{Environment.NewLine}+{sellPrice}".ToUpper();
            
            sellButton.onClick.AddListener(OnSellClicked);
        }

        protected override void OnClosed()
        {
            base.OnClosed();
            
            sellButton.onClick.RemoveListener(OnSellClicked);
        }

        private void OnSellClicked()
        {
            _levelScene.TowerBuildManager.DestroyTower(TargetSlot);
        }
    }
}