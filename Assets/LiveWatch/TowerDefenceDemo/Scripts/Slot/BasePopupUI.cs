using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    [Serializable]
    public abstract class BasePopupUI : MonoBehaviour
    {
        public bool IsOpened { get; private set; }

        [SerializeField] protected bool _closeByClickOutside = true;
        [SerializeField] protected bool _lookAtCamera = true;
        [SerializeField] protected GameObject _panel;

        protected LevelScene _levelScene;
        private Camera _mainCamera;
        private bool _skipNextClick;
        private HashSet<GameObject> _clickableChilds;
        
        protected virtual void Awake()
        {
            _levelScene = FindObjectOfType<LevelScene>();
            _clickableChilds = GetComponentsInChildren<Selectable>().Select(s => s.gameObject).ToHashSet();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            if (_lookAtCamera)
            {
                transform.rotation = _mainCamera.transform.rotation;
            }

            OnCreated();
        }

        protected virtual void Update()
        {
            if (IsOpened
                && _closeByClickOutside
                && Input.GetMouseButtonDown(0)
                && !_clickableChilds.Contains(EventSystem.current.currentSelectedGameObject))
            {
                if (_skipNextClick)
                    _skipNextClick = false;
                else
                    Close();
            }
        }

        protected virtual void OnCreated()
        {
            
        }

        protected virtual void OnOpened()
        {
            
        }

        protected virtual void OnClosed()
        {
            
        }
        
        public void Open()
        {
            if (IsOpened)
                return;
                
            IsOpened = true;

            _skipNextClick = true;
            _panel.SetActive(true);

            OnOpened();
        }
            
        public void Close()
        {
            if (!IsOpened)
                return;
                
            OnClosed();
            
            IsOpened = false;
            _panel.SetActive(false);
        }
    }
}