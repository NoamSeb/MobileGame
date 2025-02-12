using System;
using UnityEngine;

public class GridExit : GridObject
{
    public bool IsExitOpen { get; private set; }

    protected override void Setup()
    {
        base.Setup();
        GridComputer.OnComputerRepair += OpenExit;
    }

    void OpenExit()
    {
        IsExitOpen = true;
    }

    public static event Action OnLevelEnd;

    protected override void Effect()
    {
        if (IsExitOpen)
        {
            OnLevelEnd?.Invoke();
        }
    }
}
