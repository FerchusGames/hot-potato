using System;
using System.Collections.Generic;
using System.Threading;
using DlibFaceLandmarkDetector;
using DlibFaceLandmarkDetector.UnityUtils;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace TvFaceNoise.Scripts
{
    public interface IWebCamEvents
    {
        public event Action OnWebCamUpdated;
    }

    public class WebCam : MonoBehaviour, IWebCamEvents
    {
        [Required]
        [SerializeField] private RawImage _displayImage;
        
        [SerializeField] private int _requestedWidth = 320;
        [SerializeField] private int _requestedHeight = 240;
        [SerializeField] private int _requestedFPS = 30;
        
        [SerializeField] private Vector2 _topLeft;
        [SerializeField] private float _width;
        
        private WebCamTexture _webCamTexture;
        
        // FACE DETECTION
        
        private FaceLandmarkDetector _faceLandmarkDetector;
        private Color32[] _webCamColors;
        private Texture2D _texture;
        
        private List<Rect> _detectedFaces = new List<Rect>();
        
        string _dlibShapePredictorFileName = "DlibFaceLandmarkDetector/sp_human_face_6.dat";
        string _dlibShapePredictorFilePath;
        
        CancellationTokenSource _cts = new CancellationTokenSource();

        [SerializeField] private int _faceDetectionRate = 1;
        
        private float _faceDetectionTimer = 0f;
        
        // END FACE DETECTION
        
        public event Action OnWebCamUpdated;
        
        private float _timer = 0f;
        
        void Start()
        {
            InitializeFaceDetector();
            InitializeWebCam();
        }
        
        private async void InitializeFaceDetector()
        {
            _dlibShapePredictorFileName = DlibFaceLandmarkDetectorExample.DlibFaceLandmarkDetectorExample.dlibShapePredictorFileName;
            _dlibShapePredictorFilePath = await Utils.getFilePathAsyncTask(_dlibShapePredictorFileName, cancellationToken: _cts.Token);
            _faceLandmarkDetector = new FaceLandmarkDetector(_dlibShapePredictorFilePath);
        }
        
        private void InitializeWebCam()
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            
            WebCamDevice obsCamera = new WebCamDevice();

            bool found = false;
            
            foreach (WebCamDevice device in devices)
            {
                if (device.name.Contains("OBS Virtual Camera"))
                {
                    obsCamera = device;
                    Debug.Log($"Found OBS Virtual Camera: {obsCamera.name}");
                    found = true;
                    break;
                }
            }
            
            if (!found)
            {
                Debug.LogError("Found no OBS Virtual Camera. Please ensure it is installed and available.");
                return;
            }
            
            // Create and start the webcam texture using the first available device.
            _webCamTexture = new WebCamTexture(obsCamera.name, _requestedWidth, _requestedHeight, _requestedFPS);
            _webCamTexture.Play();
            
            _texture = new Texture2D(_webCamTexture.width, _webCamTexture.height, TextureFormat.RGBA32, false);
            
            // Assign the webcam texture directly to the RawImage.
            _displayImage.texture = _webCamTexture;
            
            // Adjust for rotation if needed.
            _displayImage.rectTransform.localEulerAngles = new Vector3(0, 0, -_webCamTexture.videoRotationAngle);
            
            // Get the webcam image data
            if (_webCamColors == null || _webCamColors.Length != _webCamTexture.width * _webCamTexture.height)
            {
                _webCamColors = new Color32[_webCamTexture.width * _webCamTexture.height];
            }
            
            // Apply the initial UV cropping.
            DetectFaces();
            UpdateRawImageUVRect();
        }

        void Update()
        {
            _timer += Time.deltaTime;
            _faceDetectionTimer += Time.deltaTime;
            
            var timeToWait = 1f / _requestedFPS;
            var faceDetectionTimeToWait = 1f / _faceDetectionRate;

            if (_faceDetectionTimer >= faceDetectionTimeToWait)
            {
                _faceDetectionTimer -= faceDetectionTimeToWait;
                DetectFaces();
                UpdateRawImageUVRect();
            }
            
            if (_timer >= timeToWait)
            {
                _timer -= timeToWait;
                OnWebCamUpdated?.Invoke();
            }
        }
        
        private void DetectFaces()
        {
            if (_webCamTexture == null || _faceLandmarkDetector == null) return;
        
            _webCamTexture.GetPixels32(_webCamColors);

            // Process the image with the face detector
            _faceLandmarkDetector.SetImage<Color32>(_webCamColors, _webCamTexture.width, _webCamTexture.height, 4, true);

            // Detect face rectangles
            _detectedFaces = _faceLandmarkDetector.Detect();
        }
        
        private void UpdateRawImageUVRect()
        {
            if(_webCamTexture == null || _detectedFaces == null || _detectedFaces.Count == 0)
                return;

            Rect detectedFace = _detectedFaces[0];
            
            Debug.Log($"X: {detectedFace.x}, Y: {detectedFace.y}, Width: {detectedFace.width}, Height: {detectedFace.height}");
            
            _topLeft = new Vector2(detectedFace.xMin, detectedFace.yMin);
            _width = detectedFace.width;
            
            // Calculate UV coordinates from pixel values.
            float uvX = _topLeft.x / _webCamTexture.width;
            float uvY = _topLeft.y / _webCamTexture.height;
            float uvWidth = _width / _webCamTexture.width;
            float uvHeight = _width / _webCamTexture.height;
            
            uvY = 1 - uvY - uvHeight; // Invert Y coordinate for Unity's UV mapping.
            
            // Set the uvRect to display only the desired portion of the webcam texture.
            _displayImage.uvRect = new Rect(uvX, uvY, uvWidth, uvHeight);
        }

        private void OnDestroy()
        {
            if (_webCamTexture != null && _webCamTexture.isPlaying)
                _webCamTexture.Stop();
        }
    }
}
