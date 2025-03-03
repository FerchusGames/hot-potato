using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;
using TMPro;
using HotPotato.Managers;

public class BombTimer : NetworkBehaviour
{
    [SerializeField]
    private int _initialTime = 10; // Initial countdown time in seconds

    [SerializeField]
    private TextMeshProUGUI _text; // UI element for displaying time

    private readonly SyncVar<float> _currentTime = new(); // Synced countdown time

    private float _lastClientUpdateTime = 0f; // Last time the client updated

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
            // Client-side approximation (interpolation)
            _lastClientUpdateTime -= Time.deltaTime;
            _text.text = _lastClientUpdateTime.ToString("F2");

            // If client gets too far behind the server, force a correction
            if (Mathf.Abs(_currentTime.Value - _lastClientUpdateTime) > 0.5f)
            {
                _lastClientUpdateTime = _currentTime.Value; // Hard sync correction
            }
        }
        if (_currentTime.Value <= 0)
        {
            _text.text = "BOOM!";
        }
    }

    private void OnTimerChanged(float prev, float next, bool asServer)
    {
        _lastClientUpdateTime = next; // Keep client in sync with the server
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
