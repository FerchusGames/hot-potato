using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class ClickableObject : MonoBehaviour
    {
        public event Action Clicked;
        
        private void OnMouseDown()
        {
            Clicked?.Invoke();
        }
    }
}