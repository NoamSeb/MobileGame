using System;
using UnityEngine;

public class GridExit : GridObject
{
    public static event Action OnLevelEnd;

    protected override void Effect()
    {
        OnLevelEnd?.Invoke();
    }
}
