using System;
using UnityEngine;

public class GridExit : GridObject
{
    private SpriteRenderer _skin;
    public bool IsExitOpen { get; private set; }

    protected override void Setup()
    {
        base.Setup();
        GridComputer.OnComputerRepair += OpenExit;
        _skin = GetComponent<SpriteRenderer>();
        _skin.enabled = false;
    }

    void OpenExit()
    {
        IsExitOpen = true;
        _skin.enabled = true;
    }

    public static event Action OnLevelEnd;

    protected override void Effect()
    {
        if (IsExitOpen)
        {
            Debug.Log("ok");
            OnLevelEnd?.Invoke();
        }
    }
}
