#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

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

        GUILayout.Label("Initialize");

        if (GUILayout.Button("Set UI Tabs"))
        {
            Canvas[] canvas = FindObjectsByType<Canvas>(FindObjectsSortMode.None);

            foreach (Canvas temp in canvas)
            {
                temp.worldCamera = Camera.main;
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
    }
}
#endif
