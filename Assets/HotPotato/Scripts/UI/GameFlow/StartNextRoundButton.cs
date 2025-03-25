using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI.GameFlow
{
    public class StartNextRoundButton : MonoBehaviour
    {
        [Required]
        [SerializeField] private Button _button;
        
        private void Awake()
        {
            _button.onClick.AddListener(StartNextRound);
        }
        
        [Server]
        private void StartNextRound()
        {
            EventBus<StartNextRoundEvent>.Raise(new StartNextRoundEvent());
            gameObject.SetActive(false);
        }
    }
}