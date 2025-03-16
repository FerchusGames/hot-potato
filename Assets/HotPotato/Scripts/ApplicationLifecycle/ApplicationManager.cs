using HotPotato.Accessibility;
using HotPotato.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.ApplicationLifecycle
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        [Required]
        [SerializeField] private AccessibilitySettings _accessibilitySettings;
         
        public ColorScheme ColorScheme => _accessibilitySettings.ColorScheme;
    }
}