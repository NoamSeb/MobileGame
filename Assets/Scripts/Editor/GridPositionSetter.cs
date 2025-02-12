using UnityEditor;
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
        if(GUILayout.Button("Set all positions"))
        {
            foreach(var obj in FindObjectsByType<GridObject>(FindObjectsSortMode.None))
            {
                obj.UpdateGridPosEditor();
            }
        }
    }
}
