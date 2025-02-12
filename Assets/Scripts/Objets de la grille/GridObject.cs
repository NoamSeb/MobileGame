using NaughtyAttributes;
using System;
using System.ComponentModel;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected Grid _grid;
    public Vector2Int GridPosition { get; private set; }
    [ShowNonSerializedField] protected Vector2Int _hoveredGridPosition;
    protected Vector2 oldPos;
    public bool IsImpassable { get; private set; }

    void Start()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        if (_grid == null) { _grid = GameManager.Instance.PlayGrid; }

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        GridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = _grid.GetCellCenterWorld(cellPosition);
    }

    [Button]
    public void UpdateGridPosEditor()
    {
        if (_grid == null)
        {
            var temp = GameObject.FindGameObjectWithTag("Playzone");
            DebugGridExistence(temp);
            _grid = temp.GetComponent<Grid>();
        }

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        _hoveredGridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
    }

    void DebugGridExistence(GameObject grid)
    {
        if (grid == null) { throw new MissingReferenceException("Grid doesn't exist in scene"); }
    }

    protected virtual void Effect() { throw new NotImplementedException(); }

    protected void SetImpassable()
    {
        IsImpassable = true;
    }
}