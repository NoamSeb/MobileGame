using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPuzzleCellDatabase", menuName = "SlidePuzzleCells", order = 0)]
public class SlidingPuzzleCellDataBase : ScriptableObject
{
    public string ID;

    public List<SlidingPuzzleCellData> slidingPuzzleCells = new();

    public SlidingPuzzleCellData FindCell(string id) => slidingPuzzleCells.Find(x => x.ID == id);
}