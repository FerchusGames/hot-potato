using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI.GameFlow
{
    public class StartNextMatchButton : MonoBehaviour
    {
        [Required]
        [SerializeField] private Button _button;
        
        private void Awake()
        {
            _button.onClick.AddListener(StartNextMatch);
        }
        
        [Server]
        private void StartNextMatch()
        {
            EventBus<StartNextMatchEvent>.Raise(new StartNextMatchEvent());
            gameObject.SetActive(false);
        }
    }
}