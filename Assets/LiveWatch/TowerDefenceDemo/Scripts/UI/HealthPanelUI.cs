using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo.UI
{
    public class HealthPanelUI : MonoBehaviour
    {
        [SerializeField] private float animationExtraScale = 0.2f;
        [SerializeField] private float animationScaleDuration = 0.2f;
        [SerializeField] private Text healthText;

        private LevelScene _levelScene;

        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }
        
        private void OnEnable()
        {
            _levelScene.HealthManager.HealthChanged += OnHealthChanged;
            RefreshHealth(false);
        }

        private void OnDisable()
        {
            _levelScene.HealthManager.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            RefreshHealth();
        }
        
        private void RefreshHealth(bool animate = true)
        {
            healthText.text = $"{_levelScene.HealthManager.CurrentHealth}";

            if (animate)
            {
                StopAllCoroutines();
                StartCoroutine(AnimateChange());
            }
        }

        private IEnumerator AnimateChange()
        {
            healthText.transform.localScale = Vector3.one;
            
            var zoomOutTime = 0f;
            while (zoomOutTime <= animationScaleDuration)
            {
                yield return new WaitForEndOfFrame();
                zoomOutTime += Time.deltaTime;
                var progress = Mathf.Clamp01(Mathf.InverseLerp(0, animationScaleDuration, zoomOutTime));
                healthText.transform.localScale = (1 + progress * animationExtraScale) * Vector3.one;
            }
            
            var zoomInTime = 0f;
            while (zoomInTime <= animationScaleDuration)
            {
                yield return new WaitForEndOfFrame();
                zoomInTime += Time.deltaTime;
                var progress = Mathf.Clamp01(Mathf.InverseLerp(0, animationScaleDuration, zoomInTime));
                healthText.transform.localScale = (1 + animationExtraScale - progress * animationExtraScale) * Vector3.one;
            }

            healthText.transform.localScale = Vector3.one;
        }
    }
}