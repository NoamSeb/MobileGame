using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public class PlayerGridMovement : MonoBehaviour
{
    private Grid _grid; // référence au composant grid
    [SerializeField] private float _moveSpeed = 5f; // vitesse de déplacement
    [ShowNonSerializedField] private Vector2Int _gridPosition; // position actuelle du joueur
    private bool _isMoving = false; // empêche les déplacements simultanés
    private int _currentRotation = 0; // rotation actuelle (0 = haut, 90 = droite, etc.)

    private Queue<ActionType> _actionQueue = new Queue<ActionType>(); // file d'attente des actions
    private Oxygen _oxygenManager; // référence à l'oxygène

    public enum ActionType { Move, TurnRight, TurnLeft, Wait }

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
        _gridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = _grid.GetCellCenterWorld(cellPosition);
    }

    public void AddAction(ActionType action)
    {
        _actionQueue.Enqueue(action);
    }

    public void ExecuteActions()
    {
        if (_actionQueue.Count > 0 && !_isMoving)
        {
            StartCoroutine(ExecuteActionQueue());
        }
    }

    IEnumerator ExecuteActionQueue()
    {
        while (_actionQueue.Count > 0 && !_oxygenManager.IsDead())
        {
            ActionType action = _actionQueue.Dequeue();

            if (action == ActionType.Move)
            {
                yield return StartCoroutine(MoveCoroutine());
            }
            else if (action == ActionType.TurnRight)
            {
                TurnRight();
                yield return new WaitForSeconds(0.2f);
            }
            else if (action == ActionType.TurnLeft)
            {
                TurnLeft();
                yield return new WaitForSeconds(0.2f);
            }
            if (action == ActionType.Wait)
            {
                yield return StartCoroutine(WaitCoroutine());
            }
        }
    }

    IEnumerator MoveCoroutine()
    {
        _isMoving = true;
        Vector2Int direction = GetDirectionVector();
        Vector2Int targetPosition = _gridPosition + direction;

        Vector3 startPosition = transform.position;
        Vector3 targetPositionWorld = _grid.GetCellCenterWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0));

        float elapsedTime = 0f;
        float moveDuration = 0.2f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPositionWorld, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPositionWorld;
        _gridPosition = targetPosition;
        _isMoving = false;

        // consommer de l'oxygène après le déplacement
        _oxygenManager.LoseOxygen();
    }
    IEnumerator WaitCoroutine()
    {
        _isMoving = true;
        yield return new WaitForSeconds(0.2f); // durée d'attente équivalente à un déplacement
        _isMoving = false;
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
}




