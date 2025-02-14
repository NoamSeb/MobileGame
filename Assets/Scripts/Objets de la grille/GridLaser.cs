using UnityEngine;

public class GridLaser : GridObject
{
    private bool _isActive = false;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _laserCollider;

    [SerializeField] private Color _activeColor = Color.red; // couleur du laser activé
    [SerializeField] private Color _inactiveColor = Color.gray; // couleur du laser désactivé

    protected override void Setup()
    {
        base.Setup();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _laserCollider = GetComponent<Collider2D>();

        // on commence avec le laser désactivé
        UpdateLaserState();

        // abonnement aux actions du joueur pour alterner ON/OFF
        PlayerGridMovement.OnActionExecuted += ToggleLaser;
    }

    private void OnDestroy()
    {
        PlayerGridMovement.OnActionExecuted -= ToggleLaser;
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
            Debug.Log("Le joueur a touché un laser activé !");

            if (Oxygen.Instance == null)
            {
                Debug.LogError("Oxygen.Instance est NULL, on attend un frame...");
            }
            else
            {
                Oxygen.Instance.StopPlayer();
            }
        }
    }
}
