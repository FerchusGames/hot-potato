using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using HotPotato.Clues;
using HotPotato.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HotPotato.UI
{
    public class UIManager : NetworkBehaviour
    {
        [Required, AssetsOnly]
        [SerializeField] private ClueFieldUI _clueFieldUIPrefab;
        
        [Required, SceneObjectsOnly]
        [SerializeField] private Transform _clueFieldParent;

        [Required, SceneObjectsOnly] 
        [SerializeField] private Button _nextRoundButton;
        
        [Required, SceneObjectsOnly] 
        [SerializeField] private Button _newMatchButton;
        
        private Dictionary<BombClueType, Dictionary<int, int>> _clueTypeData;
        private List<ClueFieldUI> _clueFieldUIList = new();

        private GameManager GameManager => base.NetworkManager.GetInstance<GameManager>();

        public override void OnStartNetwork()
        {
            base.NetworkManager.RegisterInstance(this);
        }

        public override void OnStartServer()
        {
            GameManager.OnRoundEnded += ClearClueTypeData;
            GameManager.OnRoundEnded += ShowNextRoundButton;
            GameManager.OnRoundStarted += ShowNextRoundClues;
            GameManager.OnMatchEnded += ShowNextMatchButton;
        }
        
        public override void OnStopServer()
        {
            GameManager.OnRoundEnded -= ClearClueTypeData;
            GameManager.OnRoundEnded -= ShowNextRoundButton;
            GameManager.OnRoundStarted -= ShowNextRoundClues;
            GameManager.OnMatchEnded -= ShowNextMatchButton;
        }

        public override void OnStartClient()
        {
            if (IsHostInitialized) return;
            RequestClueTypeData(LocalConnection); // TODO: Prevent this from being called multiple times when rejoining
        }

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
        
        [Server]
        private void ClearClueTypeData()
        {
            _clueTypeData = null;
        }
        
        [ObserversRpc]
        private void ShowNextRoundClues()
        {
            foreach (var clueFieldUI in _clueFieldUIList)
            {
                Destroy(clueFieldUI.gameObject);
            }
            
            _clueFieldUIList.Clear();
            RequestClueTypeData(LocalConnection);
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
        
        private void ShowNextRoundButton()
        {
            if (IsHostInitialized) _nextRoundButton.gameObject.SetActive(true);
        }
        
        private void ShowNextMatchButton()
        {
            if (IsHostInitialized) _newMatchButton.gameObject.SetActive(true);
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
                
                _clueFieldUIList.Add(clueFieldUI);
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