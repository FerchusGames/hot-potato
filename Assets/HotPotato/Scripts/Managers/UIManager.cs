using System;
using HotPotato.Utilities;

namespace HotPotato.Managers
{
    public class UIManager : Singleton<UIManager>
    {
        public event Action<bool> OnTurnOwnerChanged;
        
        public void TriggerIsMyOwnerChanged(bool isOwner)
        {
            OnTurnOwnerChanged?.Invoke(isOwner);
        }
    }
}