using HotPotato.Player;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI
{
    public class MatchResultUI : MonoBehaviour
    {
        [Required]
        [SerializeField] private Image _background;
        
        [Required]
        [SerializeField] private TextMeshProUGUI _resultText;

        private void Start()
        {
            OwnedPlayerManager.Instance.OnWinMatch += ShowWinResult;
            OwnedPlayerManager.Instance.OnLoseMatch += ShowLoseResult;
            OwnedPlayerManager.Instance.OnRoundStarted += HideResult;
            HideResult();
        }
        
        private void OnDestroy()
        {
            OwnedPlayerManager.Instance.OnWinMatch -= ShowWinResult;
            OwnedPlayerManager.Instance.OnLoseMatch -= ShowLoseResult;
            OwnedPlayerManager.Instance.OnRoundStarted -= HideResult;
        }

        private void ShowWinResult(int winCount)
        {
            gameObject.SetActive(true);
            _background.color = Color.yellow;
            _resultText.text = "You've won the match!";
        }
        
        private void ShowLoseResult()
        {
            gameObject.SetActive(true);
            _background.color = Color.red;
            _resultText.text = "You've lost the match!";
        }

        private void HideResult()
        {
            gameObject.SetActive(false);
        }
    }
}
