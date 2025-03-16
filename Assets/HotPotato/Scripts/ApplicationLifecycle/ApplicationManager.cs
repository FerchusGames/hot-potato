using HotPotato.Accessibility;
using HotPotato.Infrastructure;
using HotPotato.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.ApplicationLifecycle
{
    public class ApplicationManager : MonoBehaviour
    {
        [Required, InlineEditor]
        [SerializeField] private AccessibilitySettings _accessibilitySettings;
         
        public AccessibilitySettings AccessibilitySettings => _accessibilitySettings;
        
        private void Awake()
        {
            GameServiceLocator.RegisterApplicationManager(this);
        }
    }
}