using UnityEngine;

public class GridLaserBlock : GridObject
{
    private Transform _playerPos;
    private float _killDistance;
    bool _isActive;

    SpriteRenderer _renderer;
    Color _inactiveColor;
    Color _baseColor;

    public void SecondSetup(Transform pos, float _distance, int angle, Color disabledColor)
    {
        _renderer = GetComponent<SpriteRenderer>();
        _baseColor = _renderer.color;
        _inactiveColor = disabledColor;

        _playerPos = pos;
        _killDistance = _distance;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        GridLaserEmittor.OnActivate += Activate;
        _isActive = true;
    }

    void Activate()
    {
        if (_isActive) { _isActive = false; _renderer.color = _inactiveColor; return; }
        _isActive = true; _renderer.color = _baseColor;
    }

    private void Update()
    {
        if (Vector3.Distance(_playerPos.position, transform.position) < _killDistance && _isActive)
        {
            GameManager.Instance.PlayerOxygen.StopPlayer();
        }
    }
}
