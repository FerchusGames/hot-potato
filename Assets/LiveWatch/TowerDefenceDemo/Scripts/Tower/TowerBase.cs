using UnityEngine;

namespace Ingvar.LiveWatch.TowerDefenceDemo
{
    public abstract class TowerBase : MonoBehaviour
    {
        public string Id { get; set; }
        public abstract TowerType Type { get; }
        public abstract void Enable(LevelScene levelScene);
        public abstract void Disable();
    }
}