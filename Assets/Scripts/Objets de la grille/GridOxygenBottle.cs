using System;
using UnityEngine;

public class GridOxygenBottle : GridObject
{
    [SerializeField] private int _oxygenRefillAmount;

    public static event Action<int> OnOxygenBottleRefill; 

    protected override void Effect()
    {
        OnOxygenBottleRefill?.Invoke(_oxygenRefillAmount);
    }
}
