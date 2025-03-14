using HotPotato.Player;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace HotPotato.UI
{
    public class WinCounterUI : MonoBehaviour
    {
        [Required]
        [SerializeField] private TextMeshProUGUI _text;
    
        private void Start()
        {
            OwnedPlayerManager.Instance.OnWinRound += UpdateWinRoundCount;
            OwnedPlayerManager.Instance.OnWinMatch += UpdateWinRoundCount;
        }

        private void OnDestroy()
        {
            OwnedPlayerManager.Instance.OnWinRound -= UpdateWinRoundCount;
            OwnedPlayerManager.Instance.OnWinMatch -= UpdateWinRoundCount;
        }

        private void UpdateWinRoundCount(int winCount)
        {
            _text.text = winCount.ToString();
        }
    }
}