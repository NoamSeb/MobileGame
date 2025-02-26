using System;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour{
    
    [SerializeField] private Image img;

    public void Awake()
    {
        img.color = new Color(142f / 255f, 31f / 255f, 35f / 255f, 1f);
    }
    
    /// <summary>
    /// Select the real level from hierarchie to display it
    /// </summary>
    /// <param name="level"></param>
    public void SelectLevel(GameObject level)
    {
        level.SetActive(true);
        Debug.Log(level.name);
    }

    [Button]
    public void Finish()
    {
        img.color = new Color(11f/255f, 190f/255f, 133f/255f, 1f);
        Debug.Log(img.color);
    }
}
