using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

namespace HotPotato.UI.Settings
{
    public class CameraDropdownSelect : MonoBehaviour
    {
        [Required]
        [SerializeField] private TMP_Dropdown _cameraDropdown;
        
        private const string SaveKey = "SelectedCameraName";
        
        public event Action<int> OnCameraSelectionChanged;

        private readonly List<string> _cameraNameList = new List<string>();

        private void Start()
        {
            PopulateCameraDropdown();
            LoadSavedCamera();
            _cameraDropdown.onValueChanged.AddListener(OnCameraSelected);
        }
        
        private void PopulateCameraDropdown()
        {
            _cameraDropdown.ClearOptions();
            
            WebCamDevice[] devices = WebCamTexture.devices;
            
            foreach (WebCamDevice device in devices)
            {
                _cameraNameList.Add(device.name);
            }
            
            if (_cameraNameList.Count == 0)
            {
                _cameraNameList.Add("No cameras available");
                _cameraDropdown.interactable = false;
            }
            else
            {
                _cameraDropdown.interactable = true;
            }
            
            _cameraDropdown.AddOptions(_cameraNameList);
        }
        
        private void OnCameraSelected(int index)
        {
            string cameraName = _cameraDropdown.options[index].text;
            
            ES3.Save(SaveKey, cameraName);
            Debug.Log($"Camera '{cameraName}' selected and saved.");
            OnCameraSelectionChanged?.Invoke(index);
        }
        
        private void LoadSavedCamera()
        {
            if (ES3.KeyExists(SaveKey))
            {
                string cameraName = ES3.Load<string>(SaveKey);
                int savedIndex = GetCameraIndexByName(cameraName);
                _cameraDropdown.SetValueWithoutNotify(savedIndex);
            }
        }
        
        private int GetCameraIndexByName(string cameraName)
        {
            foreach (var option in _cameraDropdown.options)
            {
                if (option.text == cameraName)
                {
                    Debug.Log($"Camera '{cameraName}' found in dropdown options.");
                    return _cameraDropdown.options.IndexOf(option);
                }
            }
            Debug.LogWarning($"Camera '{cameraName}' not found in dropdown options. Defaulting to first camera.");
            return 0; // Default to the first camera if not found
        }
    }
}