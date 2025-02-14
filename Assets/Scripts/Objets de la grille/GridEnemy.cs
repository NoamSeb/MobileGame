using UnityEngine;

public class GridEnemy : GridObject
{
    private float _moveDuration;

    protected override void Setup()
    {
        base.Setup();
        _moveDuration = GameManager.Instance.PlayerScript.MoveDuration;
    }


}
