using System;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class ReturnToPoolTimer : MonoBehaviour
    {
        [SerializeField] private bool useUnscaledTime = false;
        [SerializeField] private float delaySeconds = 3;

        private PoolManager poolManager;
        private float currentTimer;
        private bool isTimerTicking;

        private void Awake()
        {
            poolManager = FindObjectOfType<PoolManager>();
        }

        private void OnEnable()
        {
            isTimerTicking = true;
            currentTimer = delaySeconds;
        }

        private void OnDisable()
        {
            isTimerTicking = false;
        }

        private void Update()
        {
            if (!isTimerTicking)
                return;

            currentTimer -= useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            if (currentTimer <= 0)
            {
                isTimerTicking = false;
                poolManager.Push(gameObject);
            }
        }
    }
}