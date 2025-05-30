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
        
        [SerializeField] private Image _underlineImage;

        [SerializeField] private bool _isUnderlined = true;
        
        [SerializeField] private bool _shouldPlayClickSound = true;
        
        protected virtual void Awake()
        {
            if (_underlineImage != null) _underlineImage.enabled = false;
            
            _button = gameObject.GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _originalColor = _text.color;
            
            if (_shouldPlayClickSound) _button.onClick.AddListener(PlayClickSound);
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
        
        protected virtual void PlayClickSound()
        {
            // Play a click sound when the button is clicked
        }
        
        private void PlaySelectedSound()
        {
            // Play a sound when the button is selected
        }
    }   
}