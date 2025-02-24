using UnityEngine;

public class GridLaser : GridObject
{
    private bool _isActive = false;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _laserCollider;
    private int _movementCount = 0; // compteur de mouvements

    [SerializeField] private Color _activeColor = Color.red; // couleur du laser activé
    [SerializeField] private Color _inactiveColor = Color.gray; // couleur du laser désactivé

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _laserCollider = GetComponent<Collider2D>();

        UpdateLaserState();

        PlayerGridMovement.OnActionExecuted += OnPlayerMoved;
    }

    private void OnDestroy()
    {
        PlayerGridMovement.OnActionExecuted -= OnPlayerMoved;
    }

    private void OnPlayerMoved()
    {
        _movementCount++;
        if (_movementCount % 2 == 0)
        {
            ToggleLaser();
        }
    }
    private void ToggleLaser()
    {
        _isActive = !_isActive;
        UpdateLaserState();
    }

    private void UpdateLaserState()
    {
        _spriteRenderer.color = _isActive ? _activeColor : _inactiveColor;
        _laserCollider.enabled = _isActive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isActive && other.CompareTag("Player"))
        {
            Oxygen.Instance.StopPlayer();
        }
    }
}

