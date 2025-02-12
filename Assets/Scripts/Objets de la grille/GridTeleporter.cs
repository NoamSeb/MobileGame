using NaughtyAttributes;
using System;
using UnityEngine;

public class GridTeleporter : GridObject
{
    [ShowNonSerializedField] private Transform _otherTeleporterPos;
    bool IsNoSecondTeleporter => _otherTeleporterPos != null;

    [Button, ShowIf(nameof(IsNoSecondTeleporter))]
    void CreateSecondTeleporter()
    {
        GridTeleporter temp = Instantiate(this.gameObject).GetComponent<GridTeleporter>();

        SetOtherTeleporter(temp);
        temp.SetOtherTeleporter(this);
    }

    void SetOtherTeleporter(GridTeleporter tp)
    {
        if (_otherTeleporterPos == null)
        {
            _otherTeleporterPos = tp.transform;
        }
    }

    public static event Action<Vector2> OnTeleport;
    protected override void Effect()
    {
        OnTeleport?.Invoke(_otherTeleporterPos.position);
    }
}
