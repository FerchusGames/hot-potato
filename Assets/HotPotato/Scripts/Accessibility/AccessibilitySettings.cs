using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Accessibility
{
    [CreateAssetMenu(fileName = "AccessibilitySettings", menuName = "HotPotato/Accessibility/Settings")]
    public class AccessibilitySettings : ScriptableObject
    {
        [Required, InlineEditor] 
        [SerializeField] private ColorScheme _colorScheme;
        
        public ColorScheme ColorScheme => _colorScheme;
    }
}