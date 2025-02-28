using UnityEngine;

public class GridLaserBlock : GridObject
{
    private Transform _playerPos;
    private float _killDistance;
    bool _isActive;

    public void SecondSetup(Transform pos, float _distance, int angle)
    {
        _playerPos = pos;
        _killDistance = _distance;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        GridLaserEmittor.OnActivate += Activate;
    }

    void Activate()
    {
        if (_isActive) { _isActive = false; return; }
        _isActive = true;
    }

    private void Update()
    {
        if (Vector3.Distance(_playerPos.position, transform.position) < _killDistance && _isActive)
        {
            GameManager.Instance.PlayerOxygen.StopPlayer();
        }
    }
}
