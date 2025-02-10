using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class OldSwipeMovement : MonoBehaviour
{
    private PlayerInput _inputs;
    private InputAction _touchPress, _touchPos;

    private Vector3 _swipeStartPos;

    [SerializeField, Range(0f, 10f)] float _minimalSwipeDistance;

    private bool IsSwiping => _swipeStartPos != Vector3.zero;

    //Ce qu'on va envoyer pour donner la direction du joueur
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
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(_touchPos.ReadValue<Vector2>());
        _swipeStartPos = touchPos;
    }

    void EndSwipe()
    {
        Vector2 deltaPos = Camera.main.ScreenToWorldPoint(_touchPos.ReadValue<Vector2>()) - _swipeStartPos;

        //Si le mouvement de swipe est trop court, ou s'il a été effectué en dehors de la zone de jeu, on annule tout
        if(deltaPos.magnitude < _minimalSwipeDistance) // || !GameManager.Instance.PlayZone.Contains(_swipeStartPos)) 
        {
            _swipeStartPos = Vector2.zero;
            return; 
        }

        //On détermine ensuite la direction de la swipe. None est envoyé si le mouvement est impossible
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