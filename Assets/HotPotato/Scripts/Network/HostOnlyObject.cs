using FishNet.Object;

namespace HotPotato.Network
{
    public class HostOnlyObject : NetworkBehaviour
    {
        public override void OnStartClient()
        {
            gameObject.SetActive(false);

            if (IsHostInitialized)
            {
                gameObject.SetActive(true);
            }
        }
    }
}