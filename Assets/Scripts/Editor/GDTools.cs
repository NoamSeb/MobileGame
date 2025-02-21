using Codice.Client.Commands.Merge;
using Codice.CM.Client.Differences.Merge;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
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
    }

    bool _isInitialize;

    string[] _gridItemsNames = { };
    int _index = 0;

    void SpawnObject()
    {
        string objName = _gridItemsNames[_index];

        GameObject obj = (GameObject)Resources.Load("GDTools Prefabs/Grid Objects/" +  objName);

        Instantiate(obj, Vector3.zero, Quaternion.identity);
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

        if (GUILayout.Button("Enable Initialisation (DO NOT CLICK IF SCENE ALREADY SET UP)"))
        {
            if (_isInitialize) { _isInitialize = false; return; }
            _isInitialize = true;
        }

        if (_isInitialize)
        {
            if (GUILayout.Button("Create base objects for scene levels"))
            {
                GameObject[] objects = Resources.LoadAll<GameObject>("GDTools Prefabs/Initialisation");

                foreach (GameObject obj in objects)
                {
                    Instantiate(obj, Vector3.zero, Quaternion.identity);
                }

                SnapAllObjects();

                _isInitialize = false;
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
