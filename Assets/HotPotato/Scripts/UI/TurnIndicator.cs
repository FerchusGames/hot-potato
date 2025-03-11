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
            OwnedPlayerManager.Instance.OnLose += Lose;
            OwnedPlayerManager.Instance.OnWin += Win;
        }
        
        private void OnDestroy()
        {
            OwnedPlayerManager.Instance.OnIsMyTurnUpdate -= SetTurnOwner;
            OwnedPlayerManager.Instance.OnLose -= Lose;
            OwnedPlayerManager.Instance.OnWin -= Win;
        }

        private void SetTurnOwner(bool isOwner)
        {
            _image.color = isOwner ? Color.green : Color.red;
        }

        private void Lose()
        {
            _image.color = Color.black;
        }

        private void Win()
        {
            _image.color = Color.yellow;
        }
    }
}