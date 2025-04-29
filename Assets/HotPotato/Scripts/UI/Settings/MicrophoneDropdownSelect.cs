using System.Collections.Generic;
using Dissonance.Integrations.FMOD_Recording;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HotPotato.UI.Settings
{
    public class MicrophoneDropdownSelect : MonoBehaviour
    {
        [Required]
        [SerializeField] private TMP_Dropdown _microphoneDropdown;
        
        private const string SaveKey = "SelectedMicrophoneName";
        
        private readonly List<string> _microphoneNameList = new List<string>();
        
        private void Start()
        {
            PopulateMicrophoneDropdown();
            LoadSavedMicrophone();
            _microphoneDropdown.onValueChanged.AddListener(OnMicrophoneSelected);
        }

        private void PopulateMicrophoneDropdown()
        {
            _microphoneDropdown.ClearOptions();

            List<string> devices = new List<string>();
            
            FMODMicrophoneInput.GetDevices(devices);
            
            foreach (string device in devices)
            {
                _microphoneNameList.Add(device);
            }
            
            if (devices.Count == 0)
            {
                devices.Add("No microphones available");
                _microphoneDropdown.interactable = false;
            }
            else
            {
                _microphoneDropdown.interactable = true;
            }
            
            _microphoneDropdown.AddOptions(devices);
        }
        
        private void OnMicrophoneSelected(int index)
        {
            string microphoneName = _microphoneDropdown.options[index].text;
            
            ES3.Save(SaveKey, microphoneName);
            Debug.Log($"Microphone '{microphoneName}' selected and saved.");
            EventBus<MicrophoneSelectedEvent>.Raise(new MicrophoneSelectedEvent
            {
                MicrophoneName = microphoneName,
            });
        }

        private void LoadSavedMicrophone()
        {
            if (ES3.KeyExists(SaveKey))
            {
                string microphoneName = ES3.Load<string>(SaveKey);
                int savedIndex = GetMicrophoneIndexByName(microphoneName);
                _microphoneDropdown.SetValueWithoutNotify(savedIndex);
            }
        }
        
        private int GetMicrophoneIndexByName(string cameraName)
        {
            foreach (var option in _microphoneDropdown.options)
            {
                if (option.text == cameraName)
                {
                    Debug.Log($"Camera '{cameraName}' found in dropdown options.");
                    return _microphoneDropdown.options.IndexOf(option);
                }
            }
            Debug.LogWarning($"Camera '{cameraName}' not found in dropdown options. Defaulting to first camera.");
            return 0; // Default to the first camera if not found
        }
    }
}