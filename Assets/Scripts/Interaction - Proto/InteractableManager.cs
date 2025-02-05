using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableManager : MonoBehaviour
{
    public static InteractableManager Instance;

    PlayerInput _inputs;

    public InputAction TouchPos { get; private set; }
    public InputAction TouchPress { get; private set; }

    [SerializeField, Range(0f, 10f)] float _itemTouchRange = 1f, _itemInteractionRange = 2f;
    public float ItemTouchRange { get { return _itemTouchRange; } }
    public float ItemInteractionRange { get { return _itemInteractionRange; } }

    public GameObject Player { get; private set; }

    private void Awake()
    {
        if (Instance != this && Instance != null) { Destroy(Instance); }
        Instance = this;

        _inputs = GetComponent<PlayerInput>();

        TouchPos = _inputs.actions["TouchPosition"];
        TouchPress = _inputs.actions["TouchPress"];

        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null) { throw new System.Exception("No Player in Scene !"); }
    }
}
