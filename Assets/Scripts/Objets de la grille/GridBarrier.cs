using UnityEngine;

public class GridBarrier : GridObject
{
    protected override void Setup()
    {
        base.Setup();
        SetImpassable();
    }
}
