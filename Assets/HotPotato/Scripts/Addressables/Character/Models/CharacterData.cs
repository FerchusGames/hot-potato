using System;
using UnityEngine;

namespace Character.Models
{
    [Serializable]
    public class CharacterData : ICharacterData
    {
        [SerializeField] 
        private StyleNameData styleName;

        [SerializeField] 
        private string color;
        
        public CharacterData(StyleNameData styleName, string color)
        {
            this.styleName = styleName;
            this.color = color;
        }
        
        public string Color => color;
        public StyleNameData StyleName => styleName;
    }
}