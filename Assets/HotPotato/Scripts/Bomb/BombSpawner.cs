using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Bomb
{
    public class BombSpawner : NetworkBehaviour
    {
        [BoxGroup("Bomb Modules"), Tooltip("List of bomb module prefabs to spawn."), Required, AssetsOnly]
        [SerializeField] private BombModule[] _bombModulePrefabs;

        [BoxGroup("Bomb Modules"), Tooltip("GameObject the modules will spawn in."), Required, SceneObjectsOnly]
        [SerializeField] private Transform _bombModuleParent;
        
        [BoxGroup("Grid Settings"), Tooltip("Defines the size of the module grid (between 2 and 10).")]
        [SerializeField, Range(2, 10)] private int _gridSize = 5;

        [BoxGroup("Grid Settings"), Tooltip("Determines the scale of each module.")]
        [SerializeField] private float _unitaryScale = 10f;

        [BoxGroup("Grid Settings"), Tooltip("Determines the spacing between modules.")]
        [SerializeField] private float _caseSize = 0.5f;
        
        public override void OnStartServer()
        {
            SpawnModuleGrid();
        }

        [Server]
        private void SpawnModuleGrid()
        {
           for (int column = 0; column < _gridSize; column++)
           {
               for (int row = 0; row < _gridSize; row++)
               {
                   GameObject bombModule = Instantiate(
                       GetRandomModule(),
                       GetModulePosition(column, row),
                       Quaternion.identity,
                       _bombModuleParent
                   );
                   
                   bombModule.transform.localScale = new Vector3(GetModuleScale(), 1, GetModuleScale());
                   bombModule.name = $"Bomb Module {column} {row}";
                   base.Spawn(bombModule);
                   bombModule.GetComponent<BombModule>().SetSettings(GetRandomSetting());
               }
           }
        }

        private GameObject GetRandomModule()
        {
            return _bombModulePrefabs[Random.Range(0, _bombModulePrefabs.Length)].gameObject;
        }

        private Vector3 GetModulePosition(int row, int column)
        {
            Vector3 position = new Vector3(
                GetFirstPositionOffset() + row * GetOffsetBetweenModules(),
                0,
                GetFirstPositionOffset() + column * GetOffsetBetweenModules()
            );
            return position;
        }

        private float GetFirstPositionOffset()
        {
            return -GetOffsetBetweenModules() * 0.5f * (_gridSize - 1);
        }

        private float GetOffsetBetweenModules()
        {
            return _caseSize / _gridSize;
        }
        
        private float GetModuleScale()
        {
            return _unitaryScale / _gridSize;
        }

        private BombModuleSettings GetRandomSetting()
        {
            return new BombModuleSettings
            {
                ColorIndex = GetRandomSettingIndex(),
                NumberIndex = GetRandomSettingIndex(),
                LetterIndex = GetRandomSettingIndex()
            };
        }

        private int GetRandomSettingIndex()
        {
            return Random.Range(0, 5);
        }
    }

    public struct BombModuleSettings
    {
        public int ColorIndex;
        public int NumberIndex;
        public int LetterIndex;
    }
}