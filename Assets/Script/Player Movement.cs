using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isMoving = true;
        }
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        rb.position = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            rb.position = targetPosition;
            isMoving = false;
        }
    }
}
