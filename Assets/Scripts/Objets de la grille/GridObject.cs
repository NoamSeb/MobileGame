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

    protected Level _parentLevel;
    protected Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null) { throw new MissingComponentException("Object is missing an animator"); }
        _parentLevel = GetComponentInParent<Level>();
        Level.OnLevelLoad += AnimateOnLevelLoad;
    }

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

    void AnimateOnLevelLoad(Level level)
    {
        if (level == _parentLevel)
        {
            _animator.SetTrigger("StartLoadingAnimation");
        }
    }

    [ExecuteInEditMode]
    protected virtual void BugFix() { }
}