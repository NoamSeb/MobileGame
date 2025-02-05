using UnityEngine;
using NaughtyAttributes;
using NUnit.Framework;

public class SlidingPuzzleFrame : MonoBehaviour
{
    [SerializeField] SlidingPuzzleCellDataBase cellsDatabase;
    [SerializeField] GameObject EmptyCell;
    [SerializeField] GameObject NotEmptyCell;

    Vector2[] anchors = new Vector2[9];

    void Start()
    {

    }

    void Update()
    {

    }

    [Button]
    void GetAllAnchors()
    {
        for (int i = 0; i < anchors.Length; i++)
        {
            {
                GameObject obj = transform.GetChild(i).gameObject;
                anchors[obj.transform.GetSiblingIndex()] = obj.GetComponent<RectTransform>().anchorMin;
            }
        }
    }
}