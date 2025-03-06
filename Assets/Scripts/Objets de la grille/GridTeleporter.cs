using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;

public class GridTeleporter : GridObject
{
    [SerializeField] private GridTeleporter _otherTeleporter;
    public GridTeleporter OtherTeleporter {  get { return _otherTeleporter; } }
    bool IsNoSecondTeleporter => _otherTeleporter == null;

    #if UNITY_EDITOR
    [Button, ShowIf(nameof(IsNoSecondTeleporter)), ExecuteInEditMode]
    public void CreateSecondTeleporter()
    {
        PlaceItemInGrid();
        GameObject obj = Resources.Load<GameObject>("GDTools Prefabs/Grid Objects/Teleporter");
        GameObject objInstantiated = (GameObject)PrefabUtility.InstantiatePrefab(obj);
        GridTeleporter temp = objInstantiated.GetComponent<GridTeleporter>();
        temp.PlaceItemInGrid();

        SetOtherTeleporter(temp);
        temp.SetOtherTeleporter(this);
    }
    #endif

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
