using NaughtyAttributes;
using System.Diagnostics;
using System;
using UnityEngine;

public class GridComputer : GridObject
{
    [ShowNonSerializedField] private int _neededToolsNumber;
    [ShowNonSerializedField] private int _currentTools;
    private bool _isEnoughToolsAcquired;
    public bool IsBroken { get; private set; }

    public static event Action OnComputerRepair;
    protected override void Setup()
    {
        base.Setup();

        IsBroken = true;
        _neededToolsNumber = FindObjectsByType<GridRepairTool>(FindObjectsSortMode.None).Length;
        GridRepairTool.OnRepairPickup += AddTools;
    }

    void AddTools()
    {
        if (_currentTools < _neededToolsNumber)
        {
            _currentTools++;
            if (_currentTools == _neededToolsNumber) { _isEnoughToolsAcquired = true; }
        }
    }

    protected override void Effect()
    {
        if (_isEnoughToolsAcquired && IsBroken)
        {
            IsBroken = false;
            OnComputerRepair?.Invoke();
        }
    }
}
