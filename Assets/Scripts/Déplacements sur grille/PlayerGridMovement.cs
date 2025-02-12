using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Codice.CM.Client.Differences;
using UnityEngine.Tilemaps;

public class PlayerGridMovement : MonoBehaviour
{
    private Grid _grid; // référence au composant grid
    [SerializeField] private float _moveSpeed = 5f; // vitesse de déplacement
    [ShowNonSerializedField] private Vector2Int _gridPositionMemory; // position actuelle du joueur
    [SerializeField, Range(0f, 1f)] private float _moveDuration = 0.2f;
    private bool _isMoving = false; // empêche les déplacements simultanés
    private int _currentRotation = 0; // rotation actuelle (0 = haut, 90 = droite, etc.)

    [SerializeField, Layer] int _playzoneLayer;
    [SerializeField, Layer] int _objectLayer;

    private bool _isInAction = false;
    private bool _executeAction = false;

    readonly private Queue<ActionType> _actionQueue = new(); // file d'attente des actions
    private Oxygen _oxygenManager; // référence à l'oxygène

    public enum ActionType { Move, TurnRight, TurnLeft }

    void Start()
    {
        _grid = GameManager.Instance.PlayGrid;
        if (_grid == null)
        {
            Debug.LogError("Le Grid n'est pas assigné dans l'inspector.");
            return;
        }

        // récupérer le script oxygen
        _oxygenManager = GetComponent<Oxygen>();
        if (_oxygenManager == null)
        {
            Debug.LogError("Aucun script Oxygen trouvé dans la scène !");
            return;
        }

        // aligner le joueur sur une case de la grille

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        _gridPositionMemory = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = _grid.GetCellCenterWorld(cellPosition);
    }

    [Button]
    public void CheckPositionInGrid()
    {
        if (_grid == null)
        {
            var temp = GameObject.FindGameObjectWithTag("Playzone");
            _grid = temp.GetComponent<Grid>();
        }

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        _gridPositionMemory = new Vector2Int(cellPosition.x, cellPosition.y);
    }

    public void AddAction(ActionType action)
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
        }else if(_actionQueue.Count <= 0)
        {
            _executeAction = false;
        }
    }



    private void ExecuteActionQueue()
    {
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
        StartCoroutine(WaitTurn());

    }

    IEnumerator MoveCoroutine()
    {
        _isMoving = true;
        Vector2Int direction = GetDirectionVector();
        Vector2Int targetPosition = _gridPositionMemory + direction;

        Vector3 startPosition = transform.position;
        Vector3 targetPositionWorld = _grid.GetCellCenterWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0));

        if (IsNextGridCaseAValidDestination(targetPositionWorld)) // PROBLEME ICI
        {
            float elapsedTime = 0f;

            while (elapsedTime < _moveDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPositionWorld, elapsedTime / _moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPositionWorld;
            _gridPositionMemory = targetPosition;
            _isMoving = false;

            // consommer de l'oxygène après le déplacement
            _oxygenManager.LoseOxygen();
        }
        else 
        {
            _isMoving = false;
            yield return new WaitForSeconds(_moveDuration); 
        }
    }

    void TurnRight()
    {
        _currentRotation = (_currentRotation + 90) % 360;
        transform.rotation = Quaternion.Euler(0, 0, -_currentRotation);
    }

    void TurnLeft()
    {
        _currentRotation = (_currentRotation - 90 + 360) % 360; // éviter les valeurs négatives
        transform.rotation = Quaternion.Euler(0, 0, -_currentRotation);
    }

    Vector2Int GetDirectionVector()
    {
        if (_currentRotation == 0) return Vector2Int.up;
        if (_currentRotation == 90) return Vector2Int.right;
        if (_currentRotation == 180) return Vector2Int.down;
        if (_currentRotation == 270) return Vector2Int.left;
        return Vector2Int.up;
    }

    bool IsNextGridCaseAValidDestination(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast
            (
            origin: pos,
            direction: pos,
            distance: Mathf.Infinity
            );

        if(hit.collider != null)
        {
            if(hit.collider is TilemapCollider2D && hit.collider.gameObject.layer == _playzoneLayer)
            {
                return true;
            }
            if (hit.collider.gameObject.TryGetComponent(out GridObject obj))
            {
                if (obj.IsImpassable) { return false; }
                else { StartCoroutine(StartInteraction(obj)); return true; }
            }
        }
        else
        {
            _isInAction = false;
        }

        return false;
    }

    public static event Action<GridObject> OnInteraction;
    IEnumerator StartInteraction(GridObject obj)
    {
        yield return new WaitForSeconds(_moveDuration);
        OnInteraction?.Invoke(obj);        
    }

    private IEnumerator WaitTurn()
    {
        yield return new WaitForSeconds(_moveDuration);
        _isInAction = false;
    }
}