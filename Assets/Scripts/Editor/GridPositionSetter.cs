using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

public class GridPositionSetter : EditorWindow
{
    [MenuItem("Tools/Grid Position Setter")]
    public static void ShowWindow()
    {
        GetWindow<GridPositionSetter>();
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Positions");

        if (GUILayout.Button("Set objects positions"))
        {
            foreach (GridObject obj in FindObjectsByType<GridObject>(FindObjectsSortMode.None))
            {
                obj.PlaceItemInGrid();
            }
        }

        if(GUILayout.Button("Set player position"))
        {
            PlayerGridMovement player = FindFirstObjectByType<PlayerGridMovement>();
            player.SetPositionInGrid();
        }
    }
}
