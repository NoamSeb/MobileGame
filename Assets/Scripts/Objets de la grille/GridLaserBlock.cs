using UnityEngine;
using System.Collections;

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
        PlayerGridMovement.OnActionExecuted += TryKillPlayer;
        _isActive = true;
    }

    void Activate()
    {
        if (_renderer != null)
        {
            if (_isActive) { _isActive = false; _renderer.color = _inactiveColor; return; }
            _isActive = true; _renderer.color = _baseColor;
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
