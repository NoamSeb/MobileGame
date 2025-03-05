using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public class GridRotationLocker : GridObject
{
    public enum InitialMoveDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    [SerializeField] private InitialMoveDirection _rotateDirection;

    private int _rotation;

    [SerializeField, BoxGroup("Sprites")]
    private Sprite _spriteUp, _spriteLeft, _spriteRight, _spriteDown;

    private void OnValidate()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (_rotateDirection)
        {
            case InitialMoveDirection.Left:
                renderer.sprite = _spriteLeft;
                _rotation = 90;
                break;
            case InitialMoveDirection.Down:
                renderer.sprite = _spriteDown;
                _rotation = 180;
                break;
            case InitialMoveDirection.Right:
                renderer.sprite = _spriteRight;
                _rotation = 270;
                break;
            case InitialMoveDirection.Up:
                renderer.sprite = _spriteUp;
                _rotation = 0;
                break;
        }
    }

    public static event Action<int> OnRotate;
    protected override void Effect()
    {
        OnRotate?.Invoke(_rotation);
    }

    public static event Action<int> OnRotateMirror;
    protected override void MirrorEffect()
    {
        OnRotateMirror?.Invoke(_rotation);
    }
}
