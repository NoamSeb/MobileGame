using UnityEngine;
using System.Collections;

public class GridLaserBlock : GridObject
{
    private Transform _playerPos;
    private float _killDistance;
    bool _isActive;

    GridLaserEmittor _emittor;
    SpriteRenderer _renderer;
    Color _inactiveColor;
    Color _baseColor;

    public void SecondSetup(Transform pos, float _distance, int angle, Color disabledColor, GridLaserEmittor emittor)
    {
        _emittor = emittor;

        _renderer = GetComponent<SpriteRenderer>();
        _baseColor = _renderer.color;
        _inactiveColor = disabledColor;

        _playerPos = pos;
        _killDistance = _distance;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        GridLaserEmittor.OnActivate += Activate;
        PlayerGridMovement.OnActionExecuted += TryKillPlayer;
        _isActive = true;
    }

    void Activate(GridLaserEmittor emittor)
    {
        if (_renderer != null && emittor == _emittor)
        {
            if (_isActive)
            {
                _renderer.color = _inactiveColor;
                _isActive = false;
            }
            else
            {
                _renderer.color = _baseColor;
                _isActive = true;
            }
        }
    }

    void TryKillPlayer()
    {
        StartCoroutine(KillPlayerIfOver());
    }

    IEnumerator KillPlayerIfOver()
    {
        yield return new WaitForSeconds(GameManager.Instance.PlayerScript.MoveDuration);
        if (Vector3.Distance(_playerPos.position, transform.position) < _killDistance && _isActive)
        {
            GameManager.Instance.PlayerOxygen.StopPlayer();
        }
    }

    private void OnDestroy()
    {
        GridLaserEmittor.OnActivate -= Activate;
        PlayerGridMovement.OnActionExecuted -= TryKillPlayer;
    }
}
