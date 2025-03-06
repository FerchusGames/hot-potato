using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Bomb
{
    public class BombSpawner : NetworkBehaviour
    {
        [BoxGroup("Bomb Modules"), Tooltip("List of bomb module prefabs to spawn.")]
        [Required, SerializeField] private BombModule[] _bombModulePrefabs;
        
        [BoxGroup("Grid Settings"), Tooltip("Defines the size of the module grid (between 2 and 10).")]
        [Range(2, 10), SerializeField] private int _gridSize = 5;

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
           for (int i = 0; i < _gridSize; i++)
           {
               for (int j = 0; j < _gridSize; j++)
               {
                   Vector3 position = new Vector3(
                       GetFirstPositionOffset() + i * GetOffsetBetweenModules(),
                       0,
                       GetFirstPositionOffset() + j * GetOffsetBetweenModules()
                   );
        
                   GameObject bombModule = Instantiate(
                       _bombModulePrefabs[Random.Range(0, _bombModulePrefabs.Length)].gameObject,
                       position,
                       Quaternion.identity
                   );
                   
                   bombModule.transform.localScale = new Vector3(GetModuleScale(), 1, GetModuleScale());

                   BombModuleSettings settings = GetRandomSetting();
                   
                   bombModule.GetComponent<BombModule>().ApplySettings(settings);
                   
                   base.Spawn(bombModule);
               }
           }
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