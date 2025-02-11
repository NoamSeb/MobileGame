using System.Diagnostics;
using UnityEngine;

public class GridComputer : GridObject
{
    private int _neededToolsNumber;
    private int _currentTools;
    private bool _isEnoughToolsAcquired;
    public bool IsBroken { get; private set; } //Utiliser ça pour contrôler la couleur de l'écran, et le sprite de l'ordi

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
        if(_isEnoughToolsAcquired && IsBroken)
        {
            IsBroken = false;
        }
    }
}
