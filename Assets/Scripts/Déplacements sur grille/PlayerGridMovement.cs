using UnityEngine;
using System.Collections;

public class PlayerGridMovement : MonoBehaviour
{
    public Grid grid; // référence au composant grid
    public float moveSpeed = 5f; // vitesse de déplacement
    public Vector2Int gridPosition; // position actuelle du joueur sur la grille
    private bool isMoving = false; // empêche plusieurs déplacements en même temps

    private Oxygen oxygenManager; // référence au script oxygen

    void Start()
    {
        if (grid == null)
        {
            Debug.LogError("le grid n'est pas assigné dans l'inspector.");
            return;
        }

        // récupérer le script oxygen pour gérer la consommation d'oxygène
        oxygenManager = (Oxygen)FindFirstObjectByType(typeof(Oxygen));

        if (oxygenManager == null)
        {
            Debug.LogError("aucun script oxygen trouvé dans la scène !");
            return;
        }

        // aligner le joueur sur une case de la grille au démarrage
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        gridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = grid.GetCellCenterWorld(cellPosition);

        // s'abonner à l'événement de swipe
        CodeblockMovement.OnMoveInstructed += HandleSwipe;
    }

    void OnDestroy()
    {
        CodeblockMovement.OnMoveInstructed -= HandleSwipe;
    }

    void HandleSwipe(CodeblockMovement.MoveInstruction direction)
    {
        if (isMoving || (oxygenManager != null && oxygenManager.IsDead())) return; // empêcher le déplacement si en mouvement ou si le joueur est mort

        Vector2Int targetPosition = gridPosition;

        // déterminer la direction du déplacement en fonction du swipe
        switch (direction)
        {
            case CodeblockMovement.MoveInstruction.Up:
                targetPosition += Vector2Int.up;
                break;
            case CodeblockMovement.MoveInstruction.Down:
                targetPosition += Vector2Int.down;
                break;
            case CodeblockMovement.MoveInstruction.Left:
                targetPosition += Vector2Int.left;
                break;
            case CodeblockMovement.MoveInstruction.Right:
                targetPosition += Vector2Int.right;
                break;
        }

        // mettre à jour la position et lancer l'animation du déplacement
        gridPosition = targetPosition;
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = grid.GetCellCenterWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0));

        // angle de rotation vers la nouvelle direction
        Vector3 direction = targetPosition - startPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        float elapsedTime = 0f;
        float moveDuration = 0.2f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;

        // consommer de l'oxygène après chaque déplacement
        if (oxygenManager != null)
        {
            oxygenManager.LoseOxygen();
        }
    }
}



