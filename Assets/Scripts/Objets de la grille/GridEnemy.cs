using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class GridEnemy : GridObject
{
    [ShowNonSerializedField] private float _moveDuration;
    [ShowNonSerializedField] private int _currentRotation = 0;
    [SerializeField] private GameObject _sleepingEnemy;

    [SerializeField] private Sprite _verticalSprite, _horizontalSprite;
    private SpriteRenderer _skin;

    public enum MovementType
    {
        Vertical,
        Horizontal
    }
    [SerializeField] private MovementType _movementType;
    bool IsVertical() { return _movementType == MovementType.Vertical; }
    bool IsHorizontal() { return _movementType == MovementType.Horizontal; }

    public enum VerticalInitialDir
    {
        Up,
        Down
    }
    [SerializeField, ShowIf(nameof(IsVertical))] private VerticalInitialDir _verticalInitialDirection;

    public enum HorizontalInitialDir
    {
        Right,
        Left
    }
    [SerializeField, ShowIf(nameof(IsHorizontal))] private HorizontalInitialDir _horizontalInitialDirection;

    public void TransferMovementParams(MovementType movement, VerticalInitialDir dir)
    {
        _movementType = movement;
        _verticalInitialDirection = dir;
    }

    public void TransferMovementParams(MovementType movement, HorizontalInitialDir dir)
    {
        _movementType = movement;
        _horizontalInitialDirection = dir;
    }

    protected override void Setup()
    {
        base.Setup();
        _skin = GetComponent<SpriteRenderer>();
        _moveDuration = GameManager.Instance.PlayerScript.MoveDuration * 4f / 5f;
        _sleepingEnemy = Resources.Load<GameObject>("GDTools Prefabs/Grid Objects/EnemySleep");
        PlayerGridMovement.OnActionExecuted += StartMovement;
        Oxygen.OnOverOxygenThreshold += ReturnToMimir;
        SetupRotation();
        InvertRotation();
    }

    void SetupRotation()
    {
        switch (_movementType)
        {
            case MovementType.Vertical:
                _skin.sprite = _verticalSprite;
                switch (_verticalInitialDirection)
                {
                    case VerticalInitialDir.Up:
                        _currentRotation = 0; break;
                    case VerticalInitialDir.Down:
                        _currentRotation = 180; break;
                }
                break;
            case MovementType.Horizontal:
                _skin.sprite = _horizontalSprite;
                switch (_horizontalInitialDirection)
                {
                    case HorizontalInitialDir.Right:
                        _currentRotation = 90; break;
                    case HorizontalInitialDir.Left:
                        _currentRotation = 270; break;
                }
                break;
        }
    }

    void StartMovement()
    {
        BugFix();
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        InvertRotation();
        Vector2Int direction = GetDirectionVector();
        Vector2Int targetPosition = (Vector2Int)_gridPosition + direction;

        Vector3 startPosition = transform.position;
        Vector3 targetPositionWorld = _grid.GetCellCenterWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0));

        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPositionWorld, elapsedTime / _moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPositionWorld;
        _gridPosition = (Vector3Int)targetPosition;
    }

    void InvertRotation()
    {
        switch (_currentRotation)
        {
            case 0:
                _currentRotation = 180; break;
            case 90:
                _currentRotation = 270; break;
            case 180:
                _currentRotation = 0; break;
            case 270:
                _currentRotation = 90; break;
        }
    }

    bool IsNextGridCaseAValidDestination(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, pos, Mathf.Infinity);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.TryGetComponent(out GridObject obj))
            {
                if (obj.IsImpassable) { return false; }
                return true;
            }
            return true;
        }
        return false;
    }

    Vector2Int GetDirectionVector()
    {
        if (_currentRotation == 0) return Vector2Int.up;
        if (_currentRotation == 90) return Vector2Int.right;
        if (_currentRotation == 180) return Vector2Int.down;
        if (_currentRotation == 270) return Vector2Int.left;
        return Vector2Int.up;
    }

    private void Update()
    {
        KillPlayerIfOverThem();
    }

    void KillPlayerIfOverThem()
    {
        if (Vector3.Distance(GameManager.Instance.PlayerScript.transform.position, transform.position) < .1f)
        {
            GameManager.Instance.PlayerOxygen.StopPlayer();
        }
    }

    bool _isDuringOnDestroy;

    private void OnDestroy()
    {
        _isDuringOnDestroy = true;
        ReturnToMimir();
    }

    void ReturnToMimir()
    {
        PlayerGridMovement.OnActionExecuted -= StartMovement;
        Oxygen.OnOverOxygenThreshold -= ReturnToMimir;
        if (!_isDuringOnDestroy) { StartCoroutine(ReturnToSleep()); }
    }

    IEnumerator ReturnToSleep()
    {
        yield return new WaitForSeconds(.1f);
        if (GameManager.Instance.PlayerOxygen.CurrentOxygen >= 5)
        {
            CorrectDirectionOnFallingAsleep();
            GridSleepingEnemy temp = Instantiate(_sleepingEnemy, transform.position, Quaternion.identity, transform.parent).GetComponent<GridSleepingEnemy>();
            if (IsVertical()) { temp.TransferMovementParams(_movementType, _verticalInitialDirection); }
            else { temp.TransferMovementParams(_movementType, _horizontalInitialDirection); }
            
            Destroy(gameObject);
        }
    }

    void CorrectDirectionOnFallingAsleep()
    {
        if (_movementType == MovementType.Vertical)
        {
            _verticalInitialDirection = _currentRotation switch
            {
                0 => VerticalInitialDir.Down,
                180 => VerticalInitialDir.Up,
                _ => throw new ArgumentException($"{name}'s script-side rotation is invalid")
            };
        }
        if (_movementType == MovementType.Horizontal)
        {
            _horizontalInitialDirection = _currentRotation switch
            {
                90 => HorizontalInitialDir.Left,
                270 => HorizontalInitialDir.Right,
                _ => throw new ArgumentException($"{name}'s script-side rotation is invalid")
            };
        }
    }
}
