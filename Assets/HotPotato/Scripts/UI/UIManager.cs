using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using HotPotato.Clues;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HotPotato.UI
{
    public class UIManager : NetworkBehaviour
    {
        [Required, AssetsOnly]
        [SerializeField] private ClueFieldUI _clueFieldUIPrefab;
        
        [Required, SceneObjectsOnly]
        [SerializeField] private Transform _clueFieldParent;
        
        private Dictionary<BombClueType, Dictionary<int, int>> _clueTypeData;

        [Server]
        public void SetClueData(ClueData clueData)
        {
            _clueTypeData = new Dictionary<BombClueType, Dictionary<int, int>>
            {
                { BombClueType.Number, clueData.ModuleNumberData },
                { BombClueType.Color, clueData.ModuleColorData },
                { BombClueType.Type, clueData.ModuleTypeData },
                { BombClueType.Letter, clueData.ModuleLetterData }
            };
        }

        public override void OnStartNetwork()
        {
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartClient()
        {
            RequestClueTypeData(LocalConnection); // TODO: Prevent this from being called multiple times when rejoining
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestClueTypeData(NetworkConnection requestingClient)
        {
            RequestClueTypeDataAsync(requestingClient).Forget();
        }

        [Server]
        private async UniTaskVoid RequestClueTypeDataAsync(NetworkConnection requestingClient)
        {
            while (_clueTypeData == null)
            {
                await UniTask.Yield();
            }
            
            var availableClueTypes = _clueTypeData.Keys.ToList();
            var clueType = availableClueTypes[Random.Range(0, availableClueTypes.Count)];
            Dictionary<int, int> clueTypeDictionary = _clueTypeData[clueType];

            _clueTypeData.Remove(clueType);
            
            InstantiateCluesUI(requestingClient, clueType, clueTypeDictionary);
        }
        
        [ObserversRpc]
        private void DebugLogObserversRpc()
        {
            Debug.Log("ObserversRpc called");
        }
        
        [TargetRpc]
        private void InstantiateCluesUI(NetworkConnection connection, BombClueType clueType, Dictionary<int, int> clueTypeData)
        {
            foreach (var clue in clueTypeData.OrderBy(kvp => kvp.Key))
            {
                var clueFieldUI = Instantiate(
                    _clueFieldUIPrefab.gameObject, 
                    Vector3.zero, 
                    Quaternion.identity, 
                    _clueFieldParent
                    ).GetComponent<ClueFieldUI>();
                
                clueFieldUI.Initialize(clueType, clue);
            }
        }
    }
    
    public enum BombClueType
    {
        Number,
        Color,
        Type,
        Letter
    }
}