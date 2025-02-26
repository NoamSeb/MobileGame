using UnityEngine;

public class GridFissure : GridObject
{
    private bool _isFissureOpening;
    protected override void Setup()
    {
        base.Setup();

        PlayerGridMovement.OnActionExecuted += HandleActionExecution;
    }

    protected override void Effect()
    {
        if (!_isFissureOpening && !IsImpassable) { _isFissureOpening = true; }
    }

    void HandleActionExecution()
    {
        if (_isFissureOpening && !IsImpassable) { SetImpassable(); }
    }
}
