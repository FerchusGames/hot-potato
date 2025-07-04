﻿using System.Collections.Generic;
using FishNet.Object;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace HotPotato.Bomb
{
    public class BombSpawner : NetworkBehaviour
    {
        [BoxGroup("Bomb Settings"), Tooltip("Number of modules to mark as traps."), MinValue(3)]
        [SerializeField] private int _trapAmount = 3;

        [BoxGroup("Bomb Modules"), Tooltip("List of bomb module prefabs to spawn."), Required, AssetsOnly]
        [SerializeField] private BombModule[] _bombModulePrefabs;

        [BoxGroup("Bomb Modules"), Tooltip("GameObject the modules will spawn in."), Required]
        [SerializeField] private NetworkObject _bombModuleParent;

        [BoxGroup("Grid Settings"), Tooltip("Defines the size of the module grid (between 2 and 10).")]
        [SerializeField, Range(2, 10)] private int _gridSize = 5;

        [BoxGroup("Grid Settings"), Tooltip("Determines the scale of each module.")]
        [SerializeField] private float _unitaryScale = 20f;

        [BoxGroup("Grid Settings"), Tooltip("Determines the spacing between modules.")]
        [SerializeField] private float _caseSize = 0.5f;

        private HashSet<int> _trapIndexes = new();
        private List<BombModuleSettings> _settingsList = new();
        
        private EventBinding<RoundStartedEvent> _roundStartedEventBinding;
        
        private int TotalModulesCount => _gridSize * _gridSize;
        private float ModuleScale => _unitaryScale * _caseSize / _gridSize;
        private float OffsetBetweenModules => _caseSize / _gridSize;
        private float FirstPositionOffset => -OffsetBetweenModules * 0.5f * (_gridSize - 1);

        public override void OnStartServer()
        {
            _roundStartedEventBinding = new EventBinding<RoundStartedEvent>(SpawnModuleGrid);
            EventBus<RoundStartedEvent>.Register(_roundStartedEventBinding);
        }

        public override void OnStopServer()
        {
            EventBus<RoundStartedEvent>.Deregister(_roundStartedEventBinding);
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

            EventBus<ModulesSpawnedEvent>.Raise(new ModulesSpawnedEvent
            {
                SettingsList = _settingsList
            });
            
            ModulesSettingsListCreated(_settingsList);
        }

        [ObserversRpc]
        private void ModulesSettingsListCreated(List<BombModuleSettings> settingsList)
        {
            EventBus<ModulesSettingsListCreatedEvent>.Raise(new ModulesSettingsListCreatedEvent
            {
                SettingsList = settingsList
            });
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
            GameObject bombModule = Instantiate(_bombModulePrefabs[moduleTypeIndex].gameObject);
            
            NetworkObject networkObject = bombModule.GetComponent<NetworkObject>();
            
            networkObject.SetParent(_bombModuleParent);
            networkObject.name = $"Bomb Module {position.x} {position.z}";
            networkObject.transform.localRotation = Quaternion.identity;
            networkObject.transform.localPosition = position;
            networkObject.transform.localScale = new Vector3(ModuleScale, 1, ModuleScale);
            
            base.Spawn(networkObject);

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

        private Vector3 GetModulePosition(int column, int row) => new(
            FirstPositionOffset + column * OffsetBetweenModules,
            0,
            FirstPositionOffset + row * OffsetBetweenModules
        );
    }
}