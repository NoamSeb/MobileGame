using System;
using UnityEngine;
using NaughtyAttributes;

public class GridBatteryModifier : GridObject
{
    [SerializeField] private int _batteryModifyAmount;

    public static event Action<int> OnBatteryModification; 

    protected override void Effect()
    {
        OnBatteryModification?.Invoke(_batteryModifyAmount);
        Destroy(gameObject);
    }
}
