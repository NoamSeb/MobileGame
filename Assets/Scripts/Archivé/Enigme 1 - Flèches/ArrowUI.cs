using System;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ArrowUI : MonoBehaviour
{
    [SerializeField] private ArrowManager _arrowManager;
    [SerializeField] private GameObject _arrowSlotPrefab;

    [SerializeField] private Sprite _arrowImageEmpty;
    public Sprite EmptySprite => _arrowImageEmpty;
    [SerializeField] private Sprite _arrowImageLeft;
    [SerializeField] private Sprite _arrowImageRight;
    
    [SerializeField] 
    private List<GameObject> _arrowSlots = new List<GameObject>();
    public List<GameObject> ArrowSlots => _arrowSlots;
    private void Start()
    {
        for (int i = 0; i < _arrowManager.GameTab.Count; i++)
        {
            switch (_arrowManager.GameTab[i].arrowDirection)
            {
                case ArrowDirection.Empty:
                    _arrowSlots[i].GetComponent<Image>().sprite = _arrowImageEmpty;
                    continue;
                case ArrowDirection.Left:
                    _arrowSlots[i].GetComponent<Image>().sprite = _arrowImageLeft;
                    continue;
                case ArrowDirection.Right:
                    _arrowSlots[i].GetComponent<Image>().sprite = _arrowImageRight;
                    continue;
                case ArrowDirection.None:
                    continue;
                default:
                    break;
            }
        }
    }
}
