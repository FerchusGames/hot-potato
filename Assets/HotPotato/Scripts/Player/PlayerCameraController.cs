using FishNet.Object;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace HotPotato.Player
{
    public class PlayerCameraController : NetworkBehaviour
    {
        [Required]
        [SerializeField] private CinemachineCamera _onTurnCamera;
        
        [Required]
        [SerializeField] private CinemachineCamera _notOnTurnCamera;

        public override void OnStartClient()
        {
            if (!IsOwner)
            {
                gameObject.SetActive(false);
                return;
            }
            
            SetCameraAsLive(_onTurnCamera);
        }

        private static void SetCameraAsLive(CinemachineCamera camera)
        {
            camera.enabled = false;
            camera.enabled = true;
        }
    }
}