using System.Collections.Generic;
using FishNet.Object;
using HotPotato.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.Bomb
{
    public class BombSpawner : NetworkBehaviour
    {
        [BoxGroup("Bomb Settings"), Tooltip("Number of modules to mark as traps."), MinValue(3)]
        [SerializeField] private int _trapAmount = 3;
        
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
        
        private GameManager _gameManager;
        private HashSet<int> _trapIndexes;
        private List<BombModuleSettings> _settings;

        public override void OnStartNetwork()
        {
            _gameManager = base.NetworkManager.GetInstance<GameManager>();
        }

        public override void OnStartServer()
        {
            SpawnModuleGrid();
        }

        [Server]
        private void SpawnModuleGrid()
        {
            ClampTrapAmount();
            InitializeTrapIndexes();
            
            var currentModule = 0;

            _settings = new List<BombModuleSettings>();
            
            for (var column = 0; column < _gridSize; column++) 
            {
               for (var row = 0; row < _gridSize; row++)
               {
                   int moduleTypeIndex = GetRandomModuleTypeIndex();
                   
                   GameObject bombModule = Instantiate(
                       _bombModulePrefabs[moduleTypeIndex].gameObject,
                       GetModulePosition(column, row),
                       Quaternion.identity,
                       _bombModuleParent
                   );
                   
                   bombModule.transform.localScale = new Vector3(GetModuleScale(), 1, GetModuleScale());
                   bombModule.name = $"Bomb Module {column} {row}";
                   base.Spawn(bombModule);

                   BombModuleSettings currentSettings = GetSettings(currentModule);
                   currentSettings.ModuleTypeIndex = moduleTypeIndex;
                   
                   bombModule.GetComponent<BombModule>().SetSettings(currentSettings);
                   _settings.Add(currentSettings);
                   
                   currentModule++;
               }
            }
            
            _gameManager.SetCurrentRoundModuleSettings(_settings);
        }

        private void ClampTrapAmount()
        { 
            _trapAmount = Mathf.Min(_trapAmount, GetModuleCount());
        }

        private int GetRandomModuleTypeIndex()
        {
            return Random.Range(0, _bombModulePrefabs.Length);
        }

        private Vector3 GetModulePosition(int row, int column)
        {
            var position = new Vector3(
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
        
        private int GetModuleCount()
        {
            return _gridSize * _gridSize;
        }

        private BombModuleSettings GetSettings(int currentModuleIndex)
        {
            return new BombModuleSettings
            {
                ColorIndex = GetRandomSettingIndex(),
                NumberIndex = GetRandomSettingIndex(),
                LetterIndex = GetRandomSettingIndex(),
                IsTrap = _trapIndexes.Contains(currentModuleIndex)
            };
        }

        private int GetRandomSettingIndex()
        {
            return Random.Range(0, 5);
        }
        
        private void InitializeTrapIndexes()
        {
            _trapIndexes = new HashSet<int>();
            while (_trapIndexes.Count < _trapAmount)
            {
                _trapIndexes.Add(Random.Range(0, _gridSize * _gridSize));
            }
        }
    }

    public struct BombModuleSettings
    {
        public int ModuleTypeIndex;
        public int ColorIndex;
        public int NumberIndex;
        public int LetterIndex;
        public bool IsTrap;
    }
}