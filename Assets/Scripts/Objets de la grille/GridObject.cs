using MoreMountains.Feedbacks;
using NaughtyAttributes;
using System;
using System.ComponentModel;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    protected Grid _grid;
    [ShowNonSerializedField] protected Vector3Int _gridPosition;
    public Vector3Int GridPosition { get { return _gridPosition; } }
    public bool IsImpassable { get; private set; }

    [SerializeField] protected MMF_Player _loadingFeedbacks;

    void Start()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        _grid = GameManager.Instance.PlayGrid;
        DebugGridExistence(_grid.gameObject);

        PlaceItemInGrid();

        PlayerGridMovement.OnInteraction += Interaction;
        PlayerMirrorMovement.OnInteraction += MirrorInteraction;

        if (_loadingFeedbacks != null)
        {
            PlayFeedbacks(_loadingFeedbacks);
        }
    }

    [ExecuteInEditMode]
    public void PlaceItemInGrid()
    {
        _grid = GameObject.FindGameObjectWithTag("Playzone").GetComponent<Grid>();
        DebugGridExistence(_grid.gameObject);

        Vector3Int cellPosition = _grid.WorldToCell(transform.position);
        _gridPosition = new Vector3Int(cellPosition.x, cellPosition.y);
        transform.position = _grid.GetCellCenterWorld(_gridPosition);
    }

    void DebugGridExistence(GameObject grid)
    {
        if (grid == null) { throw new MissingReferenceException("Grid doesn't exist in scene"); }
    }

    protected void Interaction(GridObject obj)
    {
        if (obj == this) { Effect(); }
    }
    protected void MirrorInteraction(GridObject obj)
    {
        if (obj == this) { MirrorEffect(); }
    }
    protected virtual void Effect() { throw new NotImplementedException(); }
    protected virtual void MirrorEffect() { Effect(); }

    protected void SetImpassable()
    {
        IsImpassable = true;
    }

    [ExecuteInEditMode]
    protected virtual void BugFix() { }

    protected void PlayFeedbacks(MMF_Player player)
    {
        player.PlayFeedbacks();
    }
}