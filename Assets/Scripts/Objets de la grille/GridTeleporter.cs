    using NaughtyAttributes;
using System;
using UnityEngine;

public class GridTeleporter : GridObject
{
    [SerializeField] private GridTeleporter _otherTeleporter;
    bool IsNoSecondTeleporter => _otherTeleporter == null;

    [Button, ShowIf(nameof(IsNoSecondTeleporter)), ExecuteInEditMode]
    public void CreateSecondTeleporter()
    {
        PlaceItemInGrid();
        GridTeleporter temp = Instantiate(this.gameObject).GetComponent<GridTeleporter>();
        temp.PlaceItemInGrid();

        SetOtherTeleporter(temp);
        temp.SetOtherTeleporter(this);
    }

    [ExecuteInEditMode]
    void SetOtherTeleporter(GridTeleporter tp)
    {
        if (_otherTeleporter == null)
        {
            _otherTeleporter = tp;
        }
    }

    public static event Action<Vector3Int> OnTeleport;
    protected override void Effect()
    {
        OnTeleport?.Invoke(_otherTeleporter.GridPosition);
    }

    public static event Action<Vector3Int> OnTeleportMirror;
    protected override void MirrorEffect()
    {
        OnTeleportMirror?.Invoke(_otherTeleporter.GridPosition);
    }
}
