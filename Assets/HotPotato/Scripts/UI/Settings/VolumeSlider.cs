using HotPotato.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI.Settings
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private AudioBus _audioBus = AudioBus.Master;
        
        private Slider _slider;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.onValueChanged.AddListener(OnVolumeChanged);
        }

        private void OnVolumeChanged(float volume)
        {
            AudioManager.Instance.SetBusVolume(_audioBus, volume);
        }
    }
}