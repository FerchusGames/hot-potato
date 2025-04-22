using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class MobWaypointMover : MonoBehaviour
    {
        public float Speed;
        public float RotationSpeed = 5f;
        public float TravelledDist { get; private set; }
        
        private const float ArriveThreshold = 0.01f;
        
        private int _currentPointIndex;
        private bool _isMoving;
        private List<Transform> _waypoints;

        private Transform nextPoint => _waypoints[_currentPointIndex];
        private Vector2 currentPosition => new Vector2(transform.position.x, transform.position.z);
        private Vector2 pointPosition => new Vector2(nextPoint.position.x, nextPoint.position.z);
        
        public void StartMoving(List<Transform> waypoints)
        {
            _isMoving = true;
            _currentPointIndex = 0;
            _waypoints = waypoints;
            TravelledDist = 0;
        }

        public void StopMoving()
        {
            _isMoving = false;
        }

        private void Update()
        {
            if (!_isMoving)
                return;

            UpdateTargetPoint();
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdateTargetPoint()
        {
            var sqrDist =  (currentPosition - pointPosition).sqrMagnitude;

            if (sqrDist <= ArriveThreshold * ArriveThreshold)
                _currentPointIndex = Mathf.Clamp(_currentPointIndex + 1, 0, _waypoints.Count - 1);
        }

        private void UpdatePosition()
        {
            var vector = pointPosition - currentPosition;
            var distLeft = (pointPosition - currentPosition).magnitude;
            
            if (distLeft < ArriveThreshold)
                return;
            
            var direction = vector / distLeft;
            var moveDelta = Mathf.Clamp(Speed * Time.deltaTime, 0, distLeft);
            var vectorMoveDelta = moveDelta * new Vector3(direction.x, 0, direction.y);

            transform.position += vectorMoveDelta;
            TravelledDist += moveDelta;
        }

        private void UpdateRotation()
        {
            var vector = pointPosition - currentPosition;
            var distLeft = (pointPosition - currentPosition).magnitude;
            
            if (distLeft < ArriveThreshold)
                return;

            var directionCurrent = new Vector2(transform.forward.x, transform.forward.z);
            var directionTarget = vector;
            var directionNew = Vector2.Lerp(directionCurrent, directionTarget, Time.unscaledDeltaTime * RotationSpeed);

            transform.forward = new Vector3(directionNew.x, 0, directionNew.y);
        }
    }
}