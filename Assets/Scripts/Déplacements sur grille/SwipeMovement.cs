using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class SwipeMovement : MonoBehaviour
{
    private PlayerInput _inputs;
    private InputAction _touchPress, _touchPos;

    private Vector3 _swipeStartPos;

    private bool IsSwiping => _swipeStartPos != Vector3.zero;

    public enum SwipeDirection
    {
        None,
        Left,
        Up,
        Right,
        Down
    }

    public static event Action<SwipeDirection> OnSwipeEnd;

    void Start()
    {
        _inputs = GetComponent<PlayerInput>();
        _touchPress = _inputs.actions.FindAction("TouchPress");
        _touchPos = _inputs.actions.FindAction("TouchPosition");
    }

    void Update()
    {
        if (IsSwiping) { Debug.DrawLine(_swipeStartPos, Camera.main.ScreenToWorldPoint(_touchPos.ReadValue<Vector2>())); }

        if (_touchPress.WasPressedThisFrame()) { StartSwipe(); }

        if (_touchPress.WasReleasedThisFrame()) { EndSwipe(); }
    }

    void StartSwipe()
    {
        _swipeStartPos = Camera.main.ScreenToWorldPoint(_touchPos.ReadValue<Vector2>());
    }

    void EndSwipe()
    {
        Vector2 deltaPos = Camera.main.ScreenToWorldPoint(_touchPos.ReadValue<Vector2>()) - _swipeStartPos;

        if (Mathf.Abs(deltaPos.x) >= Mathf.Abs(deltaPos.y))
        {
            if (Vector2.Dot(deltaPos, Vector2.right) < 0) { OnSwipeEnd?.Invoke(SwipeDirection.Left); }
            else if (Vector2.Dot(deltaPos, Vector2.right) > 0) { OnSwipeEnd?.Invoke(SwipeDirection.Right); }
            else { OnSwipeEnd?.Invoke(SwipeDirection.None); }
        }
        else
        {
            if (Vector2.Dot(deltaPos, Vector2.up) < 0) { OnSwipeEnd?.Invoke(SwipeDirection.Down); }
            else if (Vector2.Dot(deltaPos, Vector2.up) > 0) { OnSwipeEnd?.Invoke(SwipeDirection.Up); }
            else { OnSwipeEnd?.Invoke(SwipeDirection.None); }
        }

        _swipeStartPos = Vector2.zero;
    }
}