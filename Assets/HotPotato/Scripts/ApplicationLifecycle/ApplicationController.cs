using HotPotato.Accessibility;
using HotPotato.Utilities;
using UnityEngine;

namespace HotPotato.ApplicationLifecycle
{
    public class ApplicationController : Singleton<ApplicationController>
    {
        public Color[] ColorScheme { get; private set; } = AccessibilitySettings.DefaultColors;
    }
}