using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo.UI
{
    public class EnergyPanelUI : MonoBehaviour
    {
        [SerializeField] private float _animationExtraScale = 0.2f;
        [SerializeField] private float _animationScaleDuration = 0.2f;
        [SerializeField] private Text _energyText;

        private LevelScene _levelScene;

        private void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
        }
        
        private void OnEnable()
        {
            _levelScene.EnergyManager.EnergyChanged += OnEnergyChanged;
            RefreshEnergy(false);
        }

        private void OnDisable()
        {
            _levelScene.EnergyManager.EnergyChanged -= OnEnergyChanged;
        }

        private void OnEnergyChanged()
        {
            RefreshEnergy();
        }
        
        private void RefreshEnergy(bool animate = true)
        {
            _energyText.text = $"{_levelScene.EnergyManager.CurrentEnergy}";

            if (animate)
            {
                StopAllCoroutines();
                StartCoroutine(AnimateChange());
            }
        }

        private IEnumerator AnimateChange()
        {
            _energyText.transform.localScale = Vector3.one;
            
            var zoomOutTime = 0f;
            while (zoomOutTime <= _animationScaleDuration)
            {
                yield return new WaitForEndOfFrame();
                zoomOutTime += Time.deltaTime;
                var progress = Mathf.Clamp01(Mathf.InverseLerp(0, _animationScaleDuration, zoomOutTime));
                _energyText.transform.localScale = (1 + progress * _animationExtraScale) * Vector3.one;
            }
            
            var zoomInTime = 0f;
            while (zoomInTime <= _animationScaleDuration)
            {
                yield return new WaitForEndOfFrame();
                zoomInTime += Time.deltaTime;
                var progress = Mathf.Clamp01(Mathf.InverseLerp(0, _animationScaleDuration, zoomInTime));
                _energyText.transform.localScale = (1 + _animationExtraScale - progress * _animationExtraScale) * Vector3.one;
            }

            _energyText.transform.localScale = Vector3.one;
        }
    }
}