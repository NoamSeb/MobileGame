using System;
using UnityEngine;

[Serializable]
public struct ArrowData
{
    // [SerializeField] private Sprite _arrowSprite;
    // public Sprite ArrowSprite => _arrowSprite;

    public ArrowDirection arrowDirection;
}

public enum ArrowDirection
{
    None,
    Empty,
    Left,
    Right,
}
