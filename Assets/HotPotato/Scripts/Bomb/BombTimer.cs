using System;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using TMPro;
using HotPotato.Managers;

public class BombTimer : NetworkBehaviour
{
    public event Action OnTimerExpired; // TODO: Change this to event bus
    
    [SerializeField]
    private int _initialTime = 20;

    [SerializeField]
    private TextMeshProUGUI _text;
    
    private readonly SyncTimer _timer = new();
    private readonly SyncVar<bool> _isRunning = new(true);
    
    private GameManager _gameManager;
    
    private bool _timerExpired = false;
    
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
        if (!_isRunning.Value) 
        {
            _text.text = "END";
            return;
        }

        _timer.Update();
    
        if (IsClientStarted)
        {
            _text.text = _timer.Remaining > 0 ? _timer.Remaining.ToString("F2") : "BOOM!";
        }
        
        CheckTimer();
    }
    
    [Server]
    private void CheckTimer()
    {
        if (_timer.Remaining <= 0 && !_timerExpired)
        {
            _timerExpired = true;
            OnTimerExpired?.Invoke();
        }
    }
    
    [Server]
    private void ResetTimer()
    {
        _isRunning.Value = true;
        _timerExpired = false;
        _timer.StartTimer(_initialTime);
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void StopTimerObserversRpc()
    {
        StopTimer();
        StopTimerClientRpc();
    }

    [Server]
    private void StopTimer()
    {
        _isRunning.Value = false;
        _timer.StopTimer();
    }

    [ObserversRpc]
    private void StopTimerClientRpc()
    {
        _isRunning.Value = false;
    }
}