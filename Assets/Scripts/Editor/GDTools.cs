using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

public class GDTools : EditorWindow
{
    [MenuItem("Tools/GD tools")]
    public static void ShowWindow()
    {
        GetWindow<GDTools>();
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
