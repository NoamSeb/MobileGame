using NaughtyAttributes;
using System;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected Grid _grid;
    public Vector2Int GridPosition { get; private set; }

    public bool IsImpassable {  get; private set; }

    void Start()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        _grid = GameManager.Instance.PlayGrid;

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        GridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = _grid.GetCellCenterWorld(cellPosition);
    }

    protected virtual void Effect() { throw new NotImplementedException(); }

    protected void SetImpassable()
    {
        IsImpassable = true;
    }
}