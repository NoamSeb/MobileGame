using System;
using UnityEngine;
using NaughtyAttributes;

public class GridOxygenBottle : GridObject
{
    [SerializeField, ValidateInput(nameof(IsGreaterThanZero))] private int _oxygenRefillAmount;
    bool IsGreaterThanZero(int n) => n > 0;

    public static event Action<int> OnOxygenBottleRefill; 

    protected override void Effect()
    {
        OnOxygenBottleRefill?.Invoke(_oxygenRefillAmount);
    }
}
