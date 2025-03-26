using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine.Tilemaps;
using TMPro;
using MoreMountains.Feedbacks;
using System.Threading.Tasks;

public class PlayerGridMovement : MonoBehaviour
{
    private Grid _grid; // r�f�rence au composant grid
    [ShowNonSerializedField] private Vector2Int _gridPosition; // position actuelle du joueur
    [SerializeField, Range(0f, 1f)] private float _moveDuration = 0.2f;
    public float MoveDuration { get { return _moveDuration; } }
    private bool _isMoving = false; // emp�che les d�placements simultan�s
    private int _currentRotation = 0; // rotation actuelle (0 = haut, 90 = droite, etc.)
    public int CurrentRotation { get { return _currentRotation; } }

    public enum InitialMoveDirection
    {
        Up,
        Left,
        Down,
        Right
    }

    [SerializeField] private InitialMoveDirection _initialMoveDirection;
    private void OnValidate()
    {
        SetCorrectDirection();
    }

    void SetCorrectDirection()
    {
        switch (_initialMoveDirection)
        {
            case InitialMoveDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                _currentRotation = 90;
                break;
            case InitialMoveDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                _currentRotation = 180;
                break;
            case InitialMoveDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                _currentRotation = 270;
                break;
            case InitialMoveDirection.Up:
                transform.rotation = Quaternion.identity;
                _currentRotation = 0;
                break;
        }

        if (FindFirstObjectByType<PlayerMirrorMovement>() != null)
        {
            FindFirstObjectByType<PlayerMirrorMovement>().MatchPlayerRotation(_initialMoveDirection);
        }
    }

    [SerializeField, Layer] int _playzoneLayer;

    private bool _isInAction = false;
    private bool _executeAction = false;

    readonly private Queue<ActionType> _actionQueue = new(); // file d'attente des actions
    private Oxygen _oxygenManager; // r�f�rence � l'oxyg�ne

    public enum ActionType { Move, TurnRight, TurnLeft, Wait }

    private bool _isRotationLocked;

    private PlayerMirrorMovement _playerMirror;

    [SerializeField] private MMF_Player _loadingFeedbacks;

    int deltaOxygen;

    private void Awake()
    {
        _loadingFeedbacks.Initialization();
    }

