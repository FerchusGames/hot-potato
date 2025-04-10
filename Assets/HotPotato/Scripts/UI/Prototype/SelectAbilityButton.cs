using HotPotato.AbilitySystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI.Prototype
{
    public class SelectAbilityButton : MonoBehaviour
    {
        [Required]
        [SerializeField] private Button _button;
        
        [SerializeField] private AbilityType _abilityType;
        
        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            EventBus<AbilitySelectRequestedEvent>.Raise(new AbilitySelectRequestedEvent
            {
                AbilityType = _abilityType,
            });
        }
    }
}