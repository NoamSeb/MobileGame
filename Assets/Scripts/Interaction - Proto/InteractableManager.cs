using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance;

    PlayerInput _inputs;

    public InputAction TouchPos { get; private set; }
    public InputAction TouchPress { get; private set; }

    [SerializeField, Range(0f, 10f)] float _itemInteractionDistance = 1f;
    public float ItemInteractionDistance { get { return _itemInteractionDistance; } }

    private void Awake()
    {
        if (Instance != this && Instance != null) { Destroy(Instance); }
        Instance = this;

        _inputs = GetComponent<PlayerInput>();

        TouchPos = _inputs.actions["TouchPosition"];
        TouchPress = _inputs.actions["TouchPress"];
    }
}
