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

        private HashSet<int> _trapIndexes = new();
        private List<BombModuleSettings> _settingsList = new();
        
        private GameManager GameManager => base.NetworkManager.GetInstance<GameManager>();
        
        private int TotalModulesCount => _gridSize * _gridSize;
        private float ModuleScale => _unitaryScale / _gridSize;
        private float OffsetBetweenModules => _caseSize / _gridSize;
        private float FirstPositionOffset => -OffsetBetweenModules * 0.5f * (_gridSize - 1);

        public override void OnStartServer()
        {
            GameManager.OnRoundStarted += SpawnModuleGrid;
        }

        public override void OnStopServer()
        {
            GameManager.OnRoundStarted -= SpawnModuleGrid;
        }

        [Server]
        private void SpawnModuleGrid()
        {
            ClampTrapAmount();
            InitializeTrapIndexes();
            _settingsList.Clear();

            for (var column = 0; column < _gridSize; column++)
            {
                for (var row = 0; row < _gridSize; row++)
                {
                    SpawnAndConfigureModule(column, row);
                }
            }

            GameManager.SetCurrentRoundModuleSettings(_settingsList);
        }

        private void SpawnAndConfigureModule(int column, int row)
        {
            int moduleTypeIndex = GetRandomModulePropertyIndex();
            Vector3 position = GetModulePosition(column, row);
            GameObject bombModule = InstantiateBombModule(moduleTypeIndex, position);

            ConfigureBombModule(bombModule, moduleTypeIndex);
        }

        private void ConfigureBombModule(GameObject bombModule, int moduleTypeIndex)
        {
            BombModuleSettings currentSettings = GenerateSettings(_settingsList.Count, moduleTypeIndex);
            bombModule.GetComponent<BombModule>().SetSettings(currentSettings);
            _settingsList.Add(currentSettings);
        }

        private GameObject InstantiateBombModule(int moduleTypeIndex, Vector3 position)
        {
            GameObject bombModule = Instantiate(
                _bombModulePrefabs[moduleTypeIndex].gameObject,
                position,
                Quaternion.identity,
                _bombModuleParent
            );

            bombModule.transform.localScale = new Vector3(ModuleScale, 1, ModuleScale);
            bombModule.name = $"Bomb Module {position.x} {position.z}";
            base.Spawn(bombModule);

            return bombModule;
        }

        private BombModuleSettings GenerateSettings(int currentModuleIndex, int moduleTypeIndex) => new()
        {
            ModuleTypeIndex = moduleTypeIndex,
            ColorIndex = GetRandomModulePropertyIndex(),
            NumberIndex = GetRandomModulePropertyIndex(),
            LetterIndex = GetRandomModulePropertyIndex(),
            IsTrap = _trapIndexes.Contains(currentModuleIndex)
        };

        private void InitializeTrapIndexes()
        {
            _trapIndexes = new HashSet<int>();
            while (_trapIndexes.Count < _trapAmount)
            {
                _trapIndexes.Add(Random.Range(0, TotalModulesCount));
            }
        }

        private void ClampTrapAmount()
        {
            _trapAmount = Mathf.Min(_trapAmount, TotalModulesCount);
        }

        private int GetRandomModulePropertyIndex() => Random.Range(0, _bombModulePrefabs.Length);

        private Vector3 GetModulePosition(int row, int column) => new(
            FirstPositionOffset + row * OffsetBetweenModules,
            0,
            FirstPositionOffset + column * OffsetBetweenModules
        );
    }
}