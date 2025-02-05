using UnityEngine;

public class SlidingPuzzleCell : MonoBehaviour
{
    RectTransform _rect;

    [SerializeField] SlidingPuzzleCellDataBase _cellsDatabase;
    public SlidingPuzzleCellData Data { get; private set; }
    public bool IsMovable { get; private set; }    

    public void SimulateStart(int i, Vector2 anchor)
    {
        _rect = GetComponent<RectTransform>();
        _rect.anchorMax = anchor;
        _rect.anchorMin = anchor;
        _rect.anchoredPosition = Vector2.zero;

        Data = _cellsDatabase.slidingPuzzleCells[i];

        IsMovable = false;
    }

    public void MarkAsMovable()
    {
        IsMovable = true;
    }
}