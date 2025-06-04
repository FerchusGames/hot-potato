using System;
using UnityEngine;

namespace Character.Models
{
    [Serializable]
    public class StyleNameData : IStyleName
    {
        [SerializeField] private string id;
        [SerializeField] private string styleName;
        [SerializeField] private int priority;

        public StyleNameData(string id, string styleName, int priority)
        {
            this.id = id;
            this.styleName = styleName;
            this.priority = priority;
        }
        
        public string Id => id;
        public string StyleName => styleName;
        public int Priority => priority;
        
        
    }
}