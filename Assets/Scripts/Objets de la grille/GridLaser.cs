using UnityEngine;

public class GridLaser : MonoBehaviour
{
    private bool isActive = false; 
    private SpriteRenderer spriteRenderer;
    private Collider2D laserCollider;

    [SerializeField] private Color activeColor = Color.red; 
    [SerializeField] private Color inactiveColor = Color.gray; 

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        laserCollider = GetComponent<Collider2D>();
        UpdateLaserState();
        InvokeRepeating(nameof(ToggleLaser), 0f, 0.4f); 
    }

    void ToggleLaser()
    {
        isActive = !isActive;
        UpdateLaserState();
    }

    void UpdateLaserState()
    {
        spriteRenderer.color = isActive ? activeColor : inactiveColor;
        laserCollider.enabled = isActive; 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            Oxygen.Instance.IsDead(); 
        }
    }
}
