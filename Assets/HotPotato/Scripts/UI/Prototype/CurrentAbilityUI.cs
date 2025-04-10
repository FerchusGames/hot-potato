using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HotPotato.UI.Prototype
{
    public class CurrentAbilityUI : MonoBehaviour
    {
        [Required]
        [SerializeField] private TextMeshProUGUI _text;
        
        private EventBinding<AbilitySelectedEvent> _abilitySelectedEventBinding;

        private void Start()
        {
            _abilitySelectedEventBinding = new EventBinding<AbilitySelectedEvent>(ShowAbility);
            EventBus<AbilitySelectedEvent>.Register(_abilitySelectedEventBinding);
        }

        private void OnDestroy()
        {
            EventBus<AbilitySelectedEvent>.Deregister(_abilitySelectedEventBinding);
        }
        
        private void ShowAbility(AbilitySelectedEvent abilitySelectedEvent)
        {
            var abilityName = abilitySelectedEvent.AbilityType.ToString();
            _text.text = abilityName.Replace("AbilityType.", "");
        }
    }
}