using System;
using UnityEngine;

namespace Character.Views
{
    public interface ICharacterView
    {
        SpriteRenderer SpriteRenderer { get; }
        string Color { get; set; }
        Transform Transform { get; }
        bool FlipSprite { get; set; }
        Vector2 Direction { get; }
    }
}