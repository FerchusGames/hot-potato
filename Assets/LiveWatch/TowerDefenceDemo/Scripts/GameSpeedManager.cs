using System;
using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public class GameSpeedManager : MonoBehaviour
    {
        public event Action SpeedChanged;
        
        public SpeedMode CurrentSpeed
        {
            get => _currentSpeed;
            protected set
            {
                if (_currentSpeed == value)
                    return;

                _currentSpeed = value;
                SpeedChanged?.Invoke();
            }
        }

        private SpeedMode _currentSpeed;
        
        public void SetDefaultSpeed()
        {
            SetGameSpeed(SpeedMode.x1);
        }
        
        public void SetGameSpeed(SpeedMode mode)
        {
            Time.timeScale = mode switch
            {
                SpeedMode.x1 => 1,
                SpeedMode.x2 => 2,
                SpeedMode.x4 => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

            CurrentSpeed = mode;
        }
    }
     
    public enum SpeedMode
    {
        x1,
        x2,
        x4
    }
}