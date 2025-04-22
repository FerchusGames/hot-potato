using System.Collections;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class LaserEffect : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.05f;
        [SerializeField] private GameObject _laserBeam;
        
        public void Play(Vector3 target)
        {
            target += Vector3.up * 0.5f;
                
            var dist = (target - transform.position).magnitude;
            transform.LookAt(target);
            _laserBeam.transform.localScale = new Vector3(
                _laserBeam.transform.localScale.x,
                _laserBeam.transform.localScale.y,
                dist/2f);
            
            StopAllCoroutines();
            StartCoroutine(ShowAndHide());
        }

        private IEnumerator ShowAndHide()
        {
            _laserBeam.SetActive(true);
            yield return new WaitForSeconds(_duration);
            _laserBeam.SetActive(false);
        }
    }
}