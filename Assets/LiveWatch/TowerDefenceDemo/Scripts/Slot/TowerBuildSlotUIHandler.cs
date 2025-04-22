using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class TowerBuildSlotUIHandler : MonoBehaviour
    {
        [SerializeField] private TowerBuildSlot _targetSlot;
        [SerializeField] private ClickableObject _slotClickable;
        [SerializeField] private TowerSlotBuildPopupUI _buildUI;
        [SerializeField] private TowerSlotDestroyPopupUI _destroyUI;

        private void Awake()
        {
            _buildUI.TargetSlot = _targetSlot;
            _destroyUI.TargetSlot = _targetSlot;
        }

        private void OnEnable()
        {
            _slotClickable.Clicked += OnSlotClicked;
            _targetSlot.OccupationChanged += OnSlotOccupied;
        }

        private void OnDisable()
        {
            _slotClickable.Clicked -= OnSlotClicked;
            _targetSlot.OccupationChanged -= OnSlotOccupied;
        }

        private void OnSlotClicked()
        {
            if (_targetSlot.IsOccupied)
            {
                _destroyUI.Open();
            }
            else
            {
                _buildUI.Open();
            }
        }
        
        private void OnSlotOccupied()
        {
            if (_targetSlot.IsOccupied)
            {
                _buildUI.Close();
            }
            else
            {
                _destroyUI.Close();
            }
        }
    }
}