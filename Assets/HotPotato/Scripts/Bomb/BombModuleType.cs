using FishNet.Object;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace HotPotato.Bomb
{
    public class BombModuleType : NetworkBehaviour
    {
        [SerializeField, Required] private Outline _outline;

        private void Activate()
        {
            _outline.enabled = true;
        }
        
        private void Deactivate()
        {
            _outline.enabled = false;
        }
    }
}