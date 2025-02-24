using NaughtyAttributes;
using UnityEngine;
using System;

public class GridPusher : GridObject
{
    public enum PushDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    [SerializeField] private PushDirection _direction;

    private Vector2Int _pushVector;

    private void OnValidate()
    {
        switch (_direction)
        {
            case PushDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, 90); 
                _pushVector = Vector2Int.left; 
                break;
            case PushDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 180); 
                _pushVector = Vector2Int.down;
                break;
            case PushDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90); 
                _pushVector = Vector2Int.right;
                break;
            case PushDirection.Up:
                transform.rotation = Quaternion.identity; 
                _pushVector = Vector2Int.up;
                break;
        }
    }

    public static event Action<Vector2Int> OnPush;
    protected override void Effect()
    {
        OnPush?.Invoke(_pushVector);
    }
}
