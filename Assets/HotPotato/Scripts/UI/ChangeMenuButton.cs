using HotPotato.Audio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HotPotato.UI
{
    [RequireComponent(typeof(Button))]
    public class ChangeMenuButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
    {
        [SerializeField, Required] private GameObject _currentMenu;
        [SerializeField, Required] private GameObject _targetMenu;
        
        [SerializeField] private EventReferenceSO _hoverSoundEventReference;
        [SerializeField] private EventReferenceSO _clickSoundEventReference;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ChangeMenu);
        }
        
        private void ChangeMenu()
        {
            PlayClickSound();
            _targetMenu.SetActive(true);
            _currentMenu.SetActive(false);
        }

        public void OnSelect(BaseEventData eventData)
        {
            PlaySelectedSound();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlaySelectedSound();
        }
        
        private void PlayClickSound()
        {
            if (_clickSoundEventReference != null)
            {
                AudioManager.Instance.PlayOneShot(_clickSoundEventReference.EventReference, transform.position);
            }
        }
        
        private void PlaySelectedSound()
        {
            if (_hoverSoundEventReference != null)
            {
                AudioManager.Instance.PlayOneShot(_hoverSoundEventReference.EventReference, transform.position);
            }
        }
    }
}