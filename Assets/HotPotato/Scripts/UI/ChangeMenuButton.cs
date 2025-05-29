using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI
{
    [RequireComponent(typeof(Button))]
    public class ChangeMenuButton : MonoBehaviour
    {
        [SerializeField, Required] private GameObject _currentMenu;
        [SerializeField, Required] private GameObject _targetMenu;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ChangeMenu);
        }
        
        private void ChangeMenu()
        {
            _targetMenu.SetActive(true);
            _currentMenu.SetActive(false);
        }
    }
}