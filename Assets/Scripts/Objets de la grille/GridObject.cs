using NaughtyAttributes;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    private Grid _grid;
    [ShowNonSerializedField] private Vector2Int _gridPosition;

    public bool IsImpassable {  get; private set; }

    void Start()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        _grid = GameManager.Instance.PlayGrid;

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        _gridPosition = new Vector2Int(cellPosition.x, cellPosition.y);
        transform.position = _grid.GetCellCenterWorld(cellPosition);
    }

    protected void SetImpassable()
    {
        IsImpassable = true;
    }
}
