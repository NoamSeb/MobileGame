using System;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    public static event Action OnLevelEnd;
    public void EndLevel()
    {
        OnLevelEnd?.Invoke();
        gameObject.SetActive(false);
    }
}
