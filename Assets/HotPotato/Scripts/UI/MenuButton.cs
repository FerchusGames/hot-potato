using System;
using HotPotato.Audio;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HotPotato.UI
{
    [RequireComponent(typeof(Button))]
    public class MenuButton: MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
    {
        protected Button _button;
        
        protected readonly Color _selectedTextColor = Color.white;

        protected TextMeshProUGUI _text;
        protected Color _originalColor;

        [SerializeField] private EventReferenceSO _hoverSoundEventReference;
        [SerializeField] private EventReferenceSO _clickSoundEventReference;
        
        [SerializeField] private Image _underlineImage;

        [SerializeField] private bool _isUnderlined = true;
        
        protected virtual void Awake()
        {
            if (_underlineImage != null) _underlineImage.enabled = false;
            
            _button = gameObject.GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _originalColor = _text.color;
            
            _button.onClick.AddListener(PlayClickSound);
        }

        protected virtual void OnDisable()
        {
            ResetColor();
            HideUnderline();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnButtonSelected();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnButtonDeselected();
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            OnButtonSelected();
        }

        private void OnButtonSelected()
        {
            SetSelectedColor();
            PlaySelectedSound();

            if (_isUnderlined)
            {
                _underlineImage.enabled = true;
            }
            
            PlaySelectedSound();
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            OnButtonDeselected();
        }

        private void OnButtonDeselected()
        {
            ResetColor();
            HideUnderline();
        }

        private void HideUnderline()
        {
            if (_isUnderlined)
            {
                _underlineImage.enabled = false;
            }
        }

        public void SetSelectedColor()
        {
            _text.color = _selectedTextColor;
        }
        
        protected virtual void ResetColor()
        {
            _text.color = _originalColor;
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