using FishNet.Object;
using HotPotato.Managers;
using HotPotato.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace HotPotato.Bomb
{
    public class BombModule : NetworkBehaviour, IPointerClickHandler
    {
        [FormerlySerializedAs("_material")] [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private TextMeshProUGUI _text;
            
        private GameManager _gameManager;
        
        private static Color[] _bombColors = 
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.white
        };
        
        public override void OnStartClient()
        {
            _gameManager = base.NetworkManager.GetInstance<GameManager>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _gameManager.InteractWithModuleServerRpc(this);
            OwnedPlayerManager.Instance.DisableModuleInteractivity();
        }
        
        public void ApplySettings(BombModuleSettings settings)
        {
            _meshRenderer.material.color = _bombColors[settings.ColorIndex];
            _text.text = GetModuleText(settings);
        }
        
        private string GetModuleText(BombModuleSettings settings)
        {
            return GetNumberStringFromIndex(settings.NumberIndex) + GetLetterStringFromIndex(settings.LetterIndex);
        }
        
        private string GetNumberStringFromIndex(int index)
        {
            return (index + 1).ToString();
        }
        
        private string GetLetterStringFromIndex(int index)
        {
            return ((char)('A' + index)).ToString();
        }
    }
}