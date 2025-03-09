using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using TMPro;
using HotPotato.Managers;

public class BombTimer : NetworkBehaviour
{
    [SerializeField]
    private int _initialTime = 10;

    [SerializeField]
    private TextMeshProUGUI _text;
    
    private readonly SyncTimer _timer = new();
    
    private GameManager _gameManager;

    public override void OnStartNetwork()
    {
        _gameManager = base.NetworkManager.GetInstance<GameManager>();
    }
    
    public override void OnStartServer()
    {
        _gameManager.OnTurnChanged += ResetTimer;
    }

    public override void OnStopServer()
    {
        _gameManager.OnTurnChanged -= ResetTimer;
    }

    private void Update()
    {
        _timer.Update();
        
        if (IsClientStarted)
        {
            _text.text = _timer.Remaining > 0 ? _timer.Remaining.ToString("F2") : "BOOM!";
        }
    }
    
    [Server]
    private void ResetTimer()
    {
        _timer.StartTimer(_initialTime);
    }
}