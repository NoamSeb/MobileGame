using NaughtyAttributes;
using System;
using UnityEngine;

public class GridTeleporter : GridObject
{
    [ShowNonSerializedField] private Transform _otherTeleporter;
    bool IsNoSecondTeleporter()
    {
        return _otherTeleporter == null;
    }

    [Button, ShowIf(nameof(IsNoSecondTeleporter))]
    void CreateSecondTeleporter()
    {
        GridTeleporter temp = Instantiate(this.gameObject).GetComponent<GridTeleporter>();

        SetOtherTeleporter(temp);
        temp.SetOtherTeleporter(this);
    }

    void SetOtherTeleporter(GridTeleporter tp)
    {
        if (_otherTeleporter == null)
        {
            _otherTeleporter = tp.transform;
        }
    }

    public static event Action<Vector2> OnTeleport;
    protected override void Effect()
    {
        OnTeleport?.Invoke(_otherTeleporter.position);
    }
}
