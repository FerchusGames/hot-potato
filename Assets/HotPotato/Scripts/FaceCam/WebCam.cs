using System;
using HotPotato.UI.Settings;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.FaceCam
{
    public interface IWebCamEvents
    {
        public event Action OnWebCamUpdated;
    }

    public class WebCam : MonoBehaviour, IWebCamEvents
    {
        [Required]
        [SerializeField] private RawImage _displayImage;
        
        [SerializeField] private int _requestedWidth = 1920;
        [SerializeField] private int _requestedHeight = 1080;
        [SerializeField] private int _requestedFPS = 30;
        
        [SerializeField] private Vector2 _bottomLeft;
        [SerializeField] private Vector2 _topRight;
        
        private WebCamTexture _webCamTexture;
        public event Action OnWebCamUpdated;
        
        private EventBinding<CameraSelectedEvent> _cameraSelectedEventBinding;
        
        private float _timer = 0f;
        
        void Start()
        {
            InitializeWebCam();
            
            _cameraSelectedEventBinding = new EventBinding<CameraSelectedEvent>(ChangeCamera);
            EventBus<CameraSelectedEvent>.Register(_cameraSelectedEventBinding);
        }
        
        private void OnDestroy()
        {
            if (_webCamTexture != null && _webCamTexture.isPlaying)
                _webCamTexture.Stop();
            
            EventBus<CameraSelectedEvent>.Deregister(_cameraSelectedEventBinding);
        }
        
        private void InitializeWebCam()
        {
            string cameraName;
            
            if (ES3.KeyExists("SelectedCameraName"))
            {
                cameraName = ES3.Load<string>("SelectedCameraName");
                Debug.Log($"Loaded camera name: {cameraName}");
            }
            else
            {
                WebCamDevice[] devices = WebCamTexture.devices;
                
                if (devices.Length == 0)
                {
                    Debug.LogError("No webcam devices found!");
                    return;
                }
                
                cameraName = devices[0].name;
            }
            
            // Create and start the webcam texture using the first available device.
            _webCamTexture = new WebCamTexture(cameraName, _requestedWidth, _requestedHeight, _requestedFPS);
            _webCamTexture.Play();
            
            // Assign the webcam texture directly to the RawImage.
            _displayImage.texture = _webCamTexture;
            
            // Adjust for rotation if needed.
            _displayImage.rectTransform.localEulerAngles = new Vector3(0, 0, -_webCamTexture.videoRotationAngle);
            
            // Apply the initial UV cropping.
            UpdateRawImageUVRect();
        }
        
        private void ChangeCamera(CameraSelectedEvent cameraSelectedEvent)
        {
            _webCamTexture.Stop();
            _webCamTexture.deviceName = cameraSelectedEvent.CameraName;
            _webCamTexture.Play();
            
            _displayImage.rectTransform.localEulerAngles = new Vector3(0, 0, -_webCamTexture.videoRotationAngle);
    
            UpdateRawImageUVRect();
        }

        void Update()
        {
            _timer += Time.deltaTime;
            
            var timeToWait = 1f / _requestedFPS;

            if (!(_timer >= timeToWait)) return;
            
            _timer -= timeToWait;
            UpdateRawImageUVRect();
            OnWebCamUpdated?.Invoke();
        }
        
        private void UpdateRawImageUVRect()
        {
            if(_webCamTexture == null)
                return;

            // Calculate UV coordinates from pixel values.
            float uvX = _bottomLeft.x / _webCamTexture.width;
            float uvY = _bottomLeft.y / _webCamTexture.height;
            float uvWidth = (_topRight.x - _bottomLeft.x) / _webCamTexture.width;
            float uvHeight = (_topRight.y - _bottomLeft.y) / _webCamTexture.height;
            
            // Set the uvRect to display only the desired portion of the webcam texture.
            _displayImage.uvRect = new Rect(uvX, uvY, uvWidth, uvHeight);
        }
    }
}