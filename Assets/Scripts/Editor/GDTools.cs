#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEditor.UI;
using UnityEngine.Tilemaps;

public class GDTools : EditorWindow
{
    [MenuItem("Tools/GD Tools")]
    public static void ShowWindow()
    {
        GetWindow<GDTools>();
    }
    void SnapAllObjects()
    {
        foreach (GridObject obj in FindObjectsByType<GridObject>(FindObjectsSortMode.None))
        {
            obj.PlaceItemInGrid();
        }

        PlayerGridMovement player = FindFirstObjectByType<PlayerGridMovement>();
        if (player != null) { player.SetPositionInGrid(); }

        PlayerMirrorMovement mirror = FindFirstObjectByType<PlayerMirrorMovement>();
        if (mirror != null) { mirror.SetPositionInGrid(); }
    }

    string _ID;
    string[] _gridItemsNames = { };
    int _index = 0;

    void SpawnObject()
    {
        string objName = _gridItemsNames[_index];

        GameObject obj = (GameObject)Resources.Load("GDTools Prefabs/Grid Objects/" + objName);

        PrefabUtility.InstantiatePrefab(obj);
    }

    private void OnGUI()
    {
        if (_gridItemsNames.Length == 0)
        {
            GameObject[] temp = Resources.LoadAll<GameObject>("GDTools Prefabs/Grid Objects");
            List<string> temp2 = new();

            foreach (GameObject obj in temp)
            {
                if (obj != null) { temp2.Add(obj.name); }
            }

            _gridItemsNames = temp2.ToArray();
        }

        GUILayout.Label("Level Number");

        _ID = GUILayout.TextArea(_ID);

        if (GUILayout.Button("Assemble In Prefab"))
        {
            if (Convert.ToInt64(_ID) > 0)
            {
                GameObject newLevel = new("Level_" + _ID);
                GameObject newObjects = new("-------- OBJECTS --------");

                foreach (GridObject obj in FindObjectsByType<GridObject>(FindObjectsSortMode.None))
                {
                    obj.transform.SetParent(newObjects.transform);
                }

                FindFirstObjectByType<PlayerGridMovement>().transform.SetParent(newObjects.transform);
                if (FindFirstObjectByType<PlayerMirrorMovement>() != null)
                {
                    FindFirstObjectByType<PlayerMirrorMovement>().transform.SetParent(newObjects.transform);
                }

                FindFirstObjectByType<Grid>().transform.SetParent(newLevel.transform);
                GameObject.Find("UI").transform.SetParent(newLevel.transform);
                foreach (var obj in FindObjectsByType<Canvas>(FindObjectsSortMode.None))
                {
                    if (obj.name == "Background")
                    {
                        obj.transform.SetParent(newLevel.transform);
                    }
                }
                newObjects.transform.SetParent(newLevel.transform);
                newLevel.AddComponent<Level>();
            }
        }


        GUILayout.Label("Objects");

        _index = EditorGUILayout.Popup("Object to spawn", _index, _gridItemsNames);

        if (GUILayout.Button("Spawn selected object"))
        {
            SpawnObject();
        }

        if (GUILayout.Button("Snap all objects positions"))
        {
            SnapAllObjects();
        }

        /*if (GUILayout.Button("test"))
        {
            Tilemap map = FindFirstObjectByType<Tilemap>();
            map.CompressBounds();

            BoundsInt bounds = map.cellBounds;
            Vector3Int point = new(
            Mathf.FloorToInt(bounds.center.x),
            Mathf.FloorToInt(bounds.center.y),
            0);
            Vector3 realpoint = map.CellToWorld(point);
            Vector3 correctionX = Vector3.zero, correctionY = Vector3.zero;
            if (bounds.size.x % 2 != 0) { correctionX = Vector3.right / 2; }
            if (bounds.size.y % 2 != 0) { correctionY = Vector3.up / 2; }
            Debug.Log(realpoint + correctionX + correctionY);
            Debug.DrawLine(Vector3.zero, realpoint + correctionX + correctionY, Color.white, 5f);

            Debug.DrawLine(new(bounds.min.x, bounds.min.y), new(bounds.min.x, bounds.max.y), Color.red, 5f);
            Debug.DrawLine(new(bounds.min.x, bounds.max.y), new(bounds.max.x, bounds.max.y), Color.red, 5f);
            Debug.DrawLine(new(bounds.max.x, bounds.max.y), new(bounds.max.x, bounds.min.y), Color.red, 5f);
            Debug.DrawLine(new(bounds.max.x, bounds.min.y), new(bounds.min.x, bounds.min.y), Color.red, 5f);
        }*/
    }
}
#endif
