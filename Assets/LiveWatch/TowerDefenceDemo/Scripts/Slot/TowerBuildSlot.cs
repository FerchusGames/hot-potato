using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class TowerBuildSlot : MonoBehaviour
    {
        public event Action OccupationChanged;
        
        public bool IsOccupied
        {
            get => _isOccupied;
            protected set
            {
                if (_isOccupied == value)
                    return;

                _isOccupied = value;
                OccupationChanged?.Invoke();
            }
        }
        public int Id { get; set; }
        public TowerBase Tower { get; protected set; }
        public Vector3 BuildLocation => transform.position;

        [SerializeField] private GameObject _emptyStateObj;
        
        public void Occupy(TowerBase tower)
        {
            Tower = tower;
            IsOccupied = true;
            
            _emptyStateObj.SetActive(false);
        }

        public void Empty()
        {
            IsOccupied = false;
            Tower = null;
            
            _emptyStateObj.SetActive(true);
        }
        
        private bool _isOccupied;
    }
}
