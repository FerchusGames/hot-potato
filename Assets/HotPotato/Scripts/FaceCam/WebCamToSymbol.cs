using System.IO;
using System.IO.Compression;
using System.Text;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine.Rendering;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace TvFaceNoise.Scripts
{
    public class WebCamToSymbol : NetworkBehaviour
    {
        [Required]
        [SerializeField] private RenderTexture _renderTexture;
        
        [Required]
        [SerializeField] private TextMeshProUGUI[] _outputAsciiText = null;
        
        [Required]
        [SerializeField] private Camera _webCamCamera = null;
        
        [Required]
        [SerializeField] private WebCam _webCam = null;
        
        [MinValue(2), MaxValue(16)]
        [SerializeField] private int _symbolCount = 16;
        
        [Range(0.5f, 10f)]
        [SerializeField] private float _luminosityMultiplier = 1f;
        
        [SerializeField] private int _frameRate = 24;
        
        private readonly SyncVar<byte[]> _asciiText = new ();
        
        private IWebCamEvents _webCamEvents;
        
        private bool _didWebCamUpdate = false;

        private static readonly char[] AsciiChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', };

        private void Awake()
        {
            _webCamEvents = _webCam as IWebCamEvents;
            _asciiText.UpdateSendRate(1f / _frameRate);
        }

        public override void OnStartClient()
        {
            _asciiText.OnChange += OnAsciiTextChanged;

            if (!IsServerInitialized) return;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
            _webCamEvents.OnWebCamUpdated += OnWebCamUpdated;
        }
        
        public override void OnStopClient()
        {
            _asciiText.OnChange -= OnAsciiTextChanged;
            
            if (!IsServerInitialized) return;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
            _webCamEvents.OnWebCamUpdated -= OnWebCamUpdated;
        }

        private void OnWebCamUpdated()
        {
            _didWebCamUpdate = true;
        }

        private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            if (camera != _webCamCamera || !_didWebCamUpdate) return;
            
            _didWebCamUpdate = false;
            
            Texture2D texture2D = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
            texture2D.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            
            var asciiText = ConvertToAsciiIndexPacked(texture2D.GetPixels());
            
            _asciiText.Value = Compress(asciiText);
            
            Debug.Log($"asciiText.Length: {asciiText.Length}, compressedAscii.Length: {_asciiText.Value.Length}");
        }

        private void OnAsciiTextChanged(byte[] value, byte[] newValue, bool asServer)
        {
            var uncompressedAscii = Decompress(newValue);
            
            var asciiArt = ConvertToAsciiArtPacked(uncompressedAscii);
            
            string[] lines = SplitByWidth(asciiArt, _renderTexture.width);
            
            for (int i = 0; i < _outputAsciiText.Length && i < lines.Length; i++)
            {
                _outputAsciiText[i].text = lines[i];
            } 
        }
        
        private byte[] ConvertToAsciiIndexPacked(Color[] pixels)
        {
            // pixels.Length is always even
            int packedLength = pixels.Length / 2;
            byte[] asciiPacked = new byte[packedLength];
            float luminosity;

            for (int i = 0; i < pixels.Length; i += 2)
            {
                // first pixel → high nibble
                luminosity = Mathf.Clamp(pixels[i].PerceivedLuminosity() * _luminosityMultiplier, 0, 1);
                int idx1 = Mathf.RoundToInt(luminosity * (_symbolCount - 1));
                idx1 = Mathf.Clamp(idx1, 0, 15);

                // second pixel → low nibble
                luminosity = Mathf.Clamp(pixels[i].PerceivedLuminosity() * _luminosityMultiplier, 0, 1);
                int idx2 = Mathf.RoundToInt(luminosity * (_symbolCount - 1));
                idx2 = Mathf.Clamp(idx2, 0, 15);

                // pack into one byte
                asciiPacked[i / 2] = (byte)((idx1 << 4) | idx2);
            }

            return asciiPacked;
        }

        private string ConvertToAsciiArtPacked(byte[] asciiPacked)
        {
            // two characters per byte
            var sb = new StringBuilder(asciiPacked.Length * 2);
            foreach (byte b in asciiPacked)
            {
                // high nibble
                int high = (b >> 4) & 0x0F;
                sb.Append(AsciiChars[high]);

                // low nibble
                int low = b & 0x0F;
                sb.Append(AsciiChars[low]);
            }
            return sb.ToString();
        }
        
        private string[] SplitByWidth(string text, int width)
        {
            int lineCount = (text.Length + width - 1) / width;
            string[] result = new string[lineCount];
    
            for (int i = 0; i < lineCount; i++)
            {
                int startIndex = i * width;
                int length = Mathf.Min(width, text.Length - startIndex);
                result[i] = text.Substring(startIndex, length);
            }
    
            return result;
        }
        
        byte[] Compress(byte[] data)
        {
            using var outputStream = new MemoryStream();
            // Using System.IO.Compression.CompressionLevel for DeflateStream
            using (var compressStream = new DeflateStream(outputStream, CompressionLevel.Optimal))
            {
                compressStream.Write(data, 0, data.Length);
            }
            return outputStream.ToArray();
        }
        
        byte[] Decompress(byte[] data)
        {
            using var inputStream = new MemoryStream(data);
            using var outputStream = new MemoryStream();
            using (var decompressStream = new DeflateStream(inputStream, CompressionMode.Decompress))
            {
                decompressStream.CopyTo(outputStream);
            }
            return outputStream.ToArray();
        }
    }
}
