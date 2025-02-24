using System;
using System.Collections;
using UnityEngine;

public class GridRotationLocker : GridObject
{
    public enum LockDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    [SerializeField] private LockDirection _rotateDirection;

    private int _rotation;

    private void OnValidate()
    {
        switch (_rotateDirection)
        {
            case LockDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                _rotation = 90;
                break;
            case LockDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                _rotation = 180;
                break;
            case LockDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                _rotation = 270;
                break;
            case LockDirection.Up:
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
}
