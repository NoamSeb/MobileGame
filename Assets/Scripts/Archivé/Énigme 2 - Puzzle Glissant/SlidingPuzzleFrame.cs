using UnityEngine;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections.Generic;
using System;

public class SlidingPuzzleFrame : MonoBehaviour
{
    [SerializeField] GameObject _cellPrefab;

    [SerializeField, ShowIf(nameof(IsNotAllAnchorsSet))]
    Vector2[] _anchors =
        { new Vector2(.2f, .8f), new Vector2(.5f, .8f), new Vector2(.8f, .8f),
        new Vector2(.2f, .5f), new Vector2(.5f, .5f), new Vector2(.8f, .5f),
        new Vector2(.2f, .2f), new Vector2(.5f, .2f), new Vector2(.8f, .2f)};

    List<SlidingPuzzleCell> _slidingCells = new();

    SlidingPuzzleCell _emptyCell;
    int _emptyCellCollumn;
    enum EmptyCellRow
    {
        None,
        First,
        Second,
        Third
    }
    EmptyCellRow _emptyCellRow;

    void Start()
    {
        for (int i = 0; i < _anchors.Length; i++)
        {
            GameObject temp = Instantiate(_cellPrefab, transform);

            SlidingPuzzleCell cell = temp.GetComponent<SlidingPuzzleCell>();
            cell.SimulateStart(i, _anchors[i]);
            _slidingCells.Add(cell);

            if (cell.Data.IsEmptyCell && _emptyCell != null)
            {
                throw new System.Exception("This database contains two empty cells, which shouldn't be possible");
            }
            else if (cell.Data.IsEmptyCell && _emptyCell == null)
            {
                _emptyCell = cell;
            }
        }
    }

    private void Update()
    {
        TagEmptyCell();
        TagMovablePuzzleCells();

#if UNITY_EDITOR || UNITY_ANDROID
        Debug();
#endif
    }

    EmptyCellRow TagCellRow(int posInSiblings)
    {
        if (posInSiblings < 3) { return EmptyCellRow.First; }
        else if (posInSiblings < 6) { return EmptyCellRow.Second; }
        else { return EmptyCellRow.Third; }
    }

    void TagEmptyCell()
    {
        int emptyPos = _emptyCell.gameObject.transform.GetSiblingIndex();
        _emptyCellCollumn = emptyPos % 3;
        _emptyCellRow = TagCellRow(emptyPos);
    }

    void TagMovablePuzzleCells()
    {
        foreach (var cell in _slidingCells)
        {
            int tempPos = cell.gameObject.transform.GetSiblingIndex();

            if (tempPos % 3 == _emptyCellCollumn) { cell.MarkAsMovable(); }

            if (TagCellRow(tempPos) == _emptyCellRow) { cell.MarkAsMovable(); }
        }
    }

    bool IsNotAllAnchorsSet()
    {
        for (int i = 0; i < _anchors.Length; i++)
        {
            if (_anchors[i] == Vector2.zero) { return true; }
        }
        return false;
    }

    void Debug()
    {
        if (_emptyCellRow == EmptyCellRow.None)
        {
            throw new Exception("The empty cell's row has been assigned to none, which shouldn't be possible");
        }
    }
}