using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Ingvar.LiveWatch.UI
{
    public class WatchSaveUI : MonoBehaviour
    {
        public bool IsOpened { get; private set; }
        
        [SerializeField] private Button _openButton;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private RectTransform _progressFill;
        [SerializeField] private RectTransform _progressBack;
        [SerializeField] private Text _statusText;
        [SerializeField] private InputField _fileNameField;
        
        private void Awake()
        {
            _statusText.text = string.Empty;
            _panel.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _openButton.onClick.AddListener(Open);
        }

        private void OnDisable()
        {
            _openButton.onClick.RemoveListener(Open);
        }
        
        private void Update()
        {
            if (!IsOpened)
                return;

            if (WatchServices.SaveLoader.IsSaving)
            {
                SetProgress(WatchServices.SaveLoader.Progress.Progress);
            }
        }

        public void Open()
        {
            if (IsOpened)
                return;

            IsOpened = true;
            
            _openButton.gameObject.SetActive(false);
            _panel.SetActive(true);
            
            _closeButton.onClick.AddListener(Close);
            _saveButton.onClick.AddListener(Save);

            SetProgress(0);

            if (!WatchServices.SaveLoader.IsSaving)
            {
                var time = DateTime.Now;
                _fileNameField.text = $"Watches_{time.Year}-{time.Month}-{time.Day}_{time.Hour}-{time.Minute}-{time.Second}";
            }
        }
        
        public void Close()
        {
            if (!IsOpened)
                return;

            IsOpened = false; 
            
            _openButton.gameObject.SetActive(true);
            _panel.SetActive(false);
            
            _closeButton.onClick.RemoveListener(Close);
            _saveButton.onClick.RemoveListener(Save);
        }

        private void Save()
        {
            _saveButton.interactable = false;
            _statusText.text = "Saving..";
            SetProgress(0);
            MainThreadDispatcher.RefreshInstance();
            
            var path = GetFilePath();
            WatchServices.SaveLoader.Save(path, Watch.Watches, (succeed, message) =>
            {
                MainThreadDispatcher.Dispatch(() =>
                {
                    _saveButton.interactable = true;
                    _statusText.text = message;
                    SetProgress(1f);
                    
                    #if UNITY_EDITOR
                    UnityEditor.EditorUtility.ClearProgressBar();
                    #endif
                });
            });
        }

        private void SetProgress(float progress)
        {
            var sizeX = Mathf.Lerp(_progressBack.rect.width, 0, progress);
            _progressFill.offsetMax = new Vector2(-sizeX, _progressFill.offsetMax.y);
        }
        
        private string GetFilePath()
        {
            return Path.Combine(Application.persistentDataPath, $"{_fileNameField.text}.{WatchServices.SaveLoader.Extension}");
        }
    }
}