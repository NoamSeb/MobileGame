using System;
using UnityEngine;

public class GridLaserEmittor : GridObject
{
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioSource _audioSource;
    enum LaserState
    {
        Activating,
        Activated,
        Deactivating,
        Deactivated
    }
    [SerializeField] private LaserState _state;
    [SerializeField, Range(0.1f, 1f)] private float _killDistance;
    [SerializeField, Range(2, 10)] private int _laserLength;
    private bool _isActivated;
    private Transform _playerPos;

    public enum Direction
    {
        Up,
        Left,
        Down,
        Right
    }

    [SerializeField] Direction _direction;
    private Vector3Int _setupDirection;
    private int _rotation;

    private void OnValidate()
    {
        switch (_direction)
        {
            case Direction.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                _rotation = 90;
                _setupDirection = Vector3Int.left;
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                _rotation = 180;
                _setupDirection = Vector3Int.down;
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                _rotation = -90;
                _setupDirection = Vector3Int.right;
                break;
            case Direction.Up:
                transform.rotation = Quaternion.identity;
                _rotation = 0;
                _setupDirection = Vector3Int.up;
                break;
        }
    }

    protected override void Setup()
    {
        base.Setup();
        _playerPos = GameManager.Instance.PlayerScript.transform;
        PlayerGridMovement.OnActionExecuted += UpdateLaserState;

        GameObject laserBlock = (GameObject)Resources.Load("Other Prefabs/LaserBlock");
        Vector3Int tempGridPos = GridPosition + _setupDirection;
        for (int i = 1; i < _laserLength; i++)
        {
            GameObject tempBlock = Instantiate(laserBlock, _grid.GetCellCenterWorld(tempGridPos), Quaternion.identity);
            tempBlock.GetComponent<GridLaserBlock>().SecondSetup(_playerPos, _killDistance, _rotation);
            tempGridPos += _setupDirection;
        }
    }

    public static event Action OnActivate;
    void UpdateLaserState()
    {
        switch (_state)
        {
            case LaserState.Activating:
                _state = LaserState.Activated; 
                _isActivated = true; 
                OnActivate?.Invoke();
                _audioSource.PlayOneShot(_laserSound);
                break;

            case LaserState.Activated:
                _state = LaserState.Deactivating; 
                break;

            case LaserState.Deactivating:
                _state = LaserState.Deactivated; 
                _isActivated = true;
                OnActivate?.Invoke();
                break;

            case LaserState.Deactivated:
                _state = LaserState.Activating; 
                break;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(_playerPos.position, transform.position) < _killDistance && _isActivated)
        {
            GameManager.Instance.PlayerOxygen.StopPlayer();
        }
    }
}
