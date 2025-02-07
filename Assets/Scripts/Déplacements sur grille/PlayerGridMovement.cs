using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerGridMovement : MonoBehaviour
{
    public TilemapCollider2D playzoneCollider; // Référence au TilemapCollider2D
    private Vector2Int minBounds;
    private Vector2Int maxBounds;

    public float moveSpeed = 5f; // vitesse de déplacement
    public Vector2Int gridPosition; // position actuelle du joueur sur la grille
    private bool isMoving = false;

    void Start()
    {
        if (playzoneCollider != null)
        {
            // récupérer les limites de "Playzone"
            Bounds bounds = playzoneCollider.bounds;
            minBounds = new Vector2Int(Mathf.FloorToInt(bounds.min.x), Mathf.FloorToInt(bounds.min.y));
            maxBounds = new Vector2Int(Mathf.CeilToInt(bounds.max.x), Mathf.CeilToInt(bounds.max.y));
        }
        else
        {
            Debug.LogError("Playzone Collider non assigné dans l'Inspector !");
        }

        // placer le joueur au centre d’une case
        transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);
        SwipeMovement.OnSwipeEnd += HandleSwipe;
    }

    void OnDestroy()
    {
        SwipeMovement.OnSwipeEnd -= HandleSwipe;
    }

    void HandleSwipe(SwipeMovement.SwipeDirection direction)
    {
        if (isMoving) return; // éviter un autre déplacement pendant l'animation

        Vector2Int targetPosition = gridPosition;

        // déterminer la direction du mouvement
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
            default:
                return;
        }

        // vérifier que la position cible est bien dans "Playzone"
        if (targetPosition.x >= minBounds.x && targetPosition.x < maxBounds.x &&
            targetPosition.y >= minBounds.y && targetPosition.y < maxBounds.y)
        {
            gridPosition = targetPosition;
            StartCoroutine(MoveCoroutine());
        }
    }

    System.Collections.IEnumerator MoveCoroutine()
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(gridPosition.x, 0, gridPosition.y);
        float elapsedTime = 0f;

        while (elapsedTime < 1f / moveSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime * moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // assurer que le joueur finit bien sur la case
        isMoving = false;
    }

    void OnDrawGizmos()
    {
        if (playzoneCollider != null)
        {
            // dessiner la vraie zone en rouge en fonction du TilemapCollider2D
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(playzoneCollider.bounds.center, playzoneCollider.bounds.size);
        }
    }
}



