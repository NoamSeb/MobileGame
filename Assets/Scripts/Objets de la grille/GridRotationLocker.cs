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
    public int Rotation { get { return _rotation; } }

    private void OnValidate()
    {
        switch (_rotateDirection)
        {
            case InitialMoveDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                _rotation = 90;
                break;
            case InitialMoveDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                _rotation = 180;
                break;
            case InitialMoveDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                _rotation = 270;
                break;
            case InitialMoveDirection.Up:
                transform.rotation = Quaternion.identity;
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
