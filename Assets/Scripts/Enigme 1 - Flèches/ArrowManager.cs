using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ArrowManager : MonoBehaviour
{
    [SerializeField] private List<ArrowData> _arrowList;

    private List<ArrowData> EnigmaTab;
    public List<ArrowData> GameTab => EnigmaTab;

    private void Awake()
    {
        // Perform the copy to EnigmaTab
        EnigmaTab = new List<ArrowData>(_arrowList);

        // Log EnigmaTab after copy
        Debug.Log($"EnigmaTab count after copy: {EnigmaTab.Count}");
    }
    
    public void MoveArrows(GameObject button)
    {
        Debug.Log($"EnigmaTab count at the beginning of function: {EnigmaTab.Count}");
        List<GameObject> ButtonList = button.GetComponentInParent<ArrowUI>().ArrowSlots;

        // Debugging ButtonList
        Debug.Log($"ButtonList count: {ButtonList.Count}");
        for (int i = 0; i < ButtonList.Count; i++)
        {
            Debug.Log($"ButtonList[{i}] name: {ButtonList[i].name}");
        }

        int arrowPositionInTab = ButtonList.IndexOf(button);

        // Debugging the arrow position
        Debug.Log($"Index of button in ButtonList: {arrowPositionInTab}");

        if (arrowPositionInTab < 0 || arrowPositionInTab >= EnigmaTab.Count)
        {
            Debug.LogError($"Index out of range in EnigmaTab! : {arrowPositionInTab}");
            Debug.LogError($"EnigmaTab length: {EnigmaTab.Count}");
            return;
        }

        ArrowData arrow = EnigmaTab[arrowPositionInTab];
        Debug.Log($"Arrow direction: {arrow.arrowDirection}");
        
        // switch (arrow.arrowDirection)
        // {
        //     case ArrowDirection.Left:
        //         Debug.Log("To the left !");
        //         break;
        //     case ArrowDirection.Right:
        //         Debug.Log("To the right !");
        //         break;
        //     default:
        //         break;
        // }
    }
    
    [Button]
    private void Reset()
    {
        EnigmaTab = new List<ArrowData>(_arrowList);
    }
}