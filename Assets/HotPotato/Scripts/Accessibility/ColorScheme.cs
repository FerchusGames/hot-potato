using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Accessibility
{
    [CreateAssetMenu(fileName = "ColorScheme", menuName = "HotPotato/Accessibility/ColorScheme")]
    public class ColorScheme : ScriptableObject
    {
        [Required,  ValidateInput("HasFiveColors", "The color scheme must have exactly 5 colors.")]
        [SerializeField] private Color[] _colors = new Color[5];

        private bool HasFiveColors(Color[] colors)
        {
            return colors != null && colors.Length == 5;
        }
        
        public Color GetColor(int index)
        {
            index = Mathf.Clamp(index, 0, _colors.Length - 1);
            
            return _colors[index];
        }
    }
}