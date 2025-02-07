using UnityEngine;
using System.Collections;
using log4net.Util;

public class PlayerGridMovement : MonoBehaviour
{
    public Grid grid; // Référence au composant Grid
    public float moveSpeed = 5f; // vitesse du déplacement
    public Vector2Int gridPosition; // position actuelle du joueur en coordonnées de grille
    private bool isMoving = false; // booléen pour éviter les déplacements en chaîne

    void Start()
    {
        if (grid == null)
        {
            Debug.LogError("Le Grid n'est pas assigné dans l'inspector.");
            return;
        }

        // aligner le joueur sur une case de la grille au démarrage
        Vector3Int cellPosition = grid.WorldToCell(transform.position);
        gridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = grid.GetCellCenterWorld(cellPosition);

        // s'abonner à l'événement de swipe
        SwipeMovement.OnSwipeEnd += HandleSwipe;
    }

    void OnDestroy()
    {
        SwipeMovement.OnSwipeEnd -= HandleSwipe;
    }

    void HandleSwipe(SwipeMovement.SwipeDirection direction)
    {
        if (isMoving) return;

        Vector2Int targetPosition = gridPosition;

        // déterminer la direction du déplacement en fonction du swipe
        switch (direction)
        {
            case SwipeMovement.SwipeDirection.Up:
                targetPosition += Vector2Int.up;
                break;
            case SwipeMovement.SwipeDirection.Down:
                targetPosition += Vector2Int.down;
                break;
            case SwipeMovement.SwipeDirection.Left:
                targetPosition += Vector2Int.left;
                break;
            case SwipeMovement.SwipeDirection.Right:
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
        float elapsedTime = 0f;
        float moveDuration = 0.2f;

        // angle de rotation vers la nouvelle direction
        Vector3 direction = targetPosition - startPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); 

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }
}



