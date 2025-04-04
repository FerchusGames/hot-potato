using DG.Tweening;
using FishNet.Component.Transforming;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Bomb
{
    public class BombMover : NetworkBehaviour    
    {
        EventBinding<MoveBombToPlayerEvent> _moveBombToPlayerRotationEventBinding;

        [MinValue(1f)]
        [SerializeField] private float _moveDuration = 1f;
        
        [SerializeField] private Ease _ease = Ease.InOutSine;
        
        [Required]
        [SerializeField] private NetworkTransform _conveyorBeltNetworkTransform;
        
        public override void OnStartServer()
        {
            _moveBombToPlayerRotationEventBinding = new EventBinding<MoveBombToPlayerEvent>(MoveBombToPlayerRotation);
            EventBus<MoveBombToPlayerEvent>.Register(_moveBombToPlayerRotationEventBinding);
        }
        
        public override void OnStopServer()
        {
            EventBus<MoveBombToPlayerEvent>.Deregister(_moveBombToPlayerRotationEventBinding);
        }

        private void MoveBombToPlayerRotation(MoveBombToPlayerEvent moveBombToPlayerEvent)
        {
            var lookAtVector = CalculateLookAtVector(moveBombToPlayerEvent.PlayerPosition);
            var targetYAngle = CalculateTargetYAngle(lookAtVector);
            StartRotationTween(targetYAngle);
        }

        private Vector3 CalculateLookAtVector(Vector3 playerPosition)
        {
            return new Vector3(
                -playerPosition.x,
                transform.position.y,
                -playerPosition.z);
        }

        private float CalculateTargetYAngle(Vector3 lookAtVector)
        {
            var targetRotation = Quaternion.LookRotation(lookAtVector - transform.position);
            var currentYAngle = transform.rotation.eulerAngles.y;
            var targetYAngle = targetRotation.eulerAngles.y;
    
            if (targetYAngle < currentYAngle)
                targetYAngle += 360f;
        
            return targetYAngle;
        }

        private void StartRotationTween(float targetYAngle)
        {
            Quaternion previousRotation = transform.rotation;
    
            transform.DORotate(new Vector3(0, targetYAngle, 0), _moveDuration, RotateMode.FastBeyond360)
                .SetEase(_ease)
                .OnUpdate(() => UpdateConveyorBeltRotation(ref previousRotation))
                .OnComplete(() => EventBus<BombHasReachedPlayerEvent>.Raise(new BombHasReachedPlayerEvent()));
        }

        private void UpdateConveyorBeltRotation(ref Quaternion previousRotation)
        {
            Quaternion rotationDelta = transform.rotation * Quaternion.Inverse(previousRotation);
            _conveyorBeltNetworkTransform.transform.rotation *= rotationDelta;
            previousRotation = transform.rotation;
        }
    }
}