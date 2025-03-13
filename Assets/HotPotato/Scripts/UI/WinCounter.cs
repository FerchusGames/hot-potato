using HotPotato.Player;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HotPotato.UI
{
    public class WinCounter : MonoBehaviour
    {
        [Required]
        [SerializeField] private TextMeshProUGUI _text;
    
        private void Start()
        {
            OwnedPlayerManager.Instance.OnWin += UpdateWinCount;
        }

        private void OnDestroy()
        {
            OwnedPlayerManager.Instance.OnWin -= UpdateWinCount;
        }

        private void UpdateWinCount(int winCount)
        {
            _text.text = winCount.ToString();
        }
    }
}