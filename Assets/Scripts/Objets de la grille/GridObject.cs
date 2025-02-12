using NaughtyAttributes;
using System;
using System.ComponentModel;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected Grid _grid;
    public Vector3Int GridPosition { get; private set; }
    [ShowNonSerializedField] protected Vector3Int _gridPositionMemory;
    public bool IsImpassable { get; private set; }

    void Start()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        _grid = GameManager.Instance.PlayGrid;

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        GridPosition = new Vector3Int(cellPosition.x, cellPosition.y);
        _gridPositionMemory = GridPosition;
        transform.position = _grid.GetCellCenterWorld(cellPosition);

        PlayerGridMovement.OnInteraction += Interaction;
    }

    [Button]
    public void CheckPositionInGrid()
    {
        if (_grid == null)
        {
            var temp = GameObject.FindGameObjectWithTag("Playzone");
            DebugGridExistence(temp);
            _grid = temp.GetComponent<Grid>();
        }

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        _gridPositionMemory = new Vector3Int(cellPosition.x, cellPosition.y);
    }

    void DebugGridExistence(GameObject grid)
    {
        if (grid == null) { throw new MissingReferenceException("Grid doesn't exist in scene"); }
    }

    protected void Interaction(GridObject obj)
    {
        if(obj == this) { Effect(); }
    }
    protected virtual void Effect() { throw new NotImplementedException(); }

    protected void SetImpassable()
    {
        IsImpassable = true;
    }
}