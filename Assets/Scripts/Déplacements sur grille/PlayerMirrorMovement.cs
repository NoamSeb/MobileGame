using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.Tilemaps;
using TMPro;
using MoreMountains.Feedbacks;

public class PlayerMirrorMovement : MonoBehaviour
{
    private Grid _grid;
    [ShowNonSerializedField] private Vector2Int _gridPosition;
    private float _moveDuration;
    private bool _isMoving = false;
    private int _currentRotation = 0;

    public void MatchPlayerRotation(PlayerGridMovement.InitialMoveDirection playerDirection)
    {
        switch (playerDirection)
        {
            case PlayerGridMovement.InitialMoveDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, -90); // Inverse la rotation
                _currentRotation = 270;
                break;
            case PlayerGridMovement.InitialMoveDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                _currentRotation = 180;
                break;
            case PlayerGridMovement.InitialMoveDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, 90); // Inverse la rotation
                _currentRotation = 90;
                break;
            case PlayerGridMovement.InitialMoveDirection.Up:
                transform.rotation = Quaternion.identity;
                _currentRotation = 0;
                break;
        }
    }

    [SerializeField, Layer] int _mirrorLayer;

    private bool _isInAction = false;
    private bool _executeAction = false;

    readonly private Queue<PlayerGridMovement.ActionType> _actionQueue = new();

    private bool _isRotationLocked;

    [SerializeField] private MMF_Player _loadingFeedbacks;

    private void Awake()
    {
        _loadingFeedbacks.Initialization();
    }

    void Start()
    {
        _grid = GameManager.Instance.PlayGrid;
        if (_grid == null)
        {
            Debug.LogError("Le Grid n'est pas assigné dans l'inspector.");
            return;
        }

        SetPositionInGrid();

        _moveDuration = GameManager.Instance.PlayerScript.MoveDuration;

        PlayFeedbacks(_loadingFeedbacks);
    }

    [ExecuteInEditMode]
    public void SetPositionInGrid()
    {
        if (_grid == null)
        {
            var temp = GameObject.FindGameObjectWithTag("Playzone");
            _grid = temp.GetComponent<Grid>();
        }

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        _gridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = _grid.GetCellCenterWorld(cellPosition);
    }

    public void AddAction(PlayerGridMovement.ActionType action)
    {
        _actionQueue.Enqueue(action);
    }

    public void ExecuteActions()
    {
        _executeAction = true;
    }

    private void Update()
    {
        if (!_executeAction) { return; }
        if (_actionQueue.Count > 0 && !_isMoving && !_isInAction)
        {
            _isInAction = true;
            ExecuteActionQueue();
        }
        else if (_actionQueue.Count <= 0)
        {
            _executeAction = false;
            StopMovement();
        }
    }

    private void ExecuteActionQueue()
    {
        PlayerGridMovement.ActionType action = _actionQueue.Dequeue();

        if (action == PlayerGridMovement.ActionType.Move)
        {
            StartCoroutine(MoveCoroutine());
        }
        else if (action == PlayerGridMovement.ActionType.TurnRight)
        {
            TurnLeft(); // Inversion de la rotation
        }
        else if (action == PlayerGridMovement.ActionType.TurnLeft)
        {
            TurnRight(); // Inversion de la rotation
        }
        else if (action == PlayerGridMovement.ActionType.Wait)
        {
            StartCoroutine(WaitCoroutine());
        }
        StartCoroutine(WaitTurn());
    }

    IEnumerator MoveCoroutine()
    {
        _isMoving = true;
        Vector2Int direction = GetDirectionVector();
        Vector2Int targetPosition = _gridPosition + direction;

        Vector3 startPosition = transform.position;
        Vector3 targetPositionWorld = _grid.GetCellCenterWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0));

        if (IsNextGridCaseAValidDestination(targetPositionWorld))
        {
            float elapsedTime = 0f;

            while (elapsedTime < _moveDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPositionWorld, elapsedTime / _moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPositionWorld;
            _gridPosition = targetPosition;
            _isMoving = false;
            _isRotationLocked = false;
        }
        else
        {
            yield return new WaitForSeconds(_moveDuration);
            _isMoving = false;
        }
    }

    IEnumerator WaitCoroutine()
    {
        _isMoving = true;
        yield return new WaitForSeconds(_moveDuration);
        _isMoving = false;
    }

    void TurnRight()
    {
        if (!_isRotationLocked)
        {
            _currentRotation = (_currentRotation - 90 + 360) % 360; // Inversion du sens de rotation
            transform.rotation = Quaternion.Euler(0, 0, _currentRotation);
        }
    }

    void TurnLeft()
    {
        if (!_isRotationLocked)
        {
            _currentRotation = (_currentRotation + 90) % 360; // Inversion du sens de rotation
            transform.rotation = Quaternion.Euler(0, 0, _currentRotation);
        }
    }

    Vector2Int GetDirectionVector()
    {
        switch (_currentRotation)
        {
            case 0: return Vector2Int.up;     
            case 90: return Vector2Int.left;  
            case 180: return Vector2Int.down; 
            case 270: return Vector2Int.right;
        }
        throw new ArgumentException("Rotation invalide pour le Mirror Player !");
    }

    bool IsNextGridCaseAValidDestination(Vector3 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(pos);

        bool hasGroundBeenDetected = false;

        if (colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out GridObject obj))
                {
                    if (obj != null && !obj.IsImpassable)
                    {
                        StartCoroutine(StartInteraction(obj));
                    }
                    else if (obj.IsImpassable) { return false; }
                }
                if (collider.TryGetComponent(out Tilemap map))
                {
                    if (map != null && map.gameObject.layer == _mirrorLayer)
                    {
                        hasGroundBeenDetected = true;
                    }
                }
            }
        }

        return hasGroundBeenDetected;
    }

    public static event Action<GridObject> OnInteraction;
    IEnumerator StartInteraction(GridObject obj)
    {
        yield return new WaitForSeconds(_moveDuration);
        OnInteraction?.Invoke(obj);
    }

    IEnumerator WaitTurn()
    {
        yield return new WaitForSeconds(_moveDuration);
        _isInAction = false;
    }

    public void StopMovement()
    {
        _isMoving = false;
        _executeAction = false;
        _actionQueue.Clear();
        Debug.Log("Le joueur miroir ne bouge plus !");
    }

    void PlayFeedbacks(MMF_Player player)
    {
        player.PlayFeedbacks();
    }
}
