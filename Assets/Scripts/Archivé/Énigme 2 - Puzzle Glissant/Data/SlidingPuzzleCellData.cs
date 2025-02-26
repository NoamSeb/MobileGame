using System;
using UnityEngine;

[Serializable]
public struct SlidingPuzzleCellData
{
    public string ID;
    public Texture2D Texture;
    public bool IsEmptyCell;
    public int CorrectPosition;

    public SlidingPuzzleCellData(string ID, Texture2D Texture, bool IsEmptyCell, int CorrectPosition)
    {
        this.ID = ID;
        this.Texture = Texture;
        this.IsEmptyCell = IsEmptyCell;
        this.CorrectPosition = CorrectPosition;
    }
}
