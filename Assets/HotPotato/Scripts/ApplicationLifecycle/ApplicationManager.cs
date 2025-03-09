using HotPotato.Accessibility;
using HotPotato.Utilities;
using UnityEngine;

namespace HotPotato.ApplicationLifecycle
{
    public class ApplicationManager : Singleton<ApplicationManager>
    {
        public Color[] ColorScheme { get; private set; } = AccessibilitySettings.DefaultColors;
    }
}