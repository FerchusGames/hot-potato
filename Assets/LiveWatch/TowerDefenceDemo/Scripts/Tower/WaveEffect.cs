using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class WaveEffect : MonoBehaviour
    {
        [SerializeField] private float _growDuration = 0.1f;
        [SerializeField] private float _stayDuration = 0.1f;
        [SerializeField] private Transform _wave;

        private float _radius;
        private Coroutine _effectRoutine;

        private void OnEnable()
        {
            SetSizeRaw(0);
        }

        public void SetRadius(float radius)
        {
            _radius = radius;
        }

        public void Play()
        {
            SetSizeRaw(0);
            
            if (_effectRoutine != null)
                StopCoroutine(_effectRoutine);

            _effectRoutine = StartCoroutine(EffectPlaying());
        }

        private IEnumerator EffectPlaying()
        {
            SetSizeRaw(0);

            var timer = _growDuration;

            while (timer >= 0)
            {
                timer -= Time.deltaTime;
                
                var progress = 1 - timer / _growDuration;
                SetSizeRaw(_radius * progress);

                yield return null;
            }

            yield return new WaitForSeconds(_stayDuration);
            
            SetSizeRaw(0);
        }

        private void SetSizeRaw(float value)
        {
            _wave.localScale = value * Vector3.one;
        }
    }
}