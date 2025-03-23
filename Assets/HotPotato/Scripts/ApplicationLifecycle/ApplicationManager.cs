using HotPotato.Accessibility;
using HotPotato.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.ApplicationLifecycle
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        [Required, InlineEditor]
        [SerializeField] private AccessibilitySettings _accessibilitySettings;
         
        public AccessibilitySettings AccessibilitySettings => _accessibilitySettings;
    }
}