using System;
using UnityEngine;

public class GridRepairTool : GridObject
{
    public static event Action OnRepairPickup;
    protected override void Effect()
    {
        OnRepairPickup?.Invoke();
        Destroy(gameObject);
    }
}