    void Start()
    {
        _grid = GameManager.Instance.PlayGrid;
        if (_grid == null)
        {
            Debug.LogError("Le Grid n'est pas assign� dans l'inspector.");
            return;
        }

        // r�cup�rer le script oxygen
        _oxygenManager = GetComponent<Oxygen>();
        if (_oxygenManager == null)
        {
            Debug.LogError("Aucun script Oxygen trouv� dans la sc�ne !");
            return;
        }

        // aligner le joueur sur une case de la grille

        SetPositionInGrid();
        SetCorrectDirection();

        GridTeleporter.OnTeleport += Teleport;
        GridPusher.OnPush += Push;
        GridRotationLocker.OnRotate += ForceRotation;

        if (GameManager.Instance.MirrorScript != null)
        {
            _playerMirror = GameManager.Instance.MirrorScript;
        }

        PlayFeedbacks(_loadingFeedbacks);

        deltaOxygen = _oxygenManager.CurrentOxygen;
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

    public void AddAction(ActionType action)
    {
        _actionQueue.Enqueue(action);
        if (_playerMirror != null) { _playerMirror.AddAction(action); }
    }

    public void ExecuteActions()
    {
        _executeAction = true;
        if (_playerMirror != null) { _playerMirror.ExecuteActions(); }
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

    public static event Action OnActionExecuted;
    private void ExecuteActionQueue()
    {
        deltaOxygen = _oxygenManager.CurrentOxygen;
        OnActionExecuted?.Invoke();
        ActionType action = _actionQueue.Dequeue();

        if (action == ActionType.Move)
        {
            StartCoroutine(MoveCoroutine());
        }
        else if (action == ActionType.TurnRight)
        {
            TurnRight();
        }
        else if (action == ActionType.TurnLeft)
        {
            TurnLeft();
        }
        else if (action == ActionType.Wait)
        {
            StartCoroutine(WaitCoroutine());
        }
        StartCoroutine(WaitTurn());
    }

    IEnumerator MoveCoroutine()
    {
        if ((_oxygenManager.CurrentOxygen >= 5 && deltaOxygen <= 4))
        {
            yield return new WaitForSeconds(_moveDuration);
        }

        _isMoving = true;
        Vector2Int direction = GetDirectionVector();
        Vector2Int targetPosition = _gridPosition + direction;

        Vector3 startPosition = transform.position;
        Vector3 targetPositionWorld = _grid.GetCellCenterWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0));

        if (IsNextGridCaseAValidDestination(targetPositionWorld))
        {
            float elapsedTime = 0f;

            while (elapsedTime < _moveDuration * 4f / 5f)
            {
                transform.position = Vector3.Lerp(startPosition, targetPositionWorld, elapsedTime / _moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPositionWorld;
            _gridPosition = targetPosition;

            _oxygenManager.LoseOxygen();

            if ((_oxygenManager.CurrentOxygen <= 4 && deltaOxygen >= 5))
            {
                yield return new WaitForSeconds(_moveDuration);
            }

            if (_oxygenManager.CurrentOxygen == 0)
            {
                _actionQueue.Clear();
            }

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
        yield return new WaitForSeconds(_moveDuration); // dur�e d'attente �quivalente � un d�placement
        _isMoving = false;
    }

    void TurnRight()
    {
        if (!_isRotationLocked)
        {
            _currentRotation = (_currentRotation - 90 + 360) % 360;
            transform.rotation = Quaternion.Euler(0, 0, _currentRotation);
        }
    }

    void TurnLeft()
    {
        if (!_isRotationLocked)
        {
            _currentRotation = (_currentRotation + 90) % 360;
            transform.rotation = Quaternion.Euler(0, 0, _currentRotation);
        }
    }

    Vector2Int GetDirectionVector()
    {
        if (_currentRotation == 0) return Vector2Int.up;
        if (_currentRotation == 270) return Vector2Int.right;
        if (_currentRotation == 180) return Vector2Int.down;
        if (_currentRotation == 90) return Vector2Int.left;
        throw new ArgumentException("The player's rotation doesn't match this script's");
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
                    if (map != null && map.gameObject.layer == _playzoneLayer)
                    {
                        hasGroundBeenDetected = true;
                    }
                }
            }
        }

        if (hasGroundBeenDetected) { return true; }
        return false;
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


    bool _isTeleporting;
    async void Teleport(Vector3Int pos)
    {
        _isTeleporting = true;
        await TeleportMovement(pos);
        _isTeleporting = false;
    }

    async Task TeleportMovement(Vector3Int pos)
    {
        _isMoving = true;
        Vector3 targetPos = _grid.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
        await Task.Delay(Mathf.FloorToInt(_moveDuration * 1000f / 2f));
        transform.position = targetPos;
        _gridPosition = (Vector2Int)pos;
        _isMoving = false;
    }

    void Push(Vector2Int direction)
    {
        StartCoroutine(PushMovement(direction));
    }

    IEnumerator PushMovement(Vector2Int direction)
    {
        _isMoving = true;
        Vector2Int targetPosition = _gridPosition + direction;

        Vector3 startPosition = transform.position;
        Vector3 targetPositionWorld = _grid.GetCellCenterWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0));

        if (IsNextGridCaseAValidDestination(targetPositionWorld))
        {
            OnActionExecuted?.Invoke();
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
        }
        else
        {
            _isMoving = false;
            yield return new WaitForSeconds(_moveDuration);
        }
    }

    void ForceRotation(int rotation)
    {
        _currentRotation = rotation;
        transform.rotation = Quaternion.Euler(0, 0, _currentRotation);
        _isRotationLocked = true;
    }

    public void StopMovement()
    {
        _isMoving = false;
        _executeAction = false;
        Debug.Log("Le joueur ne bouge plus !");

        _actionQueue.Clear();
    }

    public void DisableActions()
    {
        _executeAction = false;
        _actionQueue.Clear();
    }

    void PlayFeedbacks(MMF_Player player)
    {
        player.PlayFeedbacks();
    }

    private void OnDestroy()
    {
        GridTeleporter.OnTeleport -= Teleport;
        GridPusher.OnPush -= Push;
        GridRotationLocker.OnRotate -= ForceRotation;
    }
}