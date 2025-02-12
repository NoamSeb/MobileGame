using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerGridMovement : MonoBehaviour
{
    public Grid grid; // référence au composant grid
    public float moveSpeed = 5f; // vitesse de déplacement
    public Vector2Int gridPosition; // position actuelle du joueur
    private bool isMoving = false; // empêche les déplacements simultanés
    private int currentRotation = 0; // rotation actuelle (0 = haut, 90 = droite, etc.)

    private Queue<ActionType> actionQueue = new Queue<ActionType>(); // file d'attente des actions
    private Oxygen oxygenManager; // référence à l'oxygène

    public enum ActionType { Move, TurnRight, TurnLeft }

    void Start()
    {
        if (grid == null)
        {
            Debug.LogError("Le Grid n'est pas assigné dans l'inspector.");
            return;
        }

        // récupérer le script oxygen
        oxygenManager = (Oxygen)FindFirstObjectByType(typeof(Oxygen));
        if (oxygenManager == null)
        {
            Debug.LogError("Aucun script Oxygen trouvé dans la scène !");
            return;
        }

        // aligner le joueur sur une case de la grille
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        gridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = grid.GetCellCenterWorld(cellPosition);
    }

    public void AddAction(ActionType action)
    {
        actionQueue.Enqueue(action);
    }

    public void ExecuteActions()
    {
        if (actionQueue.Count > 0 && !isMoving)
        {
            StartCoroutine(ExecuteActionQueue());
        }
    }

    IEnumerator ExecuteActionQueue()
    {
        while (actionQueue.Count > 0 && !oxygenManager.IsDead())
        {
            ActionType action = actionQueue.Dequeue();

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
        }
    }

    IEnumerator MoveCoroutine()
    {
        isMoving = true;
        Vector2Int direction = GetDirectionVector();
        Vector2Int targetPosition = gridPosition + direction;

        Vector3 startPosition = transform.position;
        Vector3 targetPositionWorld = grid.GetCellCenterWorld(new Vector3Int(targetPosition.x, targetPosition.y, 0));

        float elapsedTime = 0f;
        float moveDuration = 0.2f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPositionWorld, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPositionWorld;
        gridPosition = targetPosition;
        isMoving = false;

        // consommer de l'oxygène après le déplacement
        oxygenManager.LoseOxygen();
    }

    void TurnRight()
    {
        currentRotation = (currentRotation + 90) % 360;
        transform.rotation = Quaternion.Euler(0, 0, -currentRotation);
    }

    void TurnLeft()
    {
        currentRotation = (currentRotation - 90 + 360) % 360; // éviter les valeurs négatives
        transform.rotation = Quaternion.Euler(0, 0, -currentRotation);
    }

    Vector2Int GetDirectionVector()
    {
        if (currentRotation == 0) return Vector2Int.up;
        if (currentRotation == 90) return Vector2Int.right;
        if (currentRotation == 180) return Vector2Int.down;
        if (currentRotation == 270) return Vector2Int.left;
        return Vector2Int.up;
    }
}




