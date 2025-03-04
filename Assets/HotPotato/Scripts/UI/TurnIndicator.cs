using HotPotato.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI
{
    public class TurnIndicator : MonoBehaviour
    {
        [SerializeField, Required] private Image _image;

        private void Start()
        {
            UIManager.Instance.OnTurnOwnerChanged += SetTurnOwner;
        }
        
        private void OnDestroy()
        {
            UIManager.Instance.OnTurnOwnerChanged -= SetTurnOwner;
        }

        private void SetTurnOwner(bool isOwner)
        {
            _image.color = isOwner ? Color.green : Color.red;
        }
    }
}