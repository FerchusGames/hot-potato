using System;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class MobHealth : MonoBehaviour
    {
        public Action HealthChanged;

        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (_currentHealth == value)
                    return;

                _currentHealth = value;
                HealthChanged?.Invoke();
                
                if (_currentHealth <= 0)
                    _deathCallback?.Invoke();
            }
        }
        public int MaxHealth => maxHealth;
        
        [SerializeField] private int maxHealth;
        private int _currentHealth;
        private Action _deathCallback;

        public void ResetHealth()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, MaxHealth);
        }

        public void SetOnDeath(Action deathCallback)
        {
            _deathCallback = deathCallback;
        }
    }
}