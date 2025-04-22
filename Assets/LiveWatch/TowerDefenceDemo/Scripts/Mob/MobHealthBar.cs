using System;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class MobHealthBar : MonoBehaviour
    {
        [SerializeField] private MobMain mob;
        [SerializeField] private Transform bar;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            mob.Health.HealthChanged += RefreshBar;
            RefreshBar();
        }

        private void OnDisable()
        {
            mob.Health.HealthChanged -= RefreshBar;
        }

        private void LateUpdate()
        {
            transform.rotation = _mainCamera.transform.rotation;
        }

        private void RefreshBar()
        {
            var progress = (float)mob.Health.CurrentHealth / mob.Health.MaxHealth;
            bar.localScale = new Vector3(progress, bar.localScale.y, bar.localScale.z);
        }
    }
}