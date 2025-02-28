using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 5f;
    private Vector2 targetPosition;
    private bool isMoving = false;
    private Rigidbody2D rb;

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        touchPositionAction = playerInput.actions["TouchPosition"];
        touchPressAction = playerInput.actions["TouchPress"];

        targetPosition = transform.position; 
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE 
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = clickPosition;
            isMoving = true;
        }
#endif

#if UNITY_ANDROID 
        if (touchPressAction.WasPressedThisFrame())
        {
            Vector2 touchPosition = touchPositionAction.ReadValue<Vector2>();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            targetPosition = worldPosition;
            isMoving = true;
        }
#endif
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
        rb.position = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            rb.position = targetPosition;
            isMoving = false;
        }
    }
}