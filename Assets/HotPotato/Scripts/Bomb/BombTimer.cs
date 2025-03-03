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
    
    private readonly SyncVar<float> _currentTime = new();

    private float _lastClientUpdateTime = 0f;

    public override void OnStartClient()
    {
        base.OnStartClient();
        _currentTime.OnChange += OnTimerChanged;
    }

    public override void OnStopClient()
    {
        _currentTime.OnChange -= OnTimerChanged;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameManager.OnTurnChanged += ResetTimer;
    }

    public override void OnStopServer()
    {
        GameManager.OnTurnChanged -= ResetTimer;
    }

    private void Update()
    {
        if (IsServerStarted)
        {
            _currentTime.Value -= Time.deltaTime;
        }
        else if (IsClientStarted)
        {
            _lastClientUpdateTime -= Time.deltaTime;
            _text.text = _lastClientUpdateTime.ToString("F2");
            
            if (Mathf.Abs(_currentTime.Value - _lastClientUpdateTime) > 0.5f)
            {
                _lastClientUpdateTime = _currentTime.Value;
            }
        }
        if (_currentTime.Value <= 0)
        {
            _text.text = "BOOM!";
        }
    }

    private void OnTimerChanged(float prev, float next, bool asServer)
    {
        _lastClientUpdateTime = next;
        _text.text = next.ToString("F2");
    }

    private void ResetTimer()
    {
        if (IsServerStarted)
        {
            _currentTime.Value = _initialTime;
            enabled = true;
        }
    }
}
