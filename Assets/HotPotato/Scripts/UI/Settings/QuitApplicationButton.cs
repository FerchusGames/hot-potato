using UnityEngine;

namespace HotPotato.UI.Settings
{
    public class QuitApplicationButton : MenuButton
    {
        protected override void Awake()
        {
            base.Awake();
            _button.onClick.AddListener(QuitApplication);
        }

        private void QuitApplication()
        {
            Debug.Log("Quitting application");
            Application.Quit();
        }
    }
}