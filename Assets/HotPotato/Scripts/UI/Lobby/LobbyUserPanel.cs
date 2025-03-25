using Heathen.SteamworksIntegration;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI.Lobby
{
    public class LobbyUserPanel : MonoBehaviour
    {
        [Required]
        [SerializeField] private RawImage _avatarImage;
        
        [Required]
        [SerializeField] private TextMeshProUGUI _userNameText;
        
        public void Initialize(UserData userData)
        {
            userData.LoadAvatar(SetAvatar);
            _userNameText.text = userData.Name;
        }
        
        private void SetAvatar(Texture2D avatarImage)
        {
            _avatarImage.texture = avatarImage;
        }
    }
}