using HotPotato.Player;
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
            OwnedPlayerManager.Instance.OnIsMyTurnUpdate += SetTurnOwner;
        }
        
        private void OnDestroy()
        {
            OwnedPlayerManager.Instance.OnIsMyTurnUpdate -= SetTurnOwner;
        }

        private void SetTurnOwner(bool isOwner)
        {
            _image.color = isOwner ? Color.green : Color.red;
        }
    }
}