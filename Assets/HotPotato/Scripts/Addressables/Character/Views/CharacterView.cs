using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Views
{
    public class CharacterView : MonoBehaviour, ICharacterView
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Vector2 direction;

        public SpriteRenderer SpriteRenderer => spriteRenderer;
        public Transform Transform => transform;
       
        public bool FlipSprite
        {
            get => spriteRenderer.flipX;
            set => spriteRenderer.flipX = value;
        }

        public string Color
        {
            get => spriteRenderer.color.ToString();
            set => spriteRenderer.color = GetColorFromHex(value);
        }

        private Color GetColorFromHex(string value)
        {
            Color color = UnityEngine.Color.white;
            ColorUtility.TryParseHtmlString(value, out color);
            return color;
        }
        
        public Vector2 Direction => direction;

        public void SetDirection(InputAction.CallbackContext ctx)
        {
            direction = ctx.ReadValue<Vector2>();
        }
    }
}