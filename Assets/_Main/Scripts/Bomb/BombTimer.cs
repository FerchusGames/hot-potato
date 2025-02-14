using UnityEngine;
using TMPro;

public class BombTimer : MonoBehaviour
{
    [SerializeField]
    private int _initialTime = 10;

    [SerializeField]
    private TextMeshProUGUI _text;

    private float _currentTime;

    private void Start()
    {
        _currentTime = _initialTime;
        _text.text = _currentTime.ToString("F2");
    }

    void Update()
    {
        _currentTime -= Time.deltaTime;
        _text.text = _currentTime.ToString("F2");

        if (_currentTime <= 0)
        {
            _text.text = "BOOM";
            enabled = false;
        }
    }
}
